using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class ProgressionBarDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Threshold { get; set; }
        public double DecayAmount { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int CheckFrequency { get; set; }
        public Func<WorldState, bool> CanProgress { get; set; }
        public Action<WorldState> OnComplete { get; set; }
        public Action<WorldState> OnDecay { get; set; }
    }

    public class ZoneEvolutionInfluence
    {
        public string SourceType { get; set; }
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public double Amount { get; set; }
    }

    public class ZoneEvolutionBarDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double StartingValue { get; set; }
        public double DecayAmount { get; set; }
        public Action<WorldState, ZoneEvolutionMilestone> OnMilestoneReached { get; set; }
        public Action<WorldState> OnMaxReached { get; set; }
    }
}
