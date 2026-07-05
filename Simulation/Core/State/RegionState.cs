using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class RegionState
    {
        public RegionId RegionId { get; set; }
        public WorldTier CurrentTier { get; set; }
        public EvolutionPath CurrentPath { get; set; }
        public EvolutionPath DefaultPath { get; set; }
        public bool HasBeenVisited { get; set; }
        public bool IsLocked { get; set; }
        public Dictionary<string, ProgressionBarState> ProgressionBars { get; set; } = new();
        public Dictionary<string, ZoneEvolutionBarState> ZoneEvolutionBars { get; set; } = new();
        public Dictionary<string, EventBarState> EventBars { get; set; } = new();
        public Dictionary<string, SettlementState> Settlements { get; set; } = new();
        public RegionDefinition Definition { get; set; }

        public RegionState(RegionDefinition definition, WorldTier tier, EvolutionPath defaultPath)
        {
            Definition = definition;
            RegionId = definition.RegionId;
            CurrentTier = tier;
            DefaultPath = defaultPath;
            CurrentPath = defaultPath;
        }

        public RegionState(RegionId regionId, WorldTier tier, EvolutionPath defaultPath)
            : this(WorldDefinitions.GetDefinition(regionId), tier, defaultPath)
        {
        }

        public override string ToString()
        {
            if (CurrentPath == EvolutionPath.None)
                return $"{Definition.Name} (Tier {(int)CurrentTier})";
            return $"{Definition.Name} - {CurrentPath} (Tier {(int)CurrentTier})";
        }
    }
}
