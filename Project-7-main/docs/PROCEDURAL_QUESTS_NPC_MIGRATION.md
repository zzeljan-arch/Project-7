# Procedural Quests & NPC Migration System

**Status:** Foundation design with implementation pointers  
**Scope:** Procedurally generated quests, NPC factions, region migration, player agency over evolution  
**Goal:** Dynamic storytelling, player choices that shape world state, prevent repetitive quests

---

## 1. Quest System Architecture

### Quest Types

Quests are procedurally generated from templates based on world state (region, path, tier):

| Quest Type | Trigger | Reward | Purpose |
|-----------|---------|--------|---------|
| **Conquest** | NPC wants territory taken from enemies | XP, materials, faction rep | Kill specific enemy types in a region |
| **Gathering** | NPC needs crafting materials | Gold, materials, faction rep | Farm specific materials from region |
| **Delivery** | Transport goods between regions | Gold, exp, faction rep | Move items between NPCs (encourages travel) |
| **Investigation** | NPC wants intel on enemy activity | XP, legendary fragment, rep | Survive waves of enemies, report back |
| **Bounty** | Kill a specific powerful enemy | Gold, unique item, rep | Hunt named enemies (mini-bosses) |
| **Ritual** | NPC wants environmental change via magic | XP, faction rep, world state change | Empower or corrupt a region (player choice!) |
| **Intervention** | Stop NPC faction from migrating/disappearing | Gold, guaranteed legendary, rep | Prevent favorite NPCs from leaving |

---

## 2. NPC Factions & Migration

### Faction System

Each region has **2-4 factions** that evolve with the path:

#### Northern Realm Factions

| Path | Dominant Faction | Primary Goal | Enemy Type |
|------|------------------|--------------|-----------|
| **Age of Raiders** | Raider Clans | Conquer & plunder | Bandits, mercenaries |
| **Eternal Winter** | Ice Druids | Preserve nature | Frost spirits, corrupted beasts |
| **Sunken Halls** | Abyssal Cult | Summon ancient horrors | Cultists, abominations |
| **Dominion of Decay** | Plague Keepers | Spread disease & weakness | Undead, wither-touched |

#### Emerald Forest Factions

| Path | Dominant Faction | Primary Goal | Enemy Type |
|------|------------------|--------------|-----------|
| **Nature's Descent** | Forest Wardens | Protect & restore nature | Poachers, corrupted treants |
| **Blighted Bloom** | Blight Collective | Spread rot & decay | Infected beasts, fungal horrors |
| **Overgrowth** | Primal Shamans | Evolve all life | Mutant beasts, shamans |
| **Withering Dusk** | Shadow Circle | Merge shadow with nature | Wraiths, shadow druids |

*(All 7 regions have similar faction breakdowns = 28 factions total, 4 per region/path)*

### Faction Attributes

```csharp
public class Faction
{
    public string FactionId { get; set; }            // "raider_clans_northern_realm"
    public string FactionName { get; set; }          // "Raider Clans"
    public string RegionId { get; set; }             // "northern_realm"
    public string EvolutionPath { get; set; }        // "age_of_raiders"
    
    // Faction power (0-100)
    public int Strength { get; set; }                // Starts at 50, rises if quests completed
    
    // Territory control (0-100 per adjacent region)
    public Dictionary<string, int> TerritoryInfluence { get; set; }
    
    // Reputation system (player relationship)
    public int PlayerReputation { get; set; }        // -100 to +100
    
    // Migration tracking
    public int TurnsUntilMigration { get; set; }     // Days until faction leaves/evolves
    public string MigrationTarget { get; set; }      // Where faction goes if undefended
    
    // Passive perks (if player is friendly)
    public List<string> FactionPerks { get; set; }   // Discounts, quest bonuses, etc.
}
```

---

## 3. NPC Migration Rules

### How NPCs Leave (Without Player Intervention)

Every in-game day, factions evaluate their situation:

```
For each faction in region:
  1. Calculate "strength" = faction_power × enemy_count × crafting_economy
  2. Calculate "player_support" = player_reputation × active_quests × gold_spent_with_npcs
  
  If strength < 30 AND player_support < 10:
    → Faction is dying. Countdown timer starts (3-7 days)
    → If not rescued by player: faction migrates to adjacent region with similar path
    → All NPCs disappear; quests become unavailable
    → Resources they controlled are seized by enemy faction
  
  If strength > 70 AND player_support > 50:
    → Faction is thriving. Expands influence to adjacent regions
    → New NPCs spawn (brings new quests)
    → Territory control increases
```

**Migration Example:**
- Raider Clans in Northern Realm are weak (strength 25)
- Player hasn't done quests (reputation -10)
- 5-day countdown begins
- If player completes 5+ raids on Bandits OR does 3 faction quests: countdown resets
- If player does nothing: Raider Clans migrate to adjacent Infernal Lands region
  - Previous Northern Realm location now controlled by Ice Druids
  - Raider quests no longer available in Northern Realm (only in Infernal Lands)
  - Player has "missed" raider storyline for that region

---

## 4. Procedural Quest Generation

### Quest Template System

Quests are generated at runtime from templates:

```csharp
public abstract class QuestTemplate
{
    public string TemplateId { get; set; }
    public string TemplateName { get; set; }
    public QuestType Type { get; set; }
    
    // Conditions for spawning this quest
    public int MinimumTier { get; set; }
    public int MaximumTier { get; set; }
    public List<string> AvailableInRegions { get; set; }
    public List<string> AvailableInPaths { get; set; }
    
    public abstract Quest GenerateQuest(
        Faction faction,
        RegionState currentRegion,
        PlayerState player);
}

// Example: Conquest Quest Template
public class ConquestQuestTemplate : QuestTemplate
{
    public override Quest GenerateQuest(Faction faction, RegionState region, PlayerState player)
    {
        // Randomly pick enemy archetype for this region
        var enemyArchetype = EnemyDatabase.Instance.GetRandomArchetype(region);
        var targetCount = DeterministicRandom.Range(10, 25);  // Kill 10-25 enemies
        
        return new ConquestQuest()
        {
            Title = $"Purge the {enemyArchetype.DisplayName}",
            Description = $"{faction.FactionName} needs you to eliminate {targetCount} {enemyArchetype.DisplayName} in {region.RegionName}.",
            Faction = faction,
            TargetEnemyArchetype = enemyArchetype.ArchetypeName,
            TargetCount = targetCount,
            RewardXP = 500 + (region.CurrentTier * 100),
            RewardGold = 1000 + (region.CurrentTier * 200),
            RewardMaterials = new() { (enemyArchetype.LootTableId, 5) }
        };
    }
}

// Example: Gathering Quest Template
public class GatheringQuestTemplate : QuestTemplate
{
    public override Quest GenerateQuest(Faction faction, RegionState region, PlayerState player)
    {
        var materials = MaterialDatabase.Instance.GetMaterialsForRegion(region.RegionId);
        var targetMaterial = materials[DeterministicRandom.Range(0, materials.Count)];
        var targetCount = DeterministicRandom.Range(5, 20);
        
        return new GatheringQuest()
        {
            Title = $"Gather {targetMaterial.DisplayName}",
            Description = $"{faction.FactionName} needs {targetCount}× {targetMaterial.DisplayName}. Collect them from enemies in {region.RegionName}.",
            Faction = faction,
            TargetMaterial = targetMaterial.MaterialId,
            TargetCount = targetCount,
            RewardXP = 300 + (region.CurrentTier * 50),
            RewardGold = 800 + (region.CurrentTier * 150),
            RewardReputation = 10
        };
    }
}

// Example: Delivery Quest Template
public class DeliveryQuestTemplate : QuestTemplate
{
    public override Quest GenerateQuest(Faction faction, RegionState region, PlayerState player)
    {
        var sourceNpc = faction.GetRandomNPC();
        var targetRegion = region.GetAdjacentRegion();  // Force travel
        var targetFaction = targetRegion.GetRandomFaction();
        var item = DeterministicRandom.Pick(new[] { "supplies", "weapons", "herbs", "gold" });
        
        return new DeliveryQuest()
        {
            Title = $"Deliver {item} to {targetRegion.RegionName}",
            Description = $"Take {item} from {sourceNpc.Name} in {region.RegionName} to {targetRegion.RegionName}.",
            Faction = faction,
            FromNPC = sourceNpc,
            ToRegion = targetRegion.RegionId,
            ToFaction = targetFaction,
            Item = item,
            RewardXP = 400 + (region.CurrentTier * 100),
            RewardGold = 1500 + (region.CurrentTier * 300),
            RewardReputation = 15  // Extra rep for helping both factions
        };
    }
}
```

### Ritual Quest Template (Player Agency)

**Special case:** Ritual quests let players reshape world evolution:

```csharp
public class RitualQuestTemplate : QuestTemplate
{
    public override Quest GenerateQuest(Faction faction, RegionState region, PlayerState player)
    {
        // Ritual quest offers player CHOICE: empower faction OR corrupt the land
        
        var choice1 = new RitualChoice()
        {
            Title = "Empower the Faction",
            Description = "Perform a ritual to strengthen this faction's connection to the land.",
            Consequence = () =>
            {
                // If chosen: boost faction strength, faction spreads to adjacent region
                faction.Strength = Math.Min(faction.Strength + 30, 100);
                region.ModifyPathInfluence(faction.EvolutionPath, +20);  // Path spreads
                player.AddReputation(faction, 25);
                return "The ritual succeeds! This faction grows stronger.";
            }
        };
        
        var choice2 = new RitualChoice()
        {
            Title = "Corrupt the Ritual",
            Description = "Sabotage the ritual to weaken this faction and corrupt the land.",
            Consequence = () =>
            {
                // If chosen: weaken faction, change path evolution, enemies mutate
                faction.Strength = Math.Max(faction.Strength - 30, 10);
                region.ModifyPathInfluence(faction.EvolutionPath, -20);  // Path retreats
                region.TriggerPathAdvancementInAdjacentRegion();  // Accelerates evolution elsewhere
                player.AddReputation(faction, -50);  // Faction becomes hostile
                return "The ritual corrupts! This land twists in unexpected ways.";
            }
        };
        
        return new RitualQuest()
        {
            Title = $"Perform Ritual in {region.RegionName}",
            Description = $"Complete a ritual to influence {region.RegionName}'s fate.",
            Faction = faction,
            Region = region,
            Choices = new[] { choice1, choice2 },
            RewardXP = 1000 + (region.CurrentTier * 200),
            RewardGold = 2000 + (region.CurrentTier * 400),
            RewardReputation = 30  // Faction grateful OR hostile (choice-dependent)
        };
    }
}
```

---

## 5. Intervention Quests (Prevent NPC Migration)

### Rescue Faction Questline

When a faction is at risk of migration, an **Intervention Quest** appears:

```csharp
public class InterventionQuestTemplate : QuestTemplate
{
    public override Quest GenerateQuest(Faction faction, RegionState region, PlayerState player)
    {
        if (faction.TurnsUntilMigration > 0 && faction.PlayerReputation < 10)
        {
            // Faction is dying. Player can save them.
            return new InterventionQuest()
            {
                Title = $"Save the {faction.FactionName}",
                Description = $"The {faction.FactionName} are retreating! Complete 3 tasks to convince them to stay.",
                Faction = faction,
                Objectives = new[]
                {
                    new QuestObjective()
                    {
                        Type = QuestObjectiveType.KillEnemies,
                        Description = "Defeat 50 enemies threatening their territory",
                        TargetCount = 50
                    },
                    new QuestObjective()
                    {
                        Type = QuestObjectiveType.GatherMaterial,
                        Description = "Gather 20× rare materials to support their economy",
                        MaterialId = faction.RegionSignatureMaterial(),
                        TargetCount = 20
                    },
                    new QuestObjective()
                    {
                        Type = QuestObjectiveType.DefeatBoss,
                        Description = "Defeat the enemy commander threatening their stronghold",
                        TargetBoss = region.GetUndefeatedBoss()
                    }
                ],
                RewardXP = 2000 + (region.CurrentTier * 300),
                RewardGold = 5000 + (region.CurrentTier * 500),
                RewardReputation = 100,  // Massive reputation boost
                RewardLegendary = true,   // Guaranteed legendary item
                OnCompletion = () =>
                {
                    faction.TurnsUntilMigration = int.MaxValue;  // They stay forever
                    faction.Strength = 70;  // Revitalized
                    region.LockPathToCurrentEvolution();  // Path won't change (for now)
                }
            };
        }
        
        return null;  // No intervention quest if faction is stable
    }
}
```

---

## 6. Quest Availability & Progression

### Daily Quest Generation

Every in-game day, available quests are regenerated:

```csharp
public class DailyQuestGenerator
{
    public static List<Quest> GenerateQuestsForDay(
        PlayerState player,
        RegionState currentRegion,
        WorldState worldState)
    {
        var quests = new List<Quest>();
        
        // Get all factions in current region
        var factions = worldState.GetFactionsInRegion(currentRegion.RegionId);
        
        // Generate 2-4 quests per faction
        foreach (var faction in factions)
        {
            int questCount = DeterministicRandom.Range(2, 5);
            
            for (int i = 0; i < questCount; i++)
            {
                // Pick a random quest template
                var template = PickQuestTemplate(faction, currentRegion, player);
                var quest = template.GenerateQuest(faction, currentRegion, player);
                
                if (quest != null)
                    quests.Add(quest);
            }
        }
        
        // Add intervention quests if any faction is dying
        foreach (var faction in factions)
        {
            if (faction.TurnsUntilMigration > 0 && faction.PlayerReputation < 10)
            {
                var interventionTemplate = new InterventionQuestTemplate();
                var quest = interventionTemplate.GenerateQuest(faction, currentRegion, player);
                if (quest != null)
                    quests.Add(quest);
            }
        }
        
        return quests;
    }
    
    private static QuestTemplate PickQuestTemplate(
        Faction faction,
        RegionState region,
        PlayerState player)
    {
        // Weight templates by player's playstyle
        var templates = new List<QuestTemplate>()
        {
            new ConquestQuestTemplate(),
            new GatheringQuestTemplate(),
            new DeliveryQuestTemplate(),
            new BountyQuestTemplate(),
            new RitualQuestTemplate()
        };
        
        // Filter by tier constraints
        templates = templates
            .Where(t => region.CurrentTier >= t.MinimumTier && region.CurrentTier <= t.MaximumTier)
            .ToList();
        
        // Pick randomly with fallback
        return templates.Count > 0 
            ? templates[DeterministicRandom.Range(0, templates.Count)]
            : new ConquestQuestTemplate();
    }
}
```

### Quest Chains

Quests can link into chains (complete one → unlock next):

```csharp
public class QuestChain
{
    public string ChainId { get; set; }
    public string Title { get; set; }
    public List<Quest> QuestsInOrder { get; set; }
    
    public Quest GetNextQuestForPlayer(PlayerState player)
    {
        // Find first incomplete quest in chain
        return QuestsInOrder.FirstOrDefault(q => !player.CompletedQuests.Contains(q.QuestId));
    }
}

// Example: "Rise of the Raider Clans" chain
var raidersChain = new QuestChain()
{
    ChainId = "raider_clans_rise",
    Title = "Rise of the Raider Clans",
    QuestsInOrder = new()
    {
        new ConquestQuest() { Title = "Recruit Outlaws", TargetCount = 20 },
        new GatheringQuest() { Title = "Gather Raider Supplies", TargetMaterial = "raider_iron", TargetCount = 15 },
        new DeliveryQuest() { Title = "Deliver Weapons to Raider Camp", Item = "weapons" },
        new BountyQuest() { Title = "Eliminate Enemy Warlord", TargetBoss = "warlord_torgun" },
        new RitualQuest() { Title = "Empower the Clans", Choices = new[] { empower, corrupt } }
    }
};
```

---

## 7. Reputation & Faction Alignment

### Reputation System

Player reputation with each faction ranges from **-100 to +100**:

```csharp
public class FactionReputation
{
    public string FactionId { get; set; }
    public int Reputation { get; set; }  // -100 to +100
    
    public FactionStanding GetStanding()
    {
        return Reputation switch
        {
            < -50 => FactionStanding.Hostile,      // Faction attacks on sight
            < -10 => FactionStanding.Unfriendly,   // No quests available
            < 10 => FactionStanding.Neutral,       // Standard quests
            < 50 => FactionStanding.Friendly,      // Better rewards, discounts
            _ => FactionStanding.Honored           // Exclusive quests, perks
        };
    }
    
    public void AddReputation(int amount)
    {
        Reputation = Math.Clamp(Reputation + amount, -100, 100);
    }
}

public enum FactionStanding
{
    Hostile,      // Faction hunts player
    Unfriendly,   // No quests or trade
    Neutral,      // Normal interaction
    Friendly,     // 10% quest reward boost
    Honored       // 20% quest reward boost + exclusive quests
}
```

### Multi-Faction Conflicts

If player has high rep with two rival factions, conflicts arise:

```
If Raider Clans (rep +80) AND Ice Druids (rep +70):
  → Both factions are in Northern Realm
  → They compete for territory control
  → Player must choose side or negotiate peace
  → Choice affects regional evolution path
  
Option 1: Support Raiders
  → Raiders gain +30 strength
  → Ice Druids lose +20 strength
  → Northern Realm trends toward Age of Raiders path
  → Ice Druids reputation with player: -30
  
Option 2: Support Ice Druids
  → Ice Druids gain +30 strength
  → Raiders lose +20 strength
  → Northern Realm trends toward Eternal Winter path
  → Raiders reputation with player: -30
  
Option 3: Negotiate Peace (expensive)
  → Cost 5000 gold + rare materials
  → Both factions stay (rare state)
  → Regional evolution becomes unpredictable
  → Unique quests unlock
```

---

## 8. Data Schemas (C# Pointers)

### Quest Base Class

```csharp
namespace PG.Gameplay.Quests
{
    public abstract class Quest
    {
        public string QuestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public QuestType Type { get; set; }
        
        // NPC/Faction
        public Faction SourceFaction { get; set; }
        public string SourceNPC { get; set; }
        
        // Requirements
        public int MinimumTier { get; set; }
        public int MaximumTier { get; set; }
        
        // Objectives (can have multiple)
        public List<QuestObjective> Objectives { get; set; }
        
        // Rewards
        public int RewardXP { get; set; }
        public int RewardGold { get; set; }
        public int RewardReputation { get; set; }
        public List<(string ItemId, int Quantity)> RewardItems { get; set; }
        public bool RewardLegendaryGuaranteed { get; set; }
        
        // State
        public QuestState CurrentState { get; set; }  // Active, Completed, Abandoned, Failed
        public DateTime? CompletedTime { get; set; }
        
        // Effects on world
        public Action<WorldState> OnCompletion { get; set; }
        public Action<WorldState> OnAbandon { get; set; }
        
        public abstract bool IsCompleted();
        public abstract void UpdateProgress(PlayerAction action);
    }
    
    public enum QuestState
    {
        Available,
        Active,
        Completed,
        Abandoned,
        Failed
    }
    
    public abstract class QuestObjective
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TargetCount { get; set; }
        public int CurrentCount { get; set; }
        public bool IsComplete => CurrentCount >= TargetCount;
    }
    
    // Specific quest types
    public class ConquestQuest : Quest
    {
        public string TargetEnemyArchetype { get; set; }
        public override bool IsCompleted() => Objectives[0].CurrentCount >= TargetCount;
    }
    
    public class GatheringQuest : Quest
    {
        public string TargetMaterial { get; set; }
        public override bool IsCompleted() => Objectives[0].CurrentCount >= TargetCount;
    }
    
    public class DeliveryQuest : Quest
    {
        public string FromNPC { get; set; }
        public string ToRegion { get; set; }
        public string Item { get; set; }
        public override bool IsCompleted() => Objectives[0].IsComplete && Objectives[1].IsComplete;
    }
    
    public class RitualQuest : Quest
    {
        public RitualChoice[] Choices { get; set; }
        public RitualChoice SelectedChoice { get; set; }
        public override bool IsCompleted() => SelectedChoice != null;
    }
    
    public class RitualChoice
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Func<string> Consequence { get; set; }  // Returns flavor text
    }
    
    public class InterventionQuest : Quest
    {
        public int DaysRemaining { get; set; }
        public override bool IsCompleted() => Objectives.All(o => o.IsComplete);
    }
}
```

### NPC & Faction Database

```csharp
public class NPCCharacter
{
    public string NpcId { get; set; }
    public string Name { get; set; }
    public string Faction { get; set; }
    public string Region { get; set; }
    
    // Status
    public int CurrentTier { get; set; }
    public bool IsActive { get; set; }  // False if migrated/dead
    public bool IsFavorite { get; set; }  // Player marked as favorite
    
    // Personality & dialogue
    public string Archetype { get; set; }  // "Mercenary", "Wizard", "Cleric", etc.
    public List<string> DialogueLines { get; set; }
    
    // Quest generation
    public List<Quest> GeneratedQuests { get; set; }
}

public class FactionDatabase
{
    private static Dictionary<string, Faction> _factions;
    
    public static Faction GetFaction(string factionId)
    {
        return _factions.TryGetValue(factionId, out var faction) ? faction : null;
    }
    
    public static List<Faction> GetFactionsInRegion(string regionId)
    {
        return _factions.Values
            .Where(f => f.RegionId == regionId)
            .ToList();
    }
    
    public static void UpdateMigration(WorldState world)
    {
        foreach (var faction in _factions.Values)
        {
            if (faction.TurnsUntilMigration > 0)
            {
                faction.TurnsUntilMigration--;
                if (faction.TurnsUntilMigration == 0)
                {
                    MigrateFaction(faction, world);
                }
            }
        }
    }
    
    private static void MigrateFaction(Faction faction, WorldState world)
    {
        var targetRegion = world.GetAdjacentRegion(faction.RegionId);
        // Move faction, update NPCs, lock old region
    }
}
```

---

## 9. Quest Impact on World Evolution

### How Quests Affect Tier Spread

Completing certain quests **accelerates or delays** the evolution path:

```csharp
public class QuestImpactOnEvolution
{
    public static void ApplyQuestReward(Quest quest, WorldState world, RegionState completionRegion)
    {
        // Quest type affects region evolution
        switch (quest.Type)
        {
            case QuestType.Conquest:
                // Eliminating enemies allows different path to spread
                completionRegion.ModifyPathInfluence(quest.SourceFaction.EvolutionPath, +10);
                break;
                
            case QuestType.Ritual:
                // Ritual quests directly reshape evolution
                var ritualQuest = quest as RitualQuest;
                var consequence = ritualQuest.SelectedChoice.Consequence();
                // Consequence modifies path influence significantly
                break;
                
            case QuestType.Gathering:
                // Supporting faction economy lets them spread
                quest.SourceFaction.Strength += 10;
                if (quest.SourceFaction.Strength > 70)
                    completionRegion.SpreadPathToAdjacentRegion(quest.SourceFaction.EvolutionPath);
                break;
                
            case QuestType.Intervention:
                // Saving a faction locks in their path (temporarily)
                completionRegion.LockPathToCurrentEvolution(turns: 10);
                break;
        }
    }
}
```

---

## 10. Implementation Roadmap

### Phase 1: Quest Foundation (Week 1)
- [ ] Create Quest base class and quest types
- [ ] Implement QuestTemplate system
- [ ] Build basic quest generation (Conquest, Gathering)
- [ ] Add quest acceptance/abandonment logic

### Phase 2: Factions & NPCs (Week 2)
- [ ] Implement Faction system with attributes
- [ ] Create NPC characters with dialogue
- [ ] Add faction reputation tracking
- [ ] Link NPCs to quest generation

### Phase 3: Migration & Intervention (Week 3)
- [ ] Implement NPC migration countdown system
- [ ] Add Intervention Quest template
- [ ] Create migration triggers (low faction strength)
- [ ] Test faction movement between regions

### Phase 4: Advanced & Polish (Week 4)
- [ ] Implement Ritual quests with player choice
- [ ] Add quest chains
- [ ] Create multi-faction conflict system
- [ ] Implement quest impact on world evolution
- [ ] Add quest UI (log, markers, objectives)

---

## 11. Testing Checklist

Before declaring quests complete:

- [ ] Quests generate daily with appropriate variety
- [ ] Quest rewards scale by tier (T1 quest ≠ T5 quest reward)
- [ ] Faction reputation changes affect standing (neutral → friendly at +10)
- [ ] NPC migration countdown triggers when faction strength < 30
- [ ] Intervention quests appear only when faction is dying
- [ ] Ritual quests offer meaningful choices that affect world state
- [ ] Quest chains unlock properly (first quest → second quest available)
- [ ] Faction conflicts occur when player is friendly with rivals
- [ ] Quests impact evolution tier spread (conquest quests spread path)
- [ ] Abandoned quests reset properly
- [ ] No quest generation errors (all templates produce valid quests)
- [ ] Legendary item rewards appear at correct rates (5-10% of quests)
- [ ] Player can maintain multiple faction relationships simultaneously
- [ ] Faction NPCs use consistent dialogue (no contradictions)

---

## 12. Example Quest Progression (4-Day Play Session)

### Day 1: First Encounter
```
Player enters Northern Realm (Age of Raiders path, T1)
→ Raider Clans faction faction strength: 70 (healthy)
→ Available quests:
  - Conquest: "Eliminate Bandits" (10 kill target)
  - Gathering: "Gather Raider Iron" (5 material target)
  - Bounty: "Kill Thug Leader Gerax"

Player accepts Conquest quest → +250 XP, +100 faction rep (now 10 rep, neutral)
```

### Day 2: Growing Involvement
```
Raider Clans strength improves (now 75)
→ New quest available:
  - Delivery: "Deliver weapons to southern camp" (cross-region travel)
  - Ritual: "Bless the Raider Stronghold" (choose empower or corrupt)

Player accepts Delivery → must travel to adjacent region
  → Meets Ice Druids (neutral toward them)
  → Returns weapons → +400 XP, +200 gold, +20 rep with both factions
```

### Day 3: Conflict
```
Player has high rep with both Raider Clans (rep 40) and Ice Druids (rep 35)
→ Factions begin competing for territory
→ NEW QUEST: "Broker Peace" (negotiation choice)
  - Support Raiders: Raiders gain power, Ice Druids hostile (-30 rep)
  - Support Druids: Druids gain power, Raiders hostile
  - Negotiate: Both stay, 5000 gold cost, unique joint quest unlocks

Player chooses Negotiate → factions share Northern Realm peacefully
→ Unique "Treaty of Ice and Raid" quest chain becomes available
```

### Day 4: Crisis
```
New NPC appears: Plague Keepers (faction strength 15, dying)
→ Intervention Quest: "Save the Plague Keepers" (5-day countdown)
  - Complete 3 objectives or they migrate to Infernal Lands
  - Rewards: +100 rep, 1000 gold, guaranteed legendary

Player can:
  A) Complete intervention (save Plague Keepers, lock Dominion of Decay path)
  B) Ignore them (they migrate, Eternal Winter path spreads instead)
  C) Attack them (reputation -50, they become hostile, forced migration)
```

---

## 13. Configuration (Example)

```csharp
public class QuestConfig
{
    // Quest generation
    public int DailyQuestsPerFaction = 3;
    public int MaxActiveQuestsPerPlayer = 10;
    public int QuestAbandonPenalty = -5;  // Reputation loss
    
    // NPC migration
    public float FactionsStrengthThreshold = 0.30f;  // 30% = dying
    public int MigrationCountdownDays = 5;
    public int InterventionQuestReward = 100;  // Rep bonus
    
    // Reputation
    public int ConquestQuestRepReward = 10;
    public int GatheringQuestRepReward = 15;
    public int DeliveryQuestRepReward = 20;
    public int RitualQuestRepReward = 30;
    public int InterventionQuestRepReward = 100;
    
    // Multi-faction balance
    public int FactionConflictThreshold = 50;  // Both > 50 = conflict
    public int NegotiationCost = 5000;
    
    // Quest impact on evolution
    public int ConquestPathInfluenceBoost = 10;
    public int RitualPathInfluenceBoost = 30;  // Ritual is most impactful
    public int FactionsStrengthPerGatheringQuest = 10;
}
```

---

## 14. Integration with Other Systems

### Crafting ↔ Quests
- Gathering quests can request specific materials
- Delivery quests transport crafted items
- Faction economy affects material scarcity (high strength = lower prices)

### Multiplayer ↔ Quests
- Conquest quests scale enemy count by party size (1-5 players)
- Delivery quests can be shared (both players get reward)
- Ritual quests require all party members to vote on choice
- Intervention quests must be completed by party together (or lost)

### World Evolution ↔ Quests
- Quests available change based on current tier & path
- Ritual quests directly modify path influence
- Intervention quests can lock or unlock evolution paths
- NPC migration shifts regional power dynamics

---

**Next Phase:** Procedural quests & NPC migration foundation created. Ready for minimal playable prototype (integrate all systems into Godot) or prototype implementation planning?
