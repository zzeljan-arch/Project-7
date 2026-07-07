using System;
using System.Collections.Generic;
using System.Linq;

namespace PG.World.Simulation
{
    public static class ZoneEvolutionBarRegistry
    {
        public static readonly Dictionary<RegionId, List<ZoneEvolutionBarDefinition>> BarsByRegion = new()
        {
            {
                RegionId.NorthernRealm,
                new()
                {
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "EternalWinter",
                        Name = "Eternal Winter",
                        Description = "The realm freezes. Blizzards rage. Life retreats.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.3,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Northern Realm succumbs to Eternal Winter!")
                    },
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "AgeOfBandits",
                        Name = "Age of Bandits",
                        Description = "Lawlessness spreads. Brigands control the roads.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.4,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Northern Realm falls into the Age of Bandits!")
                    },
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "UndeadPower",
                        Name = "Undead Power",
                        Description = "Death stirs. Tombs crack open. The living are few.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.2,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Northern Realm becomes an Undead Realm!")
                    },
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "DivineOrder",
                        Name = "Divine Order",
                        Description = "Faith strengthens. Temples rise. Civilization flourishes.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.5,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Northern Realm ascends under Divine Order!")
                    },
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "DawnOfGodsHidden",
                        Name = "Dawn of the Gods",
                        Description = "A hidden mythic current reshapes the Northern Realm beneath the surface.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.1,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Northern Realm enters the Dawn of the Gods.")
                    }
                }
            },
            {
                RegionId.DesertKingdom,
                new()
                {
                    new ZoneEvolutionBarDefinition
                    {
                        Id = "MerchantsRepublic",
                        Name = "Merchants Republic",
                        Description = "Trade houses dominate politics and commerce in the desert realm.",
                        MinValue = 0,
                        MaxValue = 100,
                        StartingValue = 0,
                        DecayAmount = 0.3,
                        OnMilestoneReached = (world, milestone) => { },
                        OnMaxReached = (world) => world?.Log(LogCategory.Simulation, "[WORLD] Desert Kingdom transforms into the Merchants Republic!")
                    }
                }
            }
        };
    }

    public static class LongTermEventRegistry
    {
        public static readonly List<LongTermEventDefinition> AllEvents = new()
        {
            new LongTermEventDefinition
            {
                Id = "DragonAwakening",
                Name = "Dragon Awakening",
                Condition = world =>
                    world.ProgressionBars.TryGetValue("MagicalAwareness", out var arcaneBar)
                    && arcaneBar.Value >= 60
                    && world.Regions.Values.Where(r => r.CurrentTier >= WorldTier.Tier3).Count() >= 1,
                OnTrigger = world =>
                {
                    world.Log(LogCategory.Player, "A powerful dragon stirs in the high mountains.");
                    if (world.Regions.TryGetValue(RegionId.Highlands, out var region))
                    {
                        region.CurrentPath = EvolutionPath.DragonRoost;
                        region.CurrentTier = WorldTier.Tier3;
                    }
                }
            }
        };
    }

    public static class HiddenWorldBarRegistry
    {
        public static readonly List<EventBarDefinition> HiddenBarDefinitions = new()
        {
            new EventBarDefinition
            {
                Id = "AncientEvil",
                Name = "Ancient Evil",
                Description = "An unseen ancient menace grows beneath forgotten ruins.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0.03,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 45,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] The Black Eclipse darkens the sky as Ancient Evil awakens."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "SevenRelics" },
                    UnlockedEncounterIds = new() { "AncientCryptRitual" },
                    ZoneEvolutionInfluences = new() { ("UndeadPower", 8) },
                    SettlementModifications = new() { ("StoneHaven", "Fear", 12), ("HolyTemple", "Faith", -10) },
                    WorldEffects = new() { "The Black Eclipse covers the north in omen and dread." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "BlackEclipse",
                        Name = "Black Eclipse",
                        Description = "An eclipse unleashes cult panic and grave unrest.",
                        UnlockedEncounterIds = new() { "EclipseCultProcession" },
                        UnlockedQuestIds = new() { "SevenRelics" },
                        SettlementModifications = new() { ("StoneHaven", "Fear", 8), ("HolyTemple", "Hope", -6) },
                        ZoneEvolutionInfluences = new() { ("UndeadPower", 6) },
                        HiddenBarInfluences = new() { ("TheVeil", 8) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "TempleActivity", -4), (RegionId.NorthernRealm, "BanditPatrol", 5) },
                        WorldEffects = new() { "Blackened daylight drives cults and grave robbers into the open." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "BuriedKingWakes",
                        Name = "The Buried King Wakes",
                        Description = "An ancient sovereign stirs beneath the frost.",
                        UnlockedEncounterIds = new() { "BuriedKingHerald" },
                        UnlockedQuestIds = new() { "SealTheBarrows" },
                        SettlementModifications = new() { ("CrossRoads", "Fear", 10), ("StoneHaven", "Crime", 6) },
                        ZoneEvolutionInfluences = new() { ("AgeOfBandits", 4), ("UndeadPower", 8) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "BanditPatrol", 6) },
                        WorldEffects = new() { "Tombs crack open and pretenders gather around the old bloodline." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "DragonAwakeningHidden",
                Name = "Dragon Awakening",
                Description = "Deep draconic forces stir from bones, magma, and relics.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0.02,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 40,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] Ancient dragons reawaken across the mountains."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "DragonBonePilgrimage" },
                    UnlockedEncounterIds = new() { "LastGiant" },
                    SettlementModifications = new() { ("StoneHaven", "Fear", 8), ("CrossRoads", "Hope", -6) },
                    WorldEffects = new() { "Dragon bones stir and mountain roads become perilous." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "BonefireDrake",
                        Name = "Bonefire Drake",
                        Description = "A drake rises from an ancient ossuary.",
                        UnlockedEncounterIds = new() { "BonefireDrake" },
                        UnlockedQuestIds = new() { "DragonBonePilgrimage" },
                        SettlementModifications = new() { ("StoneHaven", "Fear", 7), ("CrossRoads", "Military", 4) },
                        ZoneEvolutionInfluences = new() { ("EternalWinter", 3) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "RangerActivity", 5) },
                        WorldEffects = new() { "A skeletal drake circles the passes and scorches abandoned watchtowers." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "VolcanicEcho",
                        Name = "Volcanic Echo",
                        Description = "Dormant mountain fire answers draconic memory.",
                        UnlockedEncounterIds = new() { "MagmaRelicSeekers" },
                        UnlockedQuestIds = new() { "CollectorsArchive" },
                        SettlementModifications = new() { ("StoneHaven", "Hope", -4), ("CrossRoads", "Influence", 5) },
                        HiddenBarInfluences = new() { ("TheVeil", 4) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "MerchantCaravan", -3) },
                        WorldEffects = new() { "Tremors and smoke force old mines closed while relic hunters flood the roads." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "OldGods",
                Name = "Old Gods",
                Description = "Forgotten deities regain whispers of worship.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0.03,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 35,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] Altars of the Old Gods answer with omens."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "AgelessPriestInquiry" },
                    ZoneEvolutionInfluences = new() { ("DivineOrder", 6), ("DawnOfGodsHidden", 10) },
                    SettlementModifications = new() { ("HolyTemple", "Faith", 14), ("HolyTemple", "Influence", 6) },
                    WorldEffects = new() { "Forgotten altars awaken and draw pilgrims back into the north." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "PilgrimFlood",
                        Name = "Pilgrim Flood",
                        Description = "Pilgrims of the old pantheon swarm hidden shrines.",
                        UnlockedEncounterIds = new() { "ForgottenPilgrims" },
                        UnlockedQuestIds = new() { "AgelessPriestInquiry" },
                        SettlementModifications = new() { ("HolyTemple", "Faith", 10), ("HolyTemple", "Culture", 6) },
                        ZoneEvolutionInfluences = new() { ("DawnOfGodsHidden", 8) },
                        WorldEffects = new() { "Old hymns return to the roads and hidden shrines gain guardians." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "Godspeakers",
                        Name = "The Godspeakers",
                        Description = "Mystics claim direct speech from forgotten powers.",
                        UnlockedQuestIds = new() { "InvestigateRumors" },
                        HiddenBarInfluences = new() { ("DawnOfGods", 10) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "TempleActivity", 6) },
                        WorldEffects = new() { "Prophets and frauds alike remake local faith and politics." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "KingsStability",
                Name = "King's Stability",
                Description = "The stability of the crown and realm authority.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 45,
                DecayAmount = 0.01,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 30,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] The crown unifies the realm under a lasting peace."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "RiverDoubleCrossInvestigation" },
                    SettlementModifications = new() { ("StoneHaven", "Loyalty", 10), ("CrossRoads", "Hope", 8) },
                    WorldEffects = new() { "Royal law stabilizes roads and trade across the realm." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "RoyalCircuit",
                        Name = "Royal Circuit",
                        Description = "Inspectors and judges sweep the north.",
                        UnlockedQuestIds = new() { "RiverDoubleCrossInvestigation" },
                        SettlementModifications = new() { ("StoneHaven", "Loyalty", 6), ("CrossRoads", "Crime", -6) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "BanditPatrol", -8), (RegionId.NorthernRealm, "MerchantCaravan", 5) },
                        WorldEffects = new() { "The crown's inspectors tighten road law and tax collection." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "BorderCharter",
                        Name = "Border Charter",
                        Description = "A new charter empowers frontier settlements.",
                        UnlockedEncounterIds = new() { "RoyalCharterEnvoy" },
                        ZoneEvolutionInfluences = new() { ("DivineOrder", 3), ("DawnOfGodsHidden", 4) },
                        SettlementModifications = new() { ("StoneHaven", "Influence", 8), ("CrossRoads", "Military", 5) },
                        WorldEffects = new() { "Frontier towns gain new banners, patrol rights, and obligations." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "CivilWar",
                Name = "Civil War",
                Description = "Factions drift toward open conflict.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 35,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] Civil War erupts across the realm."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "BjornLastStand" },
                    ZoneEvolutionInfluences = new() { ("AgeOfBandits", 10) },
                    SettlementModifications = new() { ("StoneHaven", "Fear", 15), ("CrossRoads", "Crime", 12) },
                    WorldEffects = new() { "The realm fractures into armed camps and rival banners." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "RoyalistPurge",
                        Name = "Royalist Purge",
                        Description = "Royalists and rebels begin open reprisals.",
                        UnlockedEncounterIds = new() { "RoyalistPurge" },
                        UnlockedQuestIds = new() { "BjornLastStand" },
                        SettlementModifications = new() { ("StoneHaven", "Fear", 12), ("CrossRoads", "Crime", 10) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "BanditPatrol", 7) },
                        WorldEffects = new() { "Banners burn, taxes fail, and militias seize the roads." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "PeasantRising",
                        Name = "Peasant Rising",
                        Description = "Villages revolt against nobles and merchants alike.",
                        UnlockedQuestIds = new() { "ProtectFromWildlife" },
                        ZoneEvolutionInfluences = new() { ("AgeOfBandits", 6) },
                        HiddenBarInfluences = new() { ("AncientEvil", 4) },
                        WorldEffects = new() { "Power vacuums create brigand kings and ruined storehouses." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "TheVeil",
                Name = "The Veil",
                Description = "The barrier between worlds grows thin.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0.02,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 35,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] The Veil tears, and impossible horrors cross over."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "SilentFlockInquiry" },
                    UnlockedEncounterIds = new() { "WhiteWolf" },
                    SettlementModifications = new() { ("StoneHaven", "Fear", 10), ("HolyTemple", "Faith", -8) },
                    WorldEffects = new() { "The boundary between worlds weakens around shrines and forests." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "VeilTear",
                        Name = "Veil Tear",
                        Description = "A rift opens and something watches from the other side.",
                        UnlockedEncounterIds = new() { "VeilTear" },
                        UnlockedQuestIds = new() { "SilentFlockInquiry" },
                        SettlementModifications = new() { ("StoneHaven", "Fear", 9), ("HolyTemple", "Faith", -5) },
                        HiddenBarInfluences = new() { ("AncientEvil", 5) },
                        WorldEffects = new() { "Dreams and disappearances spread around the north." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "GhostRoad",
                        Name = "The Ghost Road",
                        Description = "An impossible road appears at dusk between towns.",
                        UnlockedEncounterIds = new() { "GhostRoadTravellers" },
                        ZoneEvolutionInfluences = new() { ("DawnOfGodsHidden", 6) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "MerchantCaravan", 4) },
                        WorldEffects = new() { "Travelers reach towns too soon, or never arrive at all." }
                    }
                }
            },
            new EventBarDefinition
            {
                Id = "DawnOfGods",
                Name = "Dawn of the Gods",
                Description = "A hidden world story arc where myth returns to daily life.",
                Category = "Hidden",
                IsHidden = true,
                IsGlobal = true,
                StartingValue = 0,
                DecayAmount = 0.02,
                Threshold = 100,
                MinValue = 0,
                MaxValue = 100,
                CooldownTurns = 50,
                Repeatable = true,
                CanProgress = world => true,
                OnThresholdReached = world => world.Log(LogCategory.Simulation, "[WORLD EVENT] Dawn of the Gods begins. Miracles and monsters walk openly."),
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestIds = new() { "InvestigateRumors", "CollectorsArchive" },
                    ZoneEvolutionInfluences = new() { ("DawnOfGodsHidden", 20), ("DivineOrder", 8) },
                    SettlementModifications = new() { ("HolyTemple", "Faith", 12), ("StoneHaven", "Hope", 10) },
                    WorldEffects = new() { "Miracles spread openly and ancient beasts stir across the world." }
                },
                PossibleOutcomes = new()
                {
                    new HiddenEventOutcome
                    {
                        Id = "MiracleSpring",
                        Name = "Miracle Spring",
                        Description = "A sacred spring erupts with healing water.",
                        UnlockedQuestIds = new() { "InvestigateRumors", "CollectorsArchive" },
                        ZoneEvolutionInfluences = new() { ("DawnOfGodsHidden", 10), ("DivineOrder", 4) },
                        SettlementModifications = new() { ("HolyTemple", "Faith", 8), ("StoneHaven", "Hope", 8) },
                        WorldEffects = new() { "Pilgrims and beasts converge on a newly awakened sacred spring." }
                    },
                    new HiddenEventOutcome
                    {
                        Id = "NornProcession",
                        Name = "The Norn Procession",
                        Description = "Thread-seers walk openly and rewrite destinies.",
                        UnlockedEncounterIds = new() { "NornProcession" },
                        UnlockedQuestIds = new() { "SevenRelics" },
                        HiddenBarInfluences = new() { ("OldGods", 6) },
                        EventBarInfluences = new() { (RegionId.NorthernRealm, "TempleActivity", 5), (RegionId.NorthernRealm, "RangerActivity", 4) },
                        WorldEffects = new() { "Fate omens spread across the north and old prophecies begin to matter again." }
                    }
                }
            }
        };
    }

    public static class UniqueNpcRegistry
    {
        public static readonly List<NpcDefinition> AllNpcs = new()
        {
            new NpcDefinition
            {
                Id = "BjornOneEyed",
                Name = "Bjorn the One-Eyed",
                Role = "Old Ranger",
                Age = 67,
                Traits = new() { "Veteran", "Gruff", "Honorable", "Knows hidden caves" },
                StartsAlive = true,
                SuccessorId = "LeifBjornson"
            },
            new NpcDefinition
            {
                Id = "LeifBjornson",
                Name = "Leif Bjornson",
                Role = "Young Ranger",
                Age = 24,
                Traits = new() { "Resolute", "Tracker" },
                StartsAlive = false
            },
            new NpcDefinition
            {
                Id = "SisterElara",
                Name = "Sister Elara",
                Role = "Priestess",
                Age = 31,
                Traits = new() { "Compassionate", "Disciplined" },
                HiddenTraits = new() { "Secret Cultist" },
                StartsAlive = true
            }
        };
    }

    public static class WorldRumorRegistry
    {
        public static readonly List<WorldRumorDefinition> Rumors = new()
        {
            new WorldRumorDefinition
            {
                Id = "LightsInMountains",
                Text = "I heard lights in the mountains.",
                TruthChance = 0.25,
                RumorGroupId = "DragonSigns",
                ShardsRequired = 4,
                QuestUnlockId = "DragonBonePilgrimage"
            },
            new WorldRumorDefinition
            {
                Id = "NoBirdsStoneHaven",
                Text = "There are no birds near Stone Haven.",
                TruthChance = 0.20,
                RumorGroupId = "VeilSigns",
                ShardsRequired = 4,
                QuestUnlockId = "SilentFlockInquiry"
            },
            new WorldRumorDefinition
            {
                Id = "MerchantRiverTwice",
                Text = "A merchant crossed the river twice.",
                TruthChance = 0.30,
                RumorGroupId = "RoyalTradeSigns",
                ShardsRequired = 4,
                QuestUnlockId = "RiverDoubleCrossInvestigation"
            },
            new WorldRumorDefinition
            {
                Id = "PriestStoppedAging",
                Text = "The old priest stopped aging.",
                TruthChance = 0.15,
                RumorGroupId = "OldGodsSigns",
                ShardsRequired = 4,
                QuestUnlockId = "AgelessPriestInquiry"
            }
        };
    }
}
