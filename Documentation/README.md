# World Evolution Simulator (C#)

This is a deterministic, headless simulator for testing the world evolution engine before integrating into Godot.

## Features

- **Deterministic:** Seeded RNG ensures reproducible campaigns
- **Fast:** Runs hundreds of campaigns in seconds for variance analysis
- **Modular:** Easy to adjust evolution rules, tier spreading, and path probabilities
- **Metrics:** Novelty Tracker measures campaign variance and path distribution

## Build & Run

```bash
cd Simulation
dotnet build
dotnet run
```

## Key Classes

- `RegionState` - Represents a region's current tier and evolution path
- `WorldCampaign` - Complete world state for one campaign
- `EvolutionRules` - Encapsulates action influence, path weighting, and path assignment logic
- `DeterministicRandom` - Seedable RNG for reproducibility
- `NoveltyTracker` - Analyzes variance across multiple campaigns

## Customization

### Modify Evolution Paths

Edit the paths in `WorldEvolution.cs` under `EvolutionRules.GetPathsForRegion()`.

### Adjust Action Influence

Edit `EvolutionRules` and `WorldCampaign.PerformPlayerAction()` to change how actions affect nearby regions and path selection.

### Run Custom Campaign

```csharp
var campaign = new WorldCampaign(seed: 12345, startingRegion: RegionId.DarkForest);
campaign.PerformPlayerAction(RegionId.NorthernRealm, PlayerAction.Defend);
Console.WriteLine(campaign.ToString());
```

## Metrics Explained

- **Path Occurrence:** How many times each region/path combination appeared
- **Std Dev:** Lower = more balanced distribution across paths
- **Novelty Score:** 0-1 scale; 1.0 = completely novel compared to previous campaigns

### Goal

Target: **<=20% repetition over 10 campaigns** = at least 80% unique scenarios per 10 runs.

## Next Steps

1. Run multiple batch analyses to identify over/under-represented paths
2. Tune probabilities in `EvolutionRules` based on results
3. Add path-specific event logic (e.g., Undead spreads curse, Winter spreads cold)
4. Port stable rules to Godot C# integration

---

For integration with Godot, keep this simulator as a standalone validation tool and port the core `EvolutionRules` logic into `src/World` modules.
