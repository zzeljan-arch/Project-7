using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Cascading effects triggered when an EventBar completes</summary>
    public class EventBarCompletion
    {
        public List<string> UnlockedEncounterIds { get; set; } = new();
        public List<string> UnlockedQuestLineIds { get; set; } = new();
        public List<string> SettlementEffects { get; set; } = new();
        public List<(string SettlementId, string EffectType, double Amount)> SettlementModifications { get; set; } = new();
        public List<(string ZoneBarId, double Amount)> ZoneEvolutionInfluences { get; set; } = new();
    }

    /// <summary>Definition of a regional progression bar (e.g., Merchant Activity, Bandit Patrols)</summary>
    public class EventBarDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double StartingValue { get; set; }
        public double DecayAmount { get; set; }
        public double Threshold { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int CheckFrequency { get; set; }
        public List<string> PrerequisiteEventBarIds { get; set; } = new();
        public int CooldownTurns { get; set; }
        public bool Repeatable { get; set; }
        public EventBarCompletion OnCompletion { get; set; } = new();
        public Func<WorldState, bool> CanProgress { get; set; }
    }
}
