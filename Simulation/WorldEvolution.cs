using System;
using System.Collections.Generic;
using System.Linq;

namespace PG.World.Simulation
{
    public static class EncounterRegistry
    {
        public static readonly List<EncounterDefinition> AllEncounters = new()
        {
            new EncounterDefinition
            {
                Id = "BanditAmbush",
                Name = "Bandit Ambush",
                Summary = "A group of bandits jumps out from cover and demands your valuables.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Regions[world.Player.CurrentRegion].Definition.Biome != BiomeType.Arcane,
                Weight = world => 1.0,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Fight the bandits.", Execute = world => { world.Regions[world.Player.CurrentRegion].EventBars["BanditPatrol"].AddProgress(12); return new EncounterResult { Message = "Combat ensues. (Bandit Patrol +12)", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Pay them off.", Execute = world => { world.Player.SpendGold(12); world.Regions[world.Player.CurrentRegion].EventBars["BanditPatrol"].AddProgress(3); return new EncounterResult { Message = "You escape by paying tribute. (Bandit Patrol +3)", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Attempt to flee.", Execute = world => new EncounterResult { Message = "You escape the ambush.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "StrandedMerchant",
                Name = "Stranded Merchant",
                Summary = "A trader is stranded after a broken wagon and needs aid.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => true,
                Weight = world => 0.9,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Repair the wagon and help.", Execute = world => { world.Regions[world.Player.CurrentRegion].EventBars["MerchantCaravan"].AddProgress(16); return new EncounterResult { Message = "You repair the wagon! Merchant Caravan activity +16", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Offer to escort them.", Execute = world => { world.Regions[world.Player.CurrentRegion].EventBars["MerchantCaravan"].AddProgress(10); return new EncounterResult { Message = "You guide them safely. Merchant Caravan activity +10.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Take advantage and steal.", Execute = world => new EncounterResult { Message = "You grab some cargo.", Success = true, PlayerVisible = true } }
                }
            }
        };
    }

    // ===== CORE ENGINE - WorldState =====
    public class WorldState
    {
        public static WorldState CurrentGlobalState { get; private set; }

        public ulong Seed { get; private set; }
        public DeterministicRandom Rng { get; private set; }
        public WorldClock Clock { get; private set; }
        public Dictionary<RegionId, RegionState> Regions { get; private set; } = new();
        public PlayerState Player { get; private set; }
        public EconomyState Economy { get; private set; }
        public Dictionary<FactionType, FactionState> Factions { get; private set; } = new();
        public Dictionary<string, ProgressionBarState> ProgressionBars { get; private set; } = new();
        public List<EncounterDefinition> Encounters { get; private set; } = new();
        public List<LongTermEventDefinition> LongTermEvents { get; private set; } = new();
        public List<QuestDefinition> QuestDefinitions { get; private set; } = new();
        public List<string> SimulationHistory { get; private set; } = new();
        public bool ShowLogs { get; set; } = true;
        public bool AutoResolveEncounters { get; set; } = false;
        public int TicksSinceLastEvolution { get; private set; }

        private NoveltyTracker _noveltyTracker = new();

        public WorldState()
        {
            Clock = new WorldClock();
            Player = new PlayerState { CurrentRegion = RegionId.NorthernRealm, Health = 100, MaxHealth = 100, Gold = SimulationConfig.DefaultStartingGold };
            Economy = new EconomyState();
        }

        private void InitializeEconomy()
        {
            if (Rng == null) return;
            
            foreach (var item in ItemRegistry.AllItems)
            {
                var basePrice = item.BaseValue * (100 + Rng.Next(-20, 20)) / 100.0;
                Economy.MarketPrices[item.Id] = basePrice;
            }
        }

        private void InitializeFactions()
        {
            foreach (var faction in FactionRegistry.AllFactions)
            {
                Factions[faction.Type] = faction;
                Player.Reputation[faction.Type] = 0;
            }
        }

        private void InitializeRegistries()
        {
            Encounters = EncounterRegistry.AllEncounters;
            LongTermEvents = LongTermEventRegistry.AllEvents;
            QuestDefinitions = QuestRegistry.AllQuests;
        }

        public static WorldState CreateCampaign(ulong seed, RegionId startingRegion)
        {
            var world = new WorldState
            {
                Seed = seed,
                Rng = new DeterministicRandom(seed)
            };

            CurrentGlobalState = world;
            world.Log(LogCategory.Simulation, $"Campaign seeded with {seed} starting at {startingRegion}");

            // Initialize all 7 regions
            var regionIds = new[] { RegionId.NorthernRealm, RegionId.DarkForest, RegionId.DesertKingdom, RegionId.Swamp, RegionId.Highlands, RegionId.InfernalLands, RegionId.ArcaneEmpire };
            foreach (var regionId in regionIds)
            {
                var tier = world.Rng.Probability(SimulationConfig.StartingTier2Chance) ? WorldTier.Tier2 : WorldTier.Tier1;
                var definition = WorldDefinitions.GetDefinition(regionId);
                var regionState = new RegionState(definition, tier, EvolutionPath.None);

                // Initialize EventBars from EventBarRegistry
                foreach (var barDef in EventBarRegistry.AllEventBarDefinitions)
                {
                    regionState.EventBars[barDef.Id] = new EventBarState(barDef);
                }

                // Initialize ZoneEvolutionBars
                if (ZoneEvolutionBarRegistry.BarsByRegion.TryGetValue(regionId, out var zoneBarDefs))
                {
                    foreach (var zoneBarDef in zoneBarDefs)
                    {
                        regionState.ZoneEvolutionBars[zoneBarDef.Id] = new ZoneEvolutionBarState(zoneBarDef);
                    }
                }

                // Initialize Settlements
                foreach (var settlementDef in SettlementRegistry.AllSettlements)
                {
                    if (settlementDef.Region == regionId)
                    {
                        regionState.Settlements[settlementDef.Id] = new SettlementState(settlementDef);
                    }
                }

                world.Regions[regionId] = regionState;
            }

            // Initialize progression bars
            foreach (var barDef in ProgressionBarRegistry.GlobalBarDefinitions)
            {
                world.ProgressionBars[barDef.Id] = new ProgressionBarState(barDef);
            }

            // Initialize economy, factions, and registries
            world.InitializeEconomy();
            world.InitializeFactions();
            world.InitializeRegistries();

            world.Player.CurrentRegion = startingRegion;
            return world;
        }

        public void Log(LogCategory category, string message)
        {
            if (ShowLogs)
                SimulationLog.Log(message, category);
            SimulationHistory.Add(message);
        }

        public void AdvanceWorld(int minutes)
        {
            Clock.AdvanceByMinutes(minutes);
            UpdateProgression();
            UpdateMarketPrices();
        }

        private void UpdateProgression()
        {
            TicksSinceLastEvolution++;
            foreach (var region in Regions.Values)
            {
                foreach (var bar in region.EventBars.Values)
                {
                    double oldValue = bar.Value;
                    bar.ApplyDecay();
                    if (Math.Abs(oldValue - bar.Value) > 0.01)
                    {
                        double progress = (bar.Value / bar.Definition.Threshold) * 100;
                        Log(LogCategory.Simulation, $"[{region.Definition.Name}] {bar.Definition.Name}: {oldValue:F1} -> {bar.Value:F1} ({progress:F0}% to threshold)");
                    }
                    
                    bar.CheckCompletion();
                    if (bar.NeedsCompletionProcessing())
                    {
                        ProcessEventBarCompletion(bar);
                        bar.MarkCompletionProcessed();
                    }
                }
                
                foreach (var zoneBar in region.ZoneEvolutionBars.Values)
                {
                    if (zoneBar.ModificationHistory.Count > 0)
                    {
                        var lastMod = zoneBar.ModificationHistory.Last();
                        Log(LogCategory.Simulation, $"[{region.Definition.Name}] {zoneBar.Definition.Name}: {lastMod} (milestone: {zoneBar.CurrentMilestone})");
                        zoneBar.ModificationHistory.Clear();
                    }
                }
            }
        }

        private void ProcessEventBarCompletion(EventBarState bar)
        {
            var definition = bar.Definition;
            Log(LogCategory.Simulation, $"[COMPLETION] {definition.Name} reached threshold ({bar.Value:F0}/{definition.Threshold})");
            
            foreach (var encounterId in definition.OnCompletion.UnlockedEncounterIds)
            {
                Log(LogCategory.Simulation, $"  → Unlocks encounter: {encounterId}");
            }

            foreach (var questLineId in definition.OnCompletion.UnlockedQuestLineIds)
            {
                Log(LogCategory.Simulation, $"  → Unlocks questline: {questLineId}");
            }

            foreach (var (settlementId, effectType, amount) in definition.OnCompletion.SettlementModifications)
            {
                var region = Regions.Values.FirstOrDefault(r => r.Settlements.ContainsKey(settlementId));
                if (region?.Settlements.TryGetValue(settlementId, out var settlement) == true)
                {
                    Log(LogCategory.Simulation, $"  → {settlement.Definition.Name} {effectType}: +{amount}");
                    if (effectType == "Wealth") settlement.ModifyWealth(amount);
                    else if (effectType == "Safety") settlement.ModifySafety(amount);
                    else if (effectType == "MerchantActivity") settlement.ModifyMerchantActivity(amount);
                }
            }
        }

        private void UpdateMarketPrices()
        {
            foreach (var itemId in Economy.MarketPrices.Keys.ToList())
            {
                var volatility = Rng.Next(-10, 10) / 100.0;
                Economy.MarketPrices[itemId] *= (1.0 + volatility * SimulationConfig.MarketVolatility);
            }
        }

        public void Explore()
        {
            Log(LogCategory.Player, "You venture forth into the surrounding land.");
            AdvanceWorld(SimulationConfig.ExploreMinutes);
            var encounterRoll = Rng.Probability(SimulationConfig.EncounterChance);
            if (encounterRoll)
            {
                var encounter = RollEncounter();
                if (encounter != null)
                {
                    Log(LogCategory.Player, $"Encounter: {encounter.Name} - {encounter.Summary}");
                    PresentEncounter(encounter);
                    return;
                }
            }

            // Try to discover a settlement by RNG
            var region = Regions[Player.CurrentRegion];
            var undiscoveredSettlements = region.Settlements.Values.Where(s => !s.Discovered).ToList();
            if (undiscoveredSettlements.Count > 0 && Rng.Probability(0.30))
            {
                var discoveredSettlement = undiscoveredSettlements[Rng.Next(undiscoveredSettlements.Count)];
                discoveredSettlement.Discover();
                Log(LogCategory.Player, $"Through your exploration, you discover the settlement of {discoveredSettlement.Definition.Name}! It has been added to your fast travel locations.");
                return;
            }

            var discoveryMessage = DiscoverLandmark();
            Log(LogCategory.Player, discoveryMessage);
        }

        private string DiscoverLandmark()
        {
            var region = Regions[Player.CurrentRegion];
            if (region.Definition.Biome == BiomeType.Forest && Rng.Probability(0.25))
                return "You discover an overgrown shrine hidden beneath the trees.";
            if (region.Definition.Biome == BiomeType.Mountain && Rng.Probability(0.20))
                return "You find a narrow pass with carvings of an old dragon cult.";
            if (region.Definition.Biome == BiomeType.Swamp && Rng.Probability(0.20))
                return "You stumble upon a flooded ruin with strange lights.";
            if (Rng.Probability(0.15))
                return "Your exploration reveals a small treasure cache of useful supplies.";
            return "The landscape shifts subtly as you move; nothing remarkable appears yet.";
        }

        private EncounterDefinition RollEncounter()
        {
            var region = Regions[Player.CurrentRegion];
            var validEncounters = Encounters.Where(e => e.CanOccur(this)).ToList();
            if (validEncounters.Count == 0) return null;

            var totalWeight = validEncounters.Sum(e => e.Weight(this));
            var roll = Rng.NextDouble() * totalWeight;
            double accumulated = 0;

            foreach (var encounter in validEncounters)
            {
                accumulated += encounter.Weight(this);
                if (roll <= accumulated) return encounter;
            }

            return validEncounters.LastOrDefault();
        }

        private void PresentEncounter(EncounterDefinition encounter)
        {
            if (!ShowLogs || AutoResolveEncounters)
            {
                var autoChoice = encounter.Options[Rng.Next(encounter.Options.Count)];
                ResolveEncounter(encounter, autoChoice);
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"--- {encounter.Name} ---");
            Console.WriteLine(encounter.Summary);
            for (int i = 0; i < encounter.Options.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {encounter.Options[i].Description}");
            }

            while (true)
            {
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out var selection) && selection >= 1 && selection <= encounter.Options.Count)
                {
                    ResolveEncounter(encounter, encounter.Options[selection - 1]);
                    break;
                }

                Console.WriteLine("Invalid selection. Please enter the number of your choice.");
            }

            Console.WriteLine();
        }

        private void ResolveEncounter(EncounterDefinition encounter, EncounterOption option)
        {
            var result = option.Execute(this);
            Log(LogCategory.Player, result.Message);
        }

        public void Rest()
        {
            var region = Regions[Player.CurrentRegion];
            var activities = GetAvailableRestActivities(region);
            Log(LogCategory.Player, "You take time to rest and recover.");
            foreach (var activity in activities)
            {
                Log(LogCategory.Player, $"- {activity}");
            }

            Player.Heal(SimulationConfig.RestHours * SimulationConfig.RestHealPerHour);
            Log(LogCategory.Player, $"You restore health to {Player.Health:F0}/{Player.MaxHealth}.");
            AdvanceWorld(SimulationConfig.RestHours * 60);
        }

        private List<string> GetAvailableRestActivities(RegionState region)
        {
            var activities = new List<string> { "Sleep", "Meditate", "Sharpen weapons" };
            if (region.Definition.HasWater)
                activities.Add("Fish");
            if (region.Definition.HasTown)
                activities.Add("Visit the tavern");
            activities.Add("Read a book");
            return activities;
        }

        public bool Travel(RegionId destination)
        {
            if (!Regions.ContainsKey(destination))
            {
                Log(LogCategory.Player, "That destination is not part of your current campaign.");
                return false;
            }

            if (destination == Player.CurrentRegion)
            {
                Log(LogCategory.Player, "You are already in that region.");
                return false;
            }

            var region = Regions[Player.CurrentRegion];
            Log(LogCategory.Player, $"You prepare to leave {region.Definition.Name}.");
            Log(LogCategory.Player, "You are about to leave this region. Returning will no longer be possible.");

            string input;
            if (AutoResolveEncounters)
            {
                input = "y";
            }
            else
            {
                Console.Write("Confirm travel? (y/n): ");
                input = Console.ReadLine()?.Trim().ToLower();
            }

            if (input != "y")
            {
                Log(LogCategory.Player, "You decide to stay for now.");
                return false;
            }

            region.IsLocked = true;
            Player.CurrentRegion = destination;
            Log(LogCategory.Player, $"You travel to {Regions[destination].Definition.Name}. It will continue evolving while you were away.");
            AdvanceWorld(SimulationConfig.ExploreMinutes);
            return true;
        }

        public void VisitTown()
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            
            if (discoveredSettlements.Count == 0)
            {
                Log(LogCategory.Player, "You have not yet discovered any settlements in this region. Explore to find towns!");
                return;
            }

            SettlementState selectedSettlement;
            if (discoveredSettlements.Count == 1)
            {
                selectedSettlement = discoveredSettlements[0];
                Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
            }
            else
            {
                if (!ShowLogs || AutoResolveEncounters)
                {
                    selectedSettlement = discoveredSettlements[Rng.Next(discoveredSettlements.Count)];
                    Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Discovered Settlements:");
                    for (int i = 0; i < discoveredSettlements.Count; i++)
                    {
                        Console.WriteLine($"[{i + 1}] {discoveredSettlements[i].Definition.Name}");
                    }

                    while (true)
                    {
                        Console.Write("Choose destination: ");
                        var input = Console.ReadLine();
                        if (int.TryParse(input, out var selection) && selection >= 1 && selection <= discoveredSettlements.Count)
                        {
                            selectedSettlement = discoveredSettlements[selection - 1];
                            Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
                            break;
                        }

                        Console.WriteLine("Invalid selection. Please enter a valid settlement number.");
                    }

                    Console.WriteLine();
                }
            }

            Log(LogCategory.Player, "You arrive in town and listen for rumors.");
            AdvanceWorld(60);
            Log(LogCategory.Player, "Merchants haggle openly and faction agents watch for trouble.");
            Player.AddItem("RumorFragment", 1);

            if (ShowLogs && !AutoResolveEncounters)
                OfferTownQuests();
            else
                AutoAcceptTownQuests();

            HandleTurnInRequests();
            CompleteQuestsInTown();
        }

        private void HandleTurnInRequests() { }
        private void OfferTownQuests() => Log(LogCategory.Player, "The town is quiet today. No new opportunities present themselves.");
        private void AutoAcceptTownQuests() { }

        public void VisitMarketplace()
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            
            if (discoveredSettlements.Count == 0)
            {
                Log(LogCategory.Player, "There is no marketplace here. You must discover a settlement first by exploring.");
                return;
            }

            var settlement = discoveredSettlements.Count == 1 
                ? discoveredSettlements[0]
                : discoveredSettlements[Rng.Next(discoveredSettlements.Count)];

            Log(LogCategory.Player, $"You browse the marketplace in {settlement.Definition.Name}.");
            AdvanceWorld(45);
            var item = ItemRegistry.AllItems[Rng.Next(ItemRegistry.AllItems.Count)];
            var price = Economy.MarketPrices[item.Id];
            Log(LogCategory.Player, $"A merchant offers {item.Name} for {price:F0} gold.");
            if (Player.Gold >= price)
            {
                Player.SpendGold(price);
                Player.AddItem(item.Id, 1);
                Log(LogCategory.Player, $"You purchase {item.Name}.");
            }
            else
            {
                Log(LogCategory.Player, "You do not have enough gold.");
            }
        }

        public void InventorySummary()
        {
            Log(LogCategory.Player, $"Gold: {Player.Gold:F0}");
            if (Player.Inventory.Count == 0)
            {
                Log(LogCategory.Player, "Inventory is empty.");
                return;
            }

            foreach (var kvp in Player.Inventory)
            {
                var item = ItemRegistry.GetItem(kvp.Key);
                Log(LogCategory.Player, $"{item.Name}: {kvp.Value}");
            }
        }

        public void CharacterSummary()
        {
            Log(LogCategory.Player, $"Health: {Player.Health:F0}/{Player.MaxHealth}");
            Log(LogCategory.Player, "Reputation:");
            foreach (var rep in Player.Reputation)
            {
                Log(LogCategory.Player, $"  {rep.Key}: {rep.Value:F1}");
            }
        }

        public void QuestLogSummary() => Log(LogCategory.Player, "No active quests.");
        public void ReadBook() => Log(LogCategory.Player, "You study an ancient tome and gain wisdom.");
        public void Fish() => Log(LogCategory.Player, "You fish but catch nothing of value.");
        public void Camp() => Log(LogCategory.Player, "You camp under the stars and recover strength.");
        public void Craft() => Log(LogCategory.Player, "You lack the materials to craft anything useful.");
        public bool TurnInQuest() => false;
        public bool AcceptQuest() => false;
        private void CompleteQuestsInTown() { }

        public void PerformAutomaticAction(bool questMode)
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            var undiscoveredSettlements = region.Settlements.Values.Where(s => !s.Discovered).ToList();
            
            // Intelligent action selection based on game state - STAY IN REGION
            if (Player.Health < Player.MaxHealth * 0.4)
            {
                // Low health: prioritize resting
                Rest();
            }
            else if (undiscoveredSettlements.Count > 0 && Rng.Probability(0.4))
            {
                // Explore to find settlements (lower weight than before)
                Explore();
            }
            else if (discoveredSettlements.Count > 0 && Rng.Probability(0.3))
            {
                // Visit discovered towns occasionally
                VisitTown();
            }
            else
            {
                // Default: explore to progress EventBars and trigger encounters
                Explore();
            }
        }

        public override string ToString()
        {
            var region = Regions[Player.CurrentRegion];
            var sb = new System.Text.StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Time: {Clock.CurrentTime}");
            sb.AppendLine($"Player Region: {region.Definition.Name}");
            sb.AppendLine($"Health: {Player.Health:F0}/{Player.MaxHealth}, Gold: {Player.Gold:F0}");
            sb.AppendLine("Regions:");
            foreach (var r in Regions.Values)
            {
                sb.AppendLine($"  {r.Definition.Name}: {r.CurrentPath} (Tier {(int)r.CurrentTier})");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public class WorldCampaign
    {
        private readonly WorldState _world;
        public int ActionCount { get; private set; }
        public WorldState World => _world;
        public Dictionary<RegionId, RegionState> Regions => _world.Regions;
        public List<string> EventLog => _world.SimulationHistory;

        public WorldCampaign(WorldState world)
        {
            _world = world;
            ActionCount = 0;
        }

        public void ExecuteAction(PlayerAction action)
        {
            ActionCount++;
        }
    }

    public class NoveltyTracker
    {
        public List<WorldState> Campaigns { get; private set; } = new();
        public Dictionary<string, int> PathOccurrences { get; private set; } = new();

        public void AddCampaign(WorldState world)
        {
            Campaigns.Add(world);
        }

        public double CalculateNovelty(WorldState world = null)
        {
            return Campaigns.Count > 0 ? 1.0 / Campaigns.Count : 1.0;
        }

        public string GenerateReport()
        {
            return $"Tracked {Campaigns.Count} campaigns";
        }
    }
}
