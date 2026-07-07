# Content Creation Templates & Guide

**How to add new quests, EventBars, settlements, and encounters to your living world**

---

## Table of Contents
1. [Quest Template](#quest-template)
2. [QuestLine Template](#questline-template)
3. [EventBar Template](#eventbar-template)
4. [Settlement Template](#settlement-template)
5. [Encounter Template](#encounter-template)
6. [Quick Reference](#quick-reference)

---

## Quest Template

### Where to Add
File: `Simulation/WorldEvolution.cs`
Section: `QuestRegistry` (around line 3293)

### Basic Template

```csharp
new QuestDefinition
{
    // UNIQUE IDENTIFIER (no spaces, use camelCase)
    Id = "YourQuestId",
    
    // DISPLAY NAME (what player sees)
    Title = "Quest Title Goes Here",
    
    // DETAILED OBJECTIVE
    Description = "Detailed description of what the player needs to do...",
    
    // WHICH REGION? 
    Region = RegionId.NorthernRealm,
    
    // WHO GIVES THE QUEST?
    Giver = "NpcName",
    
    // PREREQUISITES (which quests must complete first?)
    Prerequisites = new() { /* "PreviousQuestId" */ },
    
    // PLAYER REWARDS
    Rewards = new() { ("Gold", 50), ("Item", 1) },
    
    // REPUTATION CHANGES (faction, amount)
    ReputationRewards = new() { (FactionType.Merchants, 5) },
    
    // WORLD BAR PROGRESSION (which EventBars advance?)
    EventBarEffects = new() { ("MerchantCaravan", 8) },
    
    // ZONE EVOLUTION INFLUENCE
    ZoneEvolutionInfluences = new() { ("DivineOrder", 3) },
    
    // SETTLEMENT MODIFICATIONS (which towns change?)
    SettlementModifications = new() { ("StoneHaven", "Wealth", 5) }
}
```

### Detailed Field Explanation

| Field | Example | Notes |
|-------|---------|-------|
| `Id` | `"RepairCart"` | Must be unique, no spaces |
| `Title` | `"Repair the Merchant Cart"` | Player-facing name |
| `Description` | `"Help a merchant fix..."` | Quest objective |
| `Region` | `RegionId.NorthernRealm` | Starting region |
| `Giver` | `"MerchantGuild"` | Who offers the quest |
| `Prerequisites` | `new() { "PreviousId" }` | Gate progression |
| `Rewards` | `new() { ("Gold", 50) }` | Gold amount |
| `ReputationRewards` | `(FactionType.Merchants, 5)` | Faction standing change |
| `EventBarEffects` | `("MerchantCaravan", 8)` | World bar progression |
| `ZoneEvolutionInfluences` | `("DivineOrder", 3)` | Zone bar influence |
| `SettlementModifications` | `("StoneHaven", "Wealth", 5)` | Town stat change |

### Realistic Reward Values

```
Gold Rewards:      25-120 based on difficulty
Item Rewards:      1-5 items
Reputation:        ±5 to ±20 per faction
EventBar Effects:  ±5 to ±20 progress
Zone Evolution:    ±2 to ±9 influence
Settlement Mods:   ±3 to ±10 per stat
```

### Example: Complete Quest

```csharp
new QuestDefinition
{
    Id = "DeliverPackage",
    Title = "Deliver the Package",
    Description = "A merchant's package must be delivered to the next town safely. Guard it from bandits.",
    Region = RegionId.NorthernRealm,
    Giver = "MerchantGuild",
    Prerequisites = new() { "RepairMerchantCart" },  // Must repair first
    Rewards = new() { ("Gold", 45), ("Item", 2) },
    ReputationRewards = new() { (FactionType.Merchants, 5) },
    EventBarEffects = new() { ("MerchantCaravan", 6), ("RangerActivity", 2) },
    ZoneEvolutionInfluences = new() { ("DivineOrder", 2) },
    SettlementModifications = new() { ("StoneHaven", "Wealth", 3) }
}
```

### Build & Test
```bash
dotnet build
dotnet run --no-build
# In game: Visit town → Accept "Deliver the Package" → See rewards
```

---

## QuestLine Template

### Where to Add
File: `Simulation/WorldEvolution.cs`
Section: `QuestLineRegistry` (around line 3030)

### Purpose
QuestLines chain multiple quests together in sequence, like World of Warcraft quest chains.

### Basic Template

```csharp
new QuestLineDefinition
{
    // UNIQUE IDENTIFIER
    Id = "MyQuestChain",
    
    // DISPLAY NAME
    Name = "Epic Quest Chain",
    
    // DESCRIPTION
    Description = "A series of connected quests telling a story...",
    
    // FIRST QUEST (player accepts this quest to start the chain)
    InitialQuestId = "FirstQuestId",
    
    // ALL QUESTS IN ORDER (must match InitialQuestId as first)
    Questlines = new()
    {
        "FirstQuestId",     // [1/4]
        "SecondQuestId",    // [2/4]
        "ThirdQuestId",     // [3/4]
        "FinalBossQuestId"  // [4/4]
    },
    
    // FINAL REWARDS (when all quests complete)
    FinalRewards = new() { ("Gold", 200), ("Item", 5) },
    
    // FINAL REPUTATION (when questline completes)
    FinalReputation = new() { (FactionType.Merchants, 20) }
}
```

### How QuestLines Work

```
Player accepts "FirstQuestId"
  ↓
Completes quest 1/4
  ↓
Quest 2/4 becomes available
  ↓
Completes quest 2/4
  ↓
Quest 3/4 becomes available
  ↓
Completes quests 3/4 and 4/4
  ↓
QuestLine completes!
  ├─ Grants final gold/items
  ├─ Grants final reputation
  ├─ Unlocks new questlines (if any)
  └─ Applies zone evolution influence
```

### Example: Complete QuestLine

```csharp
new QuestLineDefinition
{
    Id = "MerchantAdvancement",
    Name = "Rise Through Merchant Guild",
    Description = "Prove yourself to the merchant guild through increasingly important tasks.",
    InitialQuestId = "DeliverSmallPackage",
    Questlines = new()
    {
        "DeliverSmallPackage",
        "EscortMerchant",
        "InvestigateAttacks",
        "DestroyBanditCamp",
        "DefendMerchantCongress"
    },
    FinalRewards = new() { ("Gold", 250), ("Item", 10) },
    FinalReputation = new() { (FactionType.Merchants, 30), (FactionType.Rangers, 10) }
}
```

### Important Rules
1. **InitialQuestId must be in Questlines list** (as first element)
2. **Order matters** - Quests unlock in list order
3. **All quest IDs must exist** in QuestRegistry
4. **Final quest completion** triggers all QuestLine effects

---

## EventBar Template

### Where to Add
File: `Simulation/WorldEvolution.cs`
Section: `EventBarRegistry` (around line 3167)

### Purpose
EventBars represent world conditions that:
- Progress from player actions
- Decay over time
- Trigger cascading effects when completing
- Can repeat to cycle through world conditions

### Basic Template

```csharp
new EventBarDefinition
{
    // UNIQUE IDENTIFIER
    Id = "MyCustomBar",
    
    // DISPLAY NAME
    Name = "Custom World Condition",
    
    // DESCRIPTION
    Description = "What does this EventBar represent?",
    
    // CATEGORY (for grouping)
    Category = "General",
    
    // STARTING VALUE (0-100 range)
    StartingValue = 0,
    
    // DECAY PER TURN (how fast it decreases)
    DecayAmount = 0.10,
    
    // COMPLETION THRESHOLD (value needed to trigger)
    Threshold = 50,
    
    // LIMITS
    MinValue = 0,
    MaxValue = 100,
    
    // HOW OFTEN TO CHECK COMPLETION (in turns)
    CheckFrequency = 3,
    
    // COOLDOWN FOR REPEATABLE BARS (turns before next completion)
    CooldownTurns = 20,
    
    // CAN THIS BAR COMPLETE MULTIPLE TIMES?
    Repeatable = true,
    
    // PREREQUISITES (other bars that must exist)
    PrerequisiteEventBarIds = new(),
    
    // CONDITION TO ALLOW PROGRESSION
    CanProgress = world => true,
    
    // COMPLETION EFFECTS
    OnCompletion = new EventBarCompletion
    {
        // QUESTS THAT BECOME AVAILABLE
        UnlockedQuestLineIds = new() { "MyQuestLine" },
        
        // ENCOUNTERS THAT APPEAR
        UnlockedEncounterIds = new() { "EncounterId" },
        
        // ZONE EVOLUTION INFLUENCES
        ZoneEvolutionInfluences = new() { ("DivineOrder", 5) },
        
        // SETTLEMENT CHANGES
        SettlementModifications = new()
        {
            ("StoneHaven", "Wealth", 10),
            ("SilverStream", "Safety", -5)
        }
    }
}
```

### Decay Mechanics Explained

```
With StartingValue=0, DecayAmount=0.10, Threshold=50:

Turn 1: Player explores (+1) → Bar = 1/50
Turn 2: Player explores (+1), decay (-0.10) → Bar = 1.9/50
Turn 3: Player helps (+3), decay (-0.10) → Bar = 4.8/50
...
Turn 15: Bar reaches 50! → COMPLETION TRIGGERED
```

### Realistic Decay Values

| Decay | Description | Example |
|-------|-------------|---------|
| 0.08 | Slow decay | Stable conditions (trade, farming) |
| 0.10-0.12 | Medium decay | Balanced (typical) |
| 0.15-0.18 | Fast decay | Transient (bandits, wildlife) |

### Completion Threshold Ranges

```
If player actions add ±3-8 per action:
  Threshold 40  → ~8-10 actions needed
  Threshold 50  → ~10-15 actions needed
  Threshold 60  → ~15-20 actions needed
  Threshold 70  → ~20-25 actions needed
```

### Example: Complete EventBar

```csharp
new EventBarDefinition
{
    Id = "DragonAwakening",
    Name = "Dragon Nesting",
    Description = "An ancient dragon awakens in the high mountains.",
    Category = "Danger",
    StartingValue = 2,
    DecayAmount = 0.08,  // Slow decay (dragons stay a while)
    Threshold = 45,
    MinValue = 0,
    MaxValue = 100,
    CheckFrequency = 2,
    CooldownTurns = 30,  // Long cooldown (dragons take time to leave)
    Repeatable = true,
    PrerequisiteEventBarIds = new(),
    CanProgress = world => world.Regions[RegionId.Highlands].CurrentTier >= WorldTier.Tier2,
    OnCompletion = new EventBarCompletion
    {
        UnlockedQuestLineIds = new() { "DragonSlayers" },
        UnlockedEncounterIds = new() { "DragonEncounter", "DraconicArtifacts" },
        ZoneEvolutionInfluences = new() { ("AgeOfBandits", 3), ("DivineOrder", -2) },
        SettlementModifications = new()
        {
            ("Highlands", "Safety", -15),
            ("Highlands", "Wealth", -10)
        }
    }
}
```

### Hook to Player Actions

After adding the EventBar, add it to `ApplyActionProgression()` method:

```csharp
// Around line 2048 in WorldEvolution.cs
case PlayerAction.Investigate:
    AddEventBarProgress(affected, "DragonAwakening", 5);  // Investigating triggers dragon
    break;
```

---

## Settlement Template

### Where to Add
File: `Simulation/WorldEvolution.cs`
Section: `SettlementRegistry` (around line 3092)

### Purpose
Settlements are NPCs/towns that respond to EventBar changes.

### Basic Template

```csharp
new SettlementDefinition
{
    // UNIQUE IDENTIFIER
    Id = "MyTown",
    
    // DISPLAY NAME
    Name = "Prosperous Riverside",
    
    // TYPE (Town, Village, Monastery, etc.)
    Type = "Town",
    
    // WHICH REGION?
    Region = RegionId.NorthernRealm,
    
    // INITIAL STATS (0-1000 for population, 0-100 for wealth/safety)
    PopulationStart = 50,
    WealthStart = 60,
    SafetyStart = 70,
    
    // BUILDINGS AVAILABLE
    InitialBuildings = new()
    {
        "Tavern",
        "Market",
        "Guard Post"
    },
    
    // STARTING QUESTLINES
    InitialQuestLineIds = new()
    {
        "MerchantGuild",
        "LocalFarmers"
    },
    
    // SETTLEMENT PROPERTIES (for future features)
    Properties = new()
    {
        { "HasMarket", true },
        { "HasTemple", false },
        { "PopulationLimit", 200 }
    }
}
```

### Settlement Stats

| Stat | Range | Meaning |
|------|-------|---------|
| Population | 0-1000 | Number of inhabitants |
| Wealth | 0-100 | Economic prosperity |
| Safety | 0-100 | Protection & security |

### Stat Effects

```
High Population (80+)
  ├─ More quests available
  ├─ Better prices
  └─ More NPC interactions

High Wealth (70+)
  ├─ Quest rewards increase
  ├─ Item prices higher
  └─ Better accommodations

High Safety (70+)
  ├─ Easier encounters
  ├─ Less bandit activity
  └─ More commerce
```

### Example: Complete Settlement

```csharp
new SettlementDefinition
{
    Id = "TradingPost",
    Name = "Crossroads Trading Post",
    Type = "Town",
    Region = RegionId.NorthernRealm,
    PopulationStart = 75,
    WealthStart = 80,
    SafetyStart = 65,
    InitialBuildings = new()
    {
        "Market",
        "Inn",
        "Stables",
        "Warehouse",
        "Guard Tower"
    },
    InitialQuestLineIds = new()
    {
        "MerchantGuild",
        "TradeExpansion",
        "RangerRecruit"
    },
    Properties = new()
    {
        { "HasMarket", true },
        { "HasInn", true },
        { "HasStables", true },
        { "HasWarehouse", true },
        { "PopulationLimit", 500 },
        { "TradeHub", true }
    }
}
```

### How Settlements Change

When EventBars complete, settlements are modified:

```csharp
// In EventBarCompletion.OnCompletion:
SettlementModifications = new()
{
    ("TradingPost", "Wealth", 20),        // +20 wealth
    ("TradingPost", "Population", 15),    // +15 population
    ("RuralVillage", "Safety", -5)        // -5 safety (new threats)
}
```

---

## Encounter Template

### Where to Add
File: `Simulation/WorldEvolution.cs`
Section: `EncounterRegistry` (around line 2500)

### Purpose
Encounters are events that trigger when exploring. They offer choices that affect EventBars and quests.

### Basic Template

```csharp
new EncounterDefinition
{
    // UNIQUE IDENTIFIER
    Id = "MyEncounter",
    
    // DISPLAY NAME
    Name = "Mysterious Encounter",
    
    // DESCRIPTION (shown to player)
    Description = "A detailed description of what the player encounters...",
    
    // UNLOCKING CONDITIONS (when does this encounter appear?)
    CanTrigger = (world, region) => true,  // Condition to show
    PrerequisiteEventBarIds = new(),       // Required EventBars
    
    // ENCOUNTER OPTIONS (choices player can make)
    Options = new()
    {
        new EncounterOption
        {
            Title = "Option 1: Be Helpful",
            Description = "Help the person/creature",
            Choice = 1,
            Outcome = (world, region) =>
            {
                // What happens when player chooses this?
                world.Log(LogCategory.Player, "You helped! They reward you.");
                // Modify EventBars, reputation, etc.
            }
        },
        new EncounterOption
        {
            Title = "Option 2: Ignore Them",
            Description = "Walk away without helping",
            Choice = 2,
            Outcome = (world, region) => { /* ... */ }
        }
    }
}
```

### Example: Complete Encounter

```csharp
new EncounterDefinition
{
    Id = "LostChild",
    Name = "Lost Child in Forest",
    Description = "A young child is lost in the woods, crying for help. What do you do?",
    CanTrigger = (world, region) => true,
    PrerequisiteEventBarIds = new(),
    Options = new()
    {
        new EncounterOption
        {
            Title = "Help the Child",
            Description = "Spend time returning the child to town",
            Choice = 1,
            Outcome = (world, region) =>
            {
                world.Log(LogCategory.Player, "You return the child safely. Their family rewards you.");
                world.Player.ChangeReputation(FactionType.Merchants, 1);
                region.EventBars["TempleActivity"].AddProgress(2);
                region.EventBars["FarmersNeed"].AddProgress(1);
            }
        },
        new EncounterOption
        {
            Title = "Give Directions",
            Description = "Point them toward town and continue",
            Choice = 2,
            Outcome = (world, region) =>
            {
                world.Log(LogCategory.Player, "You point them toward town.");
                // Minimal reward for minimal effort
            }
        },
        new EncounterOption
        {
            Title = "Ignore Them",
            Description = "Continue on your way",
            Choice = 3,
            Outcome = (world, region) =>
            {
                world.Log(LogCategory.Player, "You leave the child behind...");
                world.Player.ChangeReputation(FactionType.Clergy, -1);
            }
        }
    }
}
```

---

## Quick Reference

### File Locations
```
Simulation/WorldEvolution.cs
├─ QuestRegistry (line 3293)
├─ QuestLineRegistry (line 3030)
├─ EventBarRegistry (line 3167)
├─ SettlementRegistry (line 3092)
├─ EncounterRegistry (line 2500)
└─ ApplyActionProgression (line 2048) ← Wire actions here
```

### Factions Available
```csharp
FactionType.Merchants    // Traders
FactionType.Rangers      // Law enforcement
FactionType.Clergy       // Religious
FactionType.Bandits      // Criminals
FactionType.Miners       // Miners
FactionType.Nobles       // Aristocracy
FactionType.Mages        // Arcane users
```

### Reward Types
```csharp
Rewards = new() { 
    ("Gold", 50),        // Currency
    ("Item", 3),         // Inventory items
    ("Experience", 100)  // Custom types
}
```

### Common EventBars
```
MerchantCaravan          // Trade activity
BanditPatrol             // Road threats
TempleActivity           // Religious influence
RangerActivity           // Law enforcement
FarmersNeed              // Agriculture
WildlifeEncroach         // Animal threats
```

### Zone Evolution Bars
```
DivineOrder              // Civilization/order
AgeOfBandits             // Chaos/crime
```

---

## Building & Testing

### After Adding Content

```bash
# 1. Save your changes
# 2. Build the project
dotnet build

# 3. If build succeeds, run the game
dotnet run --no-build

# 4. Test your content in-game
> explore
> character
> quests
> history
```

### Common Issues

**Quest doesn't appear**
- Check Prerequisites are empty or completed
- Check EventBar completion unlocks the questline

**EventBar not progressing**
- Check ApplyActionProgression() includes your bar
- Check action type actually progresses it

**Build fails**
- Ensure all IDs referenced actually exist
- Check for typos in settlement/questline IDs
- Run `dotnet clean && dotnet restore && dotnet build`

---

## Summary

| Type | Time | Location | Complexity |
|------|------|----------|------------|
| Quest | 2 min | QuestRegistry | Simple |
| QuestLine | 3 min | QuestLineRegistry | Simple |
| EventBar | 5 min | EventBarRegistry | Medium |
| Settlement | 3 min | SettlementRegistry | Simple |
| Encounter | 5-10 min | EncounterRegistry | Medium-Hard |

**Total time to add quest + wire it: ~5 minutes**

All templates provide working examples. Copy, modify, and build!

---

**Template Version:** 1.0
**Date:** 2026-07-05
**Status:** Ready for content creation ✅
