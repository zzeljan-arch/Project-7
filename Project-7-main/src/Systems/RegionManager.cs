using System;
using System.Collections.Generic;
using PG.World.Simulation;

namespace PG.Systems
{
    public class RegionManager
    {
        private readonly DeterministicRandom _rng;
        private readonly Dictionary<RegionId, RegionState> _regions;

        public RegionManager(int seed)
        {
            _rng = new DeterministicRandom((ulong)seed);
            _regions = new Dictionary<RegionId, RegionState>();
            InitializeRegions();
        }

        private void InitializeRegions()
        {
            foreach (RegionId regionId in Enum.GetValues(typeof(RegionId)))
            {
                var paths = EvolutionRules.GetPathsForRegion(regionId);
                var selectedPath = paths[_rng.Next(paths.Count)];
                _regions[regionId] = new RegionState(regionId, WorldTier.Tier1, selectedPath);
            }
        }

        public RegionState GetInitialRegion()
        {
            return _regions[RegionId.NorthernRealm];
        }

        public RegionState GetRegion(RegionId regionId)
        {
            return _regions.TryGetValue(regionId, out var region) ? region : null;
        }
    }
}
