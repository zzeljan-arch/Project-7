using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Rewards given when a quest is completed</summary>
    public class QuestReward
    {
        public double? Gold { get; set; }
        public List<ItemStack> Items { get; set; } = new();
        public Dictionary<FactionType, double> ReputationChanges { get; set; } = new();
        public List<string> UnlockedQuestLineIds { get; set; } = new();
        public List<string> UnlockedEncounterIds { get; set; } = new();
    }

    /// <summary>Effect a quest has on EventBars when completed</summary>
    public class QuestEffect
    {
        public string BarId { get; set; }
        public double Amount { get; set; }
        public RegionId? TargetRegion { get; set; }
    }

    /// <summary>Single quest definition - immutable template for a quest</summary>
    public class QuestDefinition
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Func<WorldState, bool> CanStart { get; set; }
        public List<string> RequiredEncounterIds { get; set; } = new();
        public QuestReward Reward { get; set; } = new();
        public List<QuestEffect> EventBarEffects { get; set; } = new();
        public List<(string ZoneBarId, double Amount)> ZoneEvolutionEffects { get; set; } = new();
        public List<(string HiddenBarId, double Amount)> HiddenBarEffects { get; set; } = new();
        public List<string> SettlementEffects { get; set; } = new();
    }

    /// <summary>Chain of related quests that must be done in sequence</summary>
    public class QuestLineDefinition
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> QuestIds { get; set; } = new();
        public Func<WorldState, bool> CanStart { get; set; }
        public List<string> PrerequisiteQuestLineIds { get; set; } = new();
        public QuestReward FinalReward { get; set; } = new();
        public Action<WorldState> OnComplete { get; set; }
    }
}
