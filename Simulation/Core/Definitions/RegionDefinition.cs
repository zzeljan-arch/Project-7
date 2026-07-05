using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Static definition of a region (immutable)</summary>
    public class RegionDefinition
    {
        public RegionId RegionId { get; set; }
        public string Name { get; set; }
        public BiomeType Biome { get; set; }
        public bool HasTown { get; set; }
        public bool HasWater { get; set; }
        public List<RegionId> AdjacentRegions { get; set; } = new();
        public List<EvolutionPath> DefaultPaths { get; set; } = new();
    }
}
