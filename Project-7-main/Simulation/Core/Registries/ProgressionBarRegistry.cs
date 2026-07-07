using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    public static class ProgressionBarRegistry
    {
        public static readonly List<ProgressionBarDefinition> GlobalBarDefinitions = new()
        {
            new ProgressionBarDefinition
            {
                Id = "WorldStability",
                Name = "World Stability",
                Threshold = 100,
                DecayAmount = 1.5,
                MinValue = 0,
                MaxValue = 150,
                CheckFrequency = 4,
                CanProgress = world => true,
                OnComplete = world => world.Log(LogCategory.Simulation, "The world has stabilized around several factions."),
                OnDecay = world => { }
            }
        };

        public static readonly List<ProgressionBarDefinition> AllRegionBarDefinitions = new()
        {
            new ProgressionBarDefinition
            {
                Id = "BanditClearance",
                Name = "Bandit Clearance",
                Threshold = 75,
                DecayAmount = 2.5,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 2,
                CanProgress = world => true,
                OnComplete = world => world.Log(LogCategory.Simulation, "A region has cleared enough bandits to secure a new route."),
                OnDecay = world => { }
            },
            new ProgressionBarDefinition
            {
                Id = "MagicalAwareness",
                Name = "Magical Awareness",
                Threshold = 60,
                DecayAmount = 1.8,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 3,
                CanProgress = world => true,
                OnComplete = world => world.Log(LogCategory.Simulation, "Arcane activity has become visible across the region."),
                OnDecay = world => { }
            }
        };
    }
}
