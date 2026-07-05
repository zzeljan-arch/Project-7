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
}
