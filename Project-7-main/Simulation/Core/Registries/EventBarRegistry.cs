using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Master list of all EventBar definitions that appear in each region</summary>
    public static class EventBarRegistry
    {
        public static readonly List<EventBarDefinition> AllEventBarDefinitions = new()
        {
            new EventBarDefinition
            {
                Id = "MerchantCaravan",
                Name = "Merchant Caravan Activity",
                Description = "Trade caravans pass through the region bringing prosperity.",
                Category = "Economy",
                StartingValue = 42,
                DecayAmount = 0.8,
                Threshold = 60,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 2,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestLineIds = new() { "TradeExpansion" },
                    SettlementModifications = new() { ("StoneHaven", "Wealth", 15) }
                }
            },
            new EventBarDefinition
            {
                Id = "BanditPatrol",
                Name = "Bandit Patrol Frequency",
                Description = "Bandit activity in the region.",
                Category = "Danger",
                StartingValue = 45,
                DecayAmount = 1.0,
                Threshold = 75,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 3,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    UnlockedEncounterIds = new() { "BanditStronghold" },
                    SettlementModifications = new() { ("StoneHaven", "Safety", -20) }
                }
            },
            new EventBarDefinition
            {
                Id = "TempleActivity",
                Name = "Temple Activity",
                Description = "Religious events and ceremonies in the region.",
                Category = "Culture",
                StartingValue = 15,
                DecayAmount = 1.0,
                Threshold = 50,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 3,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestLineIds = new() { "ClericTraining" }
                }
            },
            new EventBarDefinition
            {
                Id = "RangerActivity",
                Name = "Ranger Patrol Activity",
                Description = "Rangers patrol the wilderness protecting travelers.",
                Category = "Safety",
                StartingValue = 25,
                DecayAmount = 1.2,
                Threshold = 65,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 2,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestLineIds = new() { "RangerRecruit" },
                    SettlementModifications = new() { ("StoneHaven", "Safety", 10) }
                }
            },
            new EventBarDefinition
            {
                Id = "FarmersNeed",
                Name = "Farmers' Needs",
                Description = "Agricultural demands and seasonal concerns.",
                Category = "Economy",
                StartingValue = 20,
                DecayAmount = 0.8,
                Threshold = 55,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 3,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    UnlockedQuestLineIds = new() { "FarmingSupport" }
                }
            },
            new EventBarDefinition
            {
                Id = "WildlifeEncroach",
                Name = "Wildlife Encroachment",
                Description = "Dangerous creatures moving closer to settlements.",
                Category = "Danger",
                StartingValue = 10,
                DecayAmount = 1.5,
                Threshold = 70,
                MinValue = 0,
                MaxValue = 100,
                CheckFrequency = 2,
                Repeatable = true,
                CanProgress = world => true,
                OnCompletion = new EventBarCompletion
                {
                    SettlementModifications = new() { ("StoneHaven", "Safety", -15) }
                }
            }
        };
    }
}
