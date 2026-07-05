using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>One player choice during an encounter</summary>
    public class EncounterOption
    {
        public string Description { get; set; }
        public Func<WorldState, EncounterResult> Execute { get; set; }
    }

    /// <summary>Outcome of an encounter option chosen by player</summary>
    public class EncounterResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public bool PlayerVisible { get; set; }
    }

    /// <summary>Definition of an event player can encounter while exploring</summary>
    public class EncounterDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public EncounterDifficulty Difficulty { get; set; }
        public Func<WorldState, bool> CanOccur { get; set; }
        public Func<WorldState, double> Weight { get; set; }
        public List<EncounterOption> Options { get; set; } = new();
        public string RelatedQuestId { get; set; }
    }

    /// <summary>World-changing event triggered when condition is met</summary>
    public class LongTermEventDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Func<WorldState, bool> Condition { get; set; }
        public Action<WorldState> OnTrigger { get; set; }
        public bool HasTriggered { get; set; }
    }
}
