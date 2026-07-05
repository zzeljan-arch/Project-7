# Procedural Game Simulation - Production Guide

**A data-driven, emergent living world simulation engine with MMO-style quest systems and dynamic EventBars.**

---

## 📋 Table of Contents

1. [Quick Start](#quick-start)
2. [Architecture Overview](#architecture-overview)
3. [Running the Simulation](#running-the-simulation)
4. [Adding New Content](#adding-new-content)
   - [Adding Quests](#adding-quests)
   - [Adding QuestLines](#adding-questlines)
   - [Adding EventBars](#adding-eventbars)
   - [Adding Settlements](#adding-settlements)
5. [Quest System Design (MMO Confirmation)](#quest-system-design-mmo-confirmation)
6. [Data-Driven Architecture](#data-driven-architecture)

---

## Quick Start

### System Requirements
- .NET 6.0 SDK or later
- Windows/Linux/macOS with PowerShell or terminal

### Build the Project

```bash
cd c:\Users\ljova\Desktop\Project7
dotnet build ProceduralGame.sln
```

### Run the Interactive Simulation

```bash
cd Simulation
dotnet run --no-build
```

The simulation will launch an interactive campaign where you can:
- Explore the world
- Complete quests and encounters
- Watch EventBars progress and affect settlements
- Observe emergent world evolution

### Example Play Session

```
> explore
[EVENT] You venture forth into the surrounding land.
[SIM] World advanced by 120 minutes. Time is now Day 1, 18:00.
[EVENT] Encounter: Stranded Merchant...

> read history
-- World Event History --
Campaign seeded with 42 starting at NorthernRealm
World advanced by 120 minutes. Time is now Day 1, 10:00.
[BAR] Bandit Clearance +35.0: 0.0  35.0/75
Reputation with Rangers changed by 2.5.
```

---

## Architecture Overview

### Folder Structure (Production-Ready)

```
Simulation/
├── Program.cs                           # Entry point & interactive loop
├── ProceduralGameSimulation.csproj      # Project file
│
├── Core/                                # Core engine classes
│   ├── Definitions/                     # Data structure definitions
│   │   ├── QuestDefinition.cs          # Quest metadata
│   │   ├── EventBarDefinition.cs       # EventBar configuration
│   │   ├── SettlementDefinition.cs     # Town/city metadata
│   │   ├── EncounterDefinition.cs      # Encounter templates
│   │   └── ZoneEvolutionBarDefinition.cs
│   │
│   ├── State/                           # Runtime state objects
│   │   ├── WorldState.cs               # Global game state
│   │   ├── RegionState.cs              # Per-region state
│   │   ├── EventBarState.cs            # EventBar runtime values
│   │   ├── SettlementState.cs          # Settlement runtime values
│   │   ├── QuestLineState.cs           # QuestLine progression
│   │   └── PlayerState.cs              # Player stats/inventory
│   │
│   └── Registries/                      # Master data registries
│       ├── QuestRegistry.cs            # All quests (data-driven)
│       ├── QuestLineRegistry.cs        # All questlines
│       ├── EventBarRegistry.cs         # All EventBars
│       ├── SettlementRegistry.cs       # All settlements
│       ├── EncounterRegistry.cs        # All encounters
│       └── ZoneEvolutionBarRegistry.cs
│
├── Systems/                             # Game systems
│   ├── ProgressionSystem.cs            # EventBar progression & decay
│   ├── EncounterSystem.cs              # Encounter triggers & results
│   ├── QuestSystem.cs                  # Quest progression & rewards
│   ├── ZoneEvolutionSystem.cs          # Zone tier & path changes
│   └── SettlementSystem.cs             # Settlement growth/decline
│
├── Content/                             # Easy-add content templates
│   ├── QUEST_TEMPLATE.cs               # Template for new quests
│   ├── QUESTLINE_TEMPLATE.cs           # Template for new questlines
│   ├── EVENTBAR_TEMPLATE.cs            # Template for new EventBars
│   └── ENCOUNTER_TEMPLATE.cs           # Template for new encounters
│
└── README_PRODUCTION.md                # This file

WorldEvolution.cs                        # Monolithic file (being refactored)
```

### Key Concepts

#### **EventBars** - The World's Pulse
EventBars are numeric values (0-100) that track regional conditions:
- **MerchantCaravan** (threshold 60) - Trade routes active
- **BanditPatrol** (threshold 50) - Bandit activity level
- **TempleActivity** (threshold 70) - Religious influence
- **RangerActivity** (threshold 50) - Law enforcement
- **FarmersNeed** (threshold 55) - Agricultural demands
- **WildlifeEncroach** (threshold 40) - Animal threats

When an EventBar completes:
1. Encounters are unlocked
2. QuestLines become available
3. Settlements are modified (wealth, population, safety)
4. Zone evolution bars are influenced

#### **QuestLines** - MMO-Style Progression
Structured quest chains like World of Warcraft:
```
MerchantGuild (5 quests)
├─ RepairMerchantCart (quest 1/5)
├─ EscortMerchant (quest 2/5)
├─ InvestigateAttacks (quest 3/5)
├─ DestroyBanditCamp (quest 4/5)
└─ DefendCaravan (quest 5/5)
    └─ Unlocks: TradeExpansion (new questline)
```

Each quest:
- Has prerequisites (quests that must complete first)
- Grants gold rewards
- Increases faction reputation
- Progresses EventBars
- Chains to the next quest

#### **Settlements** - Dynamic NPCs
Towns that grow/shrink based on EventBars:
- **Population** - Number of inhabitants
- **Wealth** - Economic prosperity
- **Safety** - Protection level

When EventBars complete, settlements are modified:
```
MerchantCaravan completes → StoneHaven +8 wealth, +3 population
BanditPatrol completes → StoneHaven -5 safety, SilverStream -8 safety
TempleActivity completes → HolyTemple +5 wealth
```

#### **Zone Evolution** - Emergent World Changes
High-level regional evolution driven by EventBar completions:
- **DivineOrder** bar increases → Region becomes more civilized
- **AgeOfBandits** bar increases → Region becomes lawless
- When zone bars reach tier thresholds → Region tier/path changes
- Affects which encounters/questlines appear

---

## Running the Simulation

### Launch Options

#### Interactive Campaign (Default)
```bash
dotnet run --no-build
```
Walk through the world, make choices, watch EventBars progress.

#### Headless Simulation (WIP)
Runs without input, automatically taking actions and tracking world evolution.

#### Specific Seed
```bash
# Run with seed 12345 starting in NorthernRealm
dotnet run --no-build -- seed=12345 region=0
```

### Interactive Commands

```
explore              - Search the region for encounters
town                 - Visit a settlement
quests               - Show active/available quests
accept               - Accept an available quest
turnin               - Turn in a completed quest
character            - Show player stats and reputation
history              - Print world event log
rest                 - Sleep and restore health
help                 - Show all commands
quit                 - Exit the campaign
```

### Monitoring EventBars

EventBars progress automatically when you:
1. **Explore** - +1 MerchantCaravan, +1 RangerActivity (baseline)
2. **Help NPCs** - +3 MerchantCaravan, +2 FarmersNeed, +1 TempleActivity
3. **Defend** - +4 RangerActivity, -3 BanditPatrol
4. **Fight/Investigate** - +5 RangerActivity, -5 to -6 BanditPatrol

Watch the console output for:
```
[BAR] EventBarName decay/progress: 45.0  50.5/75
[EVENT] EventBarName has completed!
```

---

## Adding New Content

### Philosophy

**All content is data-driven. No C# coding needed to add quests, EventBars, or settlements.**

Simply add definitions to the registries and the engine handles the rest.

### Adding Quests

#### Step 1: Navigate to Registry

Open [Simulation/WorldEvolution.cs](WorldEvolution.cs) and find `QuestRegistry` (around line 3293).

#### Step 2: Define Your Quest

Use this template:

```csharp
new QuestDefinition
{
    Id = "MyNewQuest",
    Title = "A Compelling Quest",
    Description = "Detailed quest description here.",
    Region = RegionId.NorthernRealm,
    Giver = "NpcName",
    Giver = "quest-giver",
    Prerequisites = new() { "PreviousQuestId" },
    Rewards = new() { ("Gold", 50), ("Item", 1) },
    ReputationRewards = new() { (FactionType.Merchants, 5) },
    EventBarEffects = new() { ("MerchantCaravan", 8), ("RangerActivity", -3) },
    ZoneEvolutionInfluences = new() { ("DivineOrder", 3) },
    SettlementModifications = new() { ("StoneHaven", "Wealth", 5) }
}
```

#### Step 3: Complete QuestLine

Add to an existing questline's `QuestIds` or create a new questline:

```csharp
// In QuestLineRegistry (around line 3030)
new QuestLineDefinition
{
    Id = "MyQuestLine",
    Name = "My Custom Quest Line",
    Description = "An epic chain of quests",
    InitialQuestId = "FirstQuestId",
    Questlines = new()
    {
        "MyNewQuest",           // Your new quest
        "AnotherQuest",
        "FinalQuest"
    },
    FinalRewards = new() { ("Gold", 200), ("Item", 5) },
    FinalReputation = new() { (FactionType.Merchants, 20) }
}
```

#### Step 4: Build & Test

```bash
dotnet build
dotnet run --no-build
```

In-game: Accept quest → Complete → See rewards + EventBar progression.

### Adding QuestLines

QuestLines are chains of quests that unlock sequentially.

#### Template

```csharp
new QuestLineDefinition
{
    Id = "MyCustomQuestLine",
    Name = "Faction Quest Chain",
    Description = "A multi-part questline with story progression",
    InitialQuestId = "FirstQuestInChain",
    Questlines = new()  // Order matters!
    {
        "Step1Quest",
        "Step2Quest", 
        "Step3Quest",
        "FinalBossQuest"
    },
    FinalRewards = new()
    {
        ("Gold", 300),
        ("LegendaryItem", 1)
    },
    FinalReputation = new()
    {
        (FactionType.Rangers, 30),
        (FactionType.Merchants, 20)
    }
}
```

**Key Points:**
- `InitialQuestId` must be the first quest ID in `Questlines`
- Order in `Questlines` list determines progression sequence
- Player receives final rewards only after completing ALL quests in chain
- Reputation is cumulative across all quests in the line

### Adding EventBars

EventBars represent world conditions. When completed, they trigger cascading effects.

#### Step 1: Add Definition

```csharp
new EventBarDefinition
{
    Id = "MyCustomBar",
    Name = "Merchants Guild Influence",
    Description = "The merchant's guild gains power in the region.",
    Category = "Merchants",
    StartingValue = 0,              // 0-100 range
    DecayAmount = 0.1,              // Loses 0.1 per turn (decay)
    Threshold = 60,                 // Completes when reaches 60
    MinValue = 0,
    MaxValue = 100,
    CheckFrequency = 3,             // Checked every 3 game turns
    CooldownTurns = 20,             // Repeatable bars need 20 turns before next completion
    Repeatable = true,              // Can complete multiple times
    PrerequisiteEventBarIds = new(), // Other bars that must exist first
    CanProgress = world => true,    // Condition to allow progression
    OnCompletion = new EventBarCompletion
    {
        UnlockedQuestLineIds = new() { "MyNewQuestLine" },
        UnlockedEncounterIds = new() { "EncounterId" },
        ZoneEvolutionInfluences = new() { ("DivineOrder", 5) },
        SettlementModifications = new()
        {
            ("StoneHaven", "Wealth", 10),
            ("StoneHaven", "Population", 5),
            ("CrossRoads", "Safety", 3)
        }
    }
}
```

**Parameters Explained:**
- `StartingValue` - Initial value (0-100). Bandits might start at 5 to indicate baseline threat
- `DecayAmount` - How much it decreases per turn (prevents bars from staying completed)
- `Threshold` - Value needed to trigger completion (typically 50-70)
- `Repeatable` - If true, can complete multiple times when threshold is reached again
- `CooldownTurns` - For repeatable bars, turns before it can complete again
- `OnCompletion` - Effects that trigger when bar reaches threshold

#### Step 2: Wire to Player Actions

In `ApplyActionProgression()` (around line 2048), add progression:

```csharp
case PlayerAction.Help:
    AddEventBarProgress(affected, "MyCustomBar", 3);  // +3 progress
    break;
```

#### Step 3: Build & Test

```bash
dotnet build
dotnet run --no-build
```

Take actions that progress the bar → Watch it reach threshold → See cascading effects.

### Adding Settlements

Settlements are NPCs/towns that can be modified by EventBars.

#### Step 1: Add Definition

```csharp
new SettlementDefinition
{
    Id = "MyTown",
    Name = "Prosperous Riverside",
    Type = "Town",  // "Town", "Village", "Monastery", etc.
    Region = RegionId.NorthernRealm,
    PopulationStart = 50,           // Initial population
    WealthStart = 60,               // Initial wealth (0-100)
    SafetyStart = 70,               // Initial safety (0-100)
    InitialBuildings = new()
    {
        "Tavern",
        "Market",
        "Guard Post"
    },
    InitialQuestLineIds = new()
    {
        "MerchantGuild",
        "LocalFarmers"
    },
    Properties = new()
    {
        { "HasMarket", true },
        { "HasTemple", false },
        { "PopulationLimit", 200 }
    }
}
```

**Stats:**
- **Population** (0-1000) - Number of people living there
- **Wealth** (0-100) - Economic prosperity (affects prices/rewards)
- **Safety** (0-100) - How protected it is (affects encounter difficulty)

#### Step 2: Modify via EventBars

In EventBar completion, settlements change:

```csharp
SettlementModifications = new()
{
    ("MyTown", "Wealth", 15),      // +15 wealth
    ("MyTown", "Population", 10),  // +10 population
    ("MyTown", "Safety", -5)       // -5 safety
}
```

#### Step 3: Auto-Discovery

Settlements are auto-discovered when you create them in the registry. Visit them with the `town` command.

---

## Quest System Design (MMO Confirmation)

### ✅ MMO-Style Features Implemented

#### 1. **QuestLine Prerequisites** (Like WoW)
```csharp
Prerequisites = new() { "PreviousQuest" }
// Can't start until previous quest is completed
```
✅ **Implemented** - Quests check prerequisites before allowing acceptance

#### 2. **Quest Chains (Sequential Progression)**
```
Questline: MerchantGuild
├─ RepairMerchantCart [1/5] → +5 MerchantCaravan
├─ EscortMerchant [2/5] → +5 MerchantCaravan
├─ InvestigateAttacks [3/5] → +5 RangerActivity, -5 BanditPatrol
├─ DestroyBanditCamp [4/5] → +10 RangerActivity, -10 BanditPatrol
└─ DefendCaravan [5/5] → +8 MerchantCaravan, unlocks TradeExpansion
```
✅ **Implemented** - Quests unlock sequentially; completing final quest unlocks next QuestLine

#### 3. **Reputation System**
```csharp
ReputationRewards = new() { (FactionType.Merchants, 10) }
// Completing quest changes faction standing
```
**Factions:**
- `Merchants` - Trading factions
- `Rangers` - Law enforcement
- `Clergy` - Religious orders
- `Bandits` - Criminal groups
- `Miners` - Underground workers
- `Nobles` - Aristocracy
- `Mages` - Arcane practitioners

✅ **Implemented** - Reputation tracked per faction; influences NPC interactions

#### 4. **Dynamic Rewards**
```csharp
Rewards = new() { ("Gold", 50), ("Item", 3) }
// Gold + item quantity (can be added to inventory)
```
✅ **Implemented** - Gold rewarded immediately; items added to inventory

#### 5. **Quest Givers/Objectives**
```csharp
Giver = "MerchantGuild",
Description = "A merchant needs help repairing their cart..."
```
✅ **Implemented** - Quests show giver name and objective

#### 6. **Event/Condition Triggers**
```csharp
EventBarEffects = new() { ("MerchantCaravan", 8) }
// Completing quest progresses world condition
```
✅ **Implemented** - Quests modify EventBars, triggering cascading effects

#### 7. **Multi-Objective Quests**
```csharp
ObjectiveDescription = "1. Defeat 5 bandits\n2. Return to town\n3. Report findings"
```
✅ **Implemented** - Quests can have complex descriptions; encounter options reflect objectives

#### 8. **Cascading Quest Unlocks**
```
Completing MerchantGuild (5 quests)
→ Unlocks TradeExpansion questline
→ Unlocks Travelling Merchant encounter
→ +4 DivineOrder (affects zone evolution)
→ StoneHaven +8 wealth, +3 population
```
✅ **Implemented** - Quest completion triggers EventBar effects → cascading unlocks

### System Architecture Comparison

| Feature | WoW | This Engine |
|---------|-----|-------------|
| Quest prerequisites | ✅ | ✅ |
| Quest chains | ✅ | ✅ |
| Reputation system | ✅ | ✅ |
| Dynamic rewards | ✅ | ✅ |
| Quest givers | ✅ | ✅ |
| World impact | ✅ | ✅ (EventBars) |
| Repeatable quests | ✅ | ✅ (with cooldowns) |
| Breadcrumb quests | ✅ | ✅ (via prerequisites) |

### Key Differences from WoW

**This Engine Adds:**
1. **Quantified World Impact** - Quests increase numeric EventBars that affect settlements/zones
2. **Emergent Cascading** - Completing one quest can unlock 3-5 others through EventBar chains
3. **Data-Driven Content** - Add quests purely through registry definitions (no C# coding)
4. **Repeatable Progression** - EventBars cycle (reset when value drops below threshold) allowing infinite quest progression
5. **Settlement Evolution** - Towns grow/shrink based on quest activity, affecting prices/difficulty

---

## Data-Driven Architecture

### How It Works

1. **Registries** (Pure Data)
   - Define all quests, questlines, EventBars, encounters, settlements
   - No logic - just configuration
   - Located in `QuestRegistry`, `EventBarRegistry`, `SettlementRegistry`, etc.

2. **Definitions** (Metadata)
   - `QuestDefinition` - quest name, rewards, effects
   - `EventBarDefinition` - bar name, threshold, completion effects
   - `SettlementDefinition` - town stats, buildings, quests

3. **State** (Runtime Values)
   - `EventBarState` - current bar value (0-100)
   - `SettlementState` - current town population/wealth/safety
   - `QuestLineState` - which quests completed
   - `WorldState` - global game state

4. **Systems** (Engine)
   - `ProgressionSystem` - applies decay, checks completion
   - `QuestSystem` - handles quest logic
   - `EncounterSystem` - triggers encounters
   - `SettlementSystem` - modifies towns

### Adding Content - No Coding Required

**To add a new quest:**
1. Edit `QuestRegistry` in WorldEvolution.cs
2. Add `QuestDefinition` with ID, rewards, effects
3. Build and test
4. Done!

The engine automatically:
- Hooks the quest to encounters
- Progresses EventBars
- Modifies settlements
- Unlocks QuestLines
- Tracks reputation

### Production-Ready Advantages

✅ **Separation of Concerns** - Data (registries) separate from Logic (systems)
✅ **Easy Content Addition** - Add quests without understanding code
✅ **Extensible** - New systems can read registries without touching existing code
✅ **Maintainable** - All world content in one place, easy to audit
✅ **Testable** - Mock registries, test systems independently
✅ **Version Control** - Registry changes show exactly what content changed

---

## Troubleshooting

### Build Fails
```bash
dotnet clean
dotnet restore
dotnet build
```

### Simulation Won't Start
Check [Program.cs](Program.cs) line 1 - ensure `WorldCampaign` is initialized correctly.

### EventBars Not Progressing
Check `ApplyActionProgression()` in [WorldEvolution.cs](WorldEvolution.cs) - ensure your action adds progress to the target bar.

### Quests Don't Unlock
Verify the quest's `Prerequisites` are completed and the EventBar completion effects include `UnlockedQuestLineIds`.

---

## Next Steps

### Short Term
- [x] Implement EventBar system
- [x] Implement QuestLine chains
- [x] Implement settlement modifications
- [x] Add reputation tracking
- [ ] Expand to all 4 regions
- [ ] Add inter-regional trade routes

### Medium Term
- [ ] Implement zone evolution tier system
- [ ] Add dynamic NPC dialogue
- [ ] Create equipment system
- [ ] Add skill progression

### Long Term
- [ ] Multiplayer synchronization
- [ ] Persistent world saves
- [ ] Web frontend
- [ ] Content editor UI

---

## File Reference

- **[WorldEvolution.cs](WorldEvolution.cs)** - Monolithic engine (3225 lines, to be split)
- **[Program.cs](Program.cs)** - Interactive campaign loop
- **[ProceduralGameSimulation.csproj](ProceduralGameSimulation.csproj)** - Project configuration

---

**Created:** 2026-07-05
**Maintained By:** You
**License:** TBD
