# Quick Start Guide - 5 Minute Setup

## Installation & First Run

### 1. Prerequisites
- .NET 6.0 SDK installed
- Windows/Linux/macOS

### 2. Clone/Navigate to Project
```bash
cd c:\Users\ljova\Desktop\Project7
```

### 3. Build the Game
```bash
dotnet build ProceduralGame.sln
```

Should see: ✅ `Build succeeded with 6 warning(s)`

### 4. Launch the Interactive Campaign
```bash
cd Simulation
dotnet run --no-build
```

You're now in the living world! 🌍

## First 2 Minutes: What to Do

### Try These Commands:

```
> explore
[EVENT] You venture forth into the surrounding land.
[SIM] World advanced by 120 minutes.
[EVENT] Encounter: Stranded Merchant...
```

Choose option 1, 2, or 3 to interact with the encounter.

```
> character
[EVENT] Health: 100/100
[EVENT] Merchants: 0.5
[EVENT] Rangers: 2.0
```

See your reputation changing as you interact.

```
> history
-- World Event History --
Campaign seeded with 42 starting at NorthernRealm
[BAR] Bandit Clearance +35.0: 0.0  35.0/75
[BAR] Bandit Clearance decay: 35.0  33.7/75
Reputation with Rangers changed by 2.5.
```

Watch EventBars progress and decay!

```
> help
Available commands:
  explore, rest, town, market, inventory, character, quests, ...
```

## Basic Gameplay Loop

1. **Explore** - Find encounters and choose how to handle them
2. **Accept Quests** - Visit towns and accept available quests
3. **Complete Encounters** - Quest objectives often appear as encounters
4. **Turn In Quests** - Return to town after completing quest
5. **Watch World Change** - Settlements and EventBars respond to your actions

## Adding Your First Quest

### Modify The Code

1. Open: `Simulation/WorldEvolution.cs`
2. Search for: `public static class QuestRegistry`
3. At the bottom of the quest list, add:

```csharp
new QuestDefinition
{
    Id = "MyFirstQuest",
    Title = "My Custom Quest",
    Description = "I created this quest!",
    Region = RegionId.NorthernRealm,
    Giver = "MyNpc",
    Prerequisites = new(),
    Rewards = new() { ("Gold", 50) },
    ReputationRewards = new() { (FactionType.Merchants, 5) },
    EventBarEffects = new() { ("MerchantCaravan", 8) },
    ZoneEvolutionInfluences = new(),
    SettlementModifications = new()
},
```

4. Save the file
5. Build: `dotnet build`
6. Run: `dotnet run --no-build`
7. In-game: Visit town → Accept "My Custom Quest" → See it progress

### Add to a QuestLine

Search for `public static class QuestLineRegistry` and add your quest ID to the Questlines list:

```csharp
Questlines = new()
{
    "RepairMerchantCart",
    "EscortMerchant",
    "MyFirstQuest",  // <-- Add your quest here
    "InvestigateAttacks",
    // ...
}
```

## Key Concepts in 30 Seconds

- **EventBars** - World conditions (0-100) that represent regions states
- **QuestLines** - Chains of quests that unlock sequentially
- **Settlements** - Towns that grow/shrink based on EventBars
- **Cascading Effects** - Quest completion → EventBar progression → settlement changes → zone evolution

## File Quick Reference

| File | Purpose |
|------|---------|
| [README_PRODUCTION.md](README_PRODUCTION.md) | Full documentation |
| [QUEST_TEMPLATE.cs](Content/QUEST_TEMPLATE.cs) | How to add quests |
| [EVENTBAR_TEMPLATE.cs](Content/EVENTBAR_TEMPLATE.cs) | How to add EventBars |
| [WorldEvolution.cs](WorldEvolution.cs) | Main engine (3200+ lines) |
| [Program.cs](Program.cs) | Game loop & UI |

## Common Issues

### "Build failed"
```bash
dotnet clean
dotnet restore
dotnet build
```

### "Quest doesn't appear"
Check that quest's `Prerequisites` are empty or completed.
Check that the EventBar triggering it has completed.

### "EventBar not progressing"
Check `ApplyActionProgression()` in WorldEvolution.cs (line ~2048).
Ensure your action type adds progress to the bar.

## What's Happening Under the Hood?

1. You take an action (explore, help, defend, etc.)
2. EventBars advance based on the action
3. Player reputation changes
4. World time advances
5. EventBars decay over time
6. When EventBar reaches threshold → completion triggers
7. Completion effects: quests unlock, settlements change, zone bars adjust
8. Repeat!

## Next Steps

- Read [README_PRODUCTION.md](README_PRODUCTION.md) for complete guide
- Use [QUEST_TEMPLATE.cs](Content/QUEST_TEMPLATE.cs) to add new quests
- Use [EVENTBAR_TEMPLATE.cs](Content/EVENTBAR_TEMPLATE.cs) to add new EventBars
- Explore the [Core/](Core/) folder structure for advanced modifications

## Troubleshooting

```bash
# Check what's running
Get-Process | Select-Object ProcessName

# Kill stuck simulation
Stop-Process -Name ProceduralGameSimulation -Force

# Full rebuild
dotnet clean && dotnet restore && dotnet build
```

---

**Estimated time to first working quest: ~3 minutes**

Let's go! 🚀
