using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Structure in a settlement (e.g., Market, Temple, Guard Tower)</summary>
    public class SettlementBuilding
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public int Level { get; set; }
    }

    /// <summary>Service provided in a settlement (e.g., Trading, Healing, Training)</summary>
    public class SettlementService
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
    }

    /// <summary>Definition of a settlement that exists in a region</summary>
    public class SettlementDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public RegionId Region { get; set; }
        public double PopulationStart { get; set; }
        public double WealthStart { get; set; }
        public double SafetyStart { get; set; }
        public List<string> InitialBuildings { get; set; } = new();
        public List<string> InitialQuestLineIds { get; set; } = new();
        public Dictionary<string, object> Properties { get; set; } = new();
    }
}
