using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Master definitions for all 7 regions in the world</summary>
    public class WorldDefinitions
    {
        public static readonly Dictionary<RegionId, RegionDefinition> AllRegions = new()
        {
            {
                RegionId.NorthernRealm,
                new RegionDefinition
                {
                    RegionId = RegionId.NorthernRealm,
                    Name = "Northern Realm",
                    Biome = BiomeType.Plains,
                    HasTown = true,
                    HasWater = false,
                    AdjacentRegions = new() { RegionId.DarkForest, RegionId.Highlands, RegionId.ArcaneEmpire },
                    DefaultPaths = new() { EvolutionPath.AgeOfRaiders, EvolutionPath.EternalWinter, EvolutionPath.Ragnarok, EvolutionPath.UndeadNorth }
                }
            },
            {
                RegionId.DarkForest,
                new RegionDefinition
                {
                    RegionId = RegionId.DarkForest,
                    Name = "Dark Forest",
                    Biome = BiomeType.Forest,
                    HasTown = false,
                    HasWater = true,
                    AdjacentRegions = new() { RegionId.NorthernRealm, RegionId.Swamp, RegionId.ArcaneEmpire },
                    DefaultPaths = new() { EvolutionPath.PrimordialJungle, EvolutionPath.CorruptedAbyss, EvolutionPath.ElvenEnclave, EvolutionPath.CursedWildlands }
                }
            },
            {
                RegionId.DesertKingdom,
                new RegionDefinition
                {
                    RegionId = RegionId.DesertKingdom,
                    Name = "Desert Kingdom",
                    Biome = BiomeType.Desert,
                    HasTown = true,
                    HasWater = false,
                    AdjacentRegions = new() { RegionId.Swamp, RegionId.Highlands, RegionId.InfernalLands },
                    DefaultPaths = new() { EvolutionPath.AgeOfMerchants, EvolutionPath.AgeOfPharaohs, EvolutionPath.ElementalCatastrophe, EvolutionPath.ShadowAndSecrets }
                }
            },
            {
                RegionId.Swamp,
                new RegionDefinition
                {
                    RegionId = RegionId.Swamp,
                    Name = "Swamp",
                    Biome = BiomeType.Swamp,
                    HasTown = false,
                    HasWater = true,
                    AdjacentRegions = new() { RegionId.DarkForest, RegionId.DesertKingdom, RegionId.Highlands },
                    DefaultPaths = new() { EvolutionPath.PrimordialBog, EvolutionPath.PlaguePestilence, EvolutionPath.MonsterDominion, EvolutionPath.MysticalAwakening }
                }
            },
            {
                RegionId.Highlands,
                new RegionDefinition
                {
                    RegionId = RegionId.Highlands,
                    Name = "Highlands",
                    Biome = BiomeType.Mountain,
                    HasTown = true,
                    HasWater = false,
                    AdjacentRegions = new() { RegionId.NorthernRealm, RegionId.Swamp, RegionId.DesertKingdom, RegionId.InfernalLands },
                    DefaultPaths = new() { EvolutionPath.DwarvenProsperity, EvolutionPath.AsceticHermitage, EvolutionPath.DragonRoost, EvolutionPath.CorruptionBlight }
                }
            },
            {
                RegionId.InfernalLands,
                new RegionDefinition
                {
                    RegionId = RegionId.InfernalLands,
                    Name = "Infernal Lands",
                    Biome = BiomeType.Infernal,
                    HasTown = false,
                    HasWater = false,
                    AdjacentRegions = new() { RegionId.Highlands, RegionId.DesertKingdom, RegionId.ArcaneEmpire },
                    DefaultPaths = new() { EvolutionPath.DemonicDominion, EvolutionPath.VolcanicCatastrophe, EvolutionPath.DemonicElementalFusion, EvolutionPath.EternalForge }
                }
            },
            {
                RegionId.ArcaneEmpire,
                new RegionDefinition
                {
                    RegionId = RegionId.ArcaneEmpire,
                    Name = "Arcane Empire",
                    Biome = BiomeType.Arcane,
                    HasTown = true,
                    HasWater = true,
                    AdjacentRegions = new() { RegionId.NorthernRealm, RegionId.DarkForest, RegionId.InfernalLands },
                    DefaultPaths = new() { EvolutionPath.ArcaneAscendance, EvolutionPath.KnowledgeHoarding, EvolutionPath.TechnoMagicalSynthesis, EvolutionPath.CorruptionDecay }
                }
            }
        };

        public static RegionDefinition GetDefinition(RegionId regionId)
        {
            return AllRegions.TryGetValue(regionId, out var def) ? def : null;
        }
    }
}
