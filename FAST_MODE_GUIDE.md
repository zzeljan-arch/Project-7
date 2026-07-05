# Fast Simulation Mode - Demonstration Guide

## What's Been Fixed

### ✅ Player Stays in Region
Fast mode no longer has the player travel between regions. Player remains in Northern Realm and focuses on exploring to progress EventBars and encounter meaningful world evolution.

### ✅ Encounters Progress EventBars
- **Stranded Merchant** encounters now progress **Merchant Caravan Activity** EventBar
  - "Repair the wagon" → +16 progress
  - "Escort them" → +10 progress
- **Bandit Ambush** encounters now progress **Bandit Patrol Frequency** EventBar
  - "Fight the bandits" → +12 progress
  - "Pay them off" → +3 progress

### ✅ EventBar Completions Trigger Cascading Effects
When EventBars reach their threshold, they unlock questlines and modify settlements:

**Example: Merchant Caravan Activity Completion Chain**
```
Player repairs merchant wagons multiple times
           ↓
Merchant Caravan Activity EventBar progresses toward 60
           ↓
EventBar reaches threshold (61/60)
           ↓
[COMPLETION] Merchant Caravan Activity reached threshold
   → Unlocks questline: TradeExpansion
   → Stone Haven Wealth: +15
```

### ✅ Balanced Tuning for Viable Progression
- **Merchant Caravan Activity**: Starts at 42 (was 20), Threshold 60, Decay 0.8/action (was 1.5)
- **Bandit Patrol Frequency**: Starts at 45 (was 30), Threshold 75, Decay 1.0/action (was 2.0)
- Encounter effects increased (repairs now give significant progress)

### ✅ Full Event Logging During Fast Simulation
Logs show:
- Individual EventBar decay/progress with percentage to threshold
- All encounters and their outcomes with EventBar modifications
- EventBar completions and their cascading effects (questlines, settlements)
- Player actions and discoveries

---

## Running Demonstration Campaigns

### Command: Show Merchant Progression Chain (200 Actions)
```powershell
cd c:\Users\ljova\Desktop\Project7\Simulation
dotnet run -- fast 1 200
```

**Expected Output:**
- Multiple "Stranded Merchant" encounters
- Player repairs wagons ("+16 to Merchant Caravan")
- Merchant Caravan Activity bar progresses toward 60
- `[COMPLETION] Merchant Caravan Activity reached threshold (61/60)`
- `→ Unlocks questline: TradeExpansion`
- `→ Stone Haven Wealth: +15`
- Bandit Patrol also progresses from encounters
- Final summary showing Gold, Health, and Novelty score

### Command: Show Quick Multi-Campaign Overview (50 Actions Each)
```powershell
cd c:\Users\ljova\Desktop\Project7\Simulation
dotnet run -- fast 3 50
```

Runs 3 campaigns showing different progression paths and EventBar values.

### Command: Extended Single Campaign (300+ Actions)
```powershell
cd c:\Users\ljova\Desktop\Project7\Simulation
dotnet run -- fast 1 300
```

Shows complete region evolution with multiple EventBar completions and settlement effects.

---

## Example Output from 200-Action Run

```
[SIM] Campaign seeded with 1000 starting at NorthernRealm

[EVENT] You venture forth into the surrounding land.
[EVENT] Encounter: Stranded Merchant - A trader is stranded after a broken wagon and needs aid.
[EVENT] You repair the wagon! Merchant Caravan activity +16

[SIM] [Northern Realm] Merchant Caravan Activity: 42.0 -> 41.2 (69% to threshold)

... (many more encounters and bar changes) ...

[EVENT] You repair the wagon! Merchant Caravan activity +16
[SIM] [Northern Realm] Merchant Caravan Activity: 61.6 -> 60.8 (101% to threshold)
[SIM] [COMPLETION] Merchant Caravan Activity reached threshold (61/60)
[SIM]   → Unlocks questline: TradeExpansion
[SIM]   → Stone Haven Wealth: +15

--- Campaign 1 Summary: Gold=8, Health=100/100, Novelty=100.0% ---
```

---

## Key Observations During Fast Simulation

1. **EventBar Decay**: All bars decay each action (~0.8-1.0 per action depending on bar)
2. **Encounters**: Roughly 25-30% of exploration actions trigger encounters
3. **Repair Frequency**: Stranded Merchants appear in ~40-50% of encounters, with random choice of helping or stealing
4. **Progress Rate**: With balanced tuning, EventBars can reach completion within 150-250 actions
5. **Settlement Effects**: Completions modify nearby settlements (wealth, safety changes)

---

## What's Not Yet Implemented (Noted in Red in Logs)

- ❌ ZoneEvolutionBar interactions (noted in logs when present)
- ❌ Full Bandit Stronghold encounter when BanditPatrol completes
- ❌ Regional tier evolution based on EventBar completions
- ❌ NPC faction interactions and reputation changes
- ❌ Full 15+ encounters in EncounterRegistry (currently has 2 core encounters)

These are marked in the simulation logs where they would appear, making it easy to see what progression paths are available vs still in development.

---

## Configuration for Custom Tuning

To adjust EventBar progression difficulty, edit:

**EventBarRegistry.cs:**
- `StartingValue`: Higher = closer to completion initially
- `DecayAmount`: Higher = faster decay (harder to complete)
- `Threshold`: Target value for completion

**WorldEvolution.cs (EncounterRegistry):**
- Adjust encounter effect amounts (currently at +16 for repairs, +12 for combat)

Example: Make merchant progression easier by increasing StartingValue to 50 or reducing Threshold to 50.

---

## Next Steps

The system is now ready for:
1. Adding more encounters with EventBar effects
2. Implementing ZoneEvolutionBar cascades
3. Creating region tier advancement based on EventBar chains
4. Adding faction reputation changes from encounters
5. Implementing settlement marketplace/buildings based on bar completions
