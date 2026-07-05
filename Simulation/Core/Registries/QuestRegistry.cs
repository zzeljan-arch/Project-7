using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Master list of all 30+ quests in the game organized by quest line</summary>
    public static class QuestRegistry
    {
        public static readonly List<QuestDefinition> AllQuests = new()
        {
            // ===== MERCHANT GUILD QUESTLINE =====
            new QuestDefinition
            {
                Id = "RepairMerchantCart",
                Title = "Repair the Merchant's Cart",
                Description = "Fix a broken wagon for a traveling merchant.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 40, ReputationChanges = new() { { FactionType.Merchants, 3 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 8 } }
            },
            new QuestDefinition
            {
                Id = "EscortMerchant",
                Title = "Escort the Merchant",
                Description = "Guide a merchant safely through dangerous roads.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 55, ReputationChanges = new() { { FactionType.Merchants, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 10 }, new QuestEffect { BarId = "BanditPatrol", Amount = -5 } }
            },
            new QuestDefinition
            {
                Id = "InvestigateAttacks",
                Title = "Investigate Merchant Attacks",
                Description = "Look into a series of attacks on merchant caravans.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 65, ReputationChanges = new() { { FactionType.Merchants, 5 }, { FactionType.Rangers, 2 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "BanditPatrol", Amount = -10 } }
            },
            new QuestDefinition
            {
                Id = "DestroyBanditCamp",
                Title = "Destroy the Bandit Camp",
                Description = "Eliminate a bandit camp threatening trade routes.",
                CanStart = world => true,
                RequiredEncounterIds = new() { "BanditStronghold" },
                Reward = new QuestReward { Gold = 100, ReputationChanges = new() { { FactionType.Merchants, 8 }, { FactionType.Rangers, 5 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "BanditPatrol", Amount = -25 } }
            },
            new QuestDefinition
            {
                Id = "DefendCaravan",
                Title = "Defend the Caravan",
                Description = "Protect a merchant caravan from ambush.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 75, ReputationChanges = new() { { FactionType.Merchants, 6 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 12 } }
            },

            // ===== TRADE EXPANSION QUESTLINE =====
            new QuestDefinition
            {
                Id = "SupplyDistrict",
                Title = "Supply the District",
                Description = "Gather supplies for a new merchant district.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 60, ReputationChanges = new() { { FactionType.Merchants, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 10 } }
            },
            new QuestDefinition
            {
                Id = "SecureRoute",
                Title = "Secure a New Trade Route",
                Description = "Clear a path for a new merchant trade route.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 80, ReputationChanges = new() { { FactionType.Merchants, 6 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "BanditPatrol", Amount = -15 } }
            },
            new QuestDefinition
            {
                Id = "BuildTradingPost",
                Title = "Build a Trading Post",
                Description = "Establish a new trading post in the region.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 120, ReputationChanges = new() { { FactionType.Merchants, 10 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 15 } }
            },

            // ===== CLERICAL TRAINING QUESTLINE =====
            new QuestDefinition
            {
                Id = "CollectHolyRelics",
                Title = "Collect Holy Relics",
                Description = "Gather sacred relics for temple ceremonies.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 45, ReputationChanges = new() { { FactionType.Clergy, 3 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 8 } }
            },
            new QuestDefinition
            {
                Id = "TrainNovices",
                Title = "Train Temple Novices",
                Description = "Help train new priests and clerics.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 50, ReputationChanges = new() { { FactionType.Clergy, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 10 } }
            },
            new QuestDefinition
            {
                Id = "DefendTemple",
                Title = "Defend the Temple",
                Description = "Protect the temple from desecration and attack.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 70, ReputationChanges = new() { { FactionType.Clergy, 8 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 12 } }
            },

            // ===== RANGER RECRUIT QUESTLINE =====
            new QuestDefinition
            {
                Id = "HuntWildbeasts",
                Title = "Hunt the Wildebeasts",
                Description = "Hunt and cull dangerous creatures threatening settlements.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 55, ReputationChanges = new() { { FactionType.Rangers, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "RangerActivity", Amount = 9 }, new QuestEffect { BarId = "WildlifeEncroach", Amount = -8 } }
            },
            new QuestDefinition
            {
                Id = "PatrolTradRoutes",
                Title = "Patrol Trade Routes",
                Description = "Patrol and protect trading routes from dangers.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 60, ReputationChanges = new() { { FactionType.Rangers, 5 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "RangerActivity", Amount = 11 } }
            },
            new QuestDefinition
            {
                Id = "EliminateBandits",
                Title = "Eliminate the Bandit Threat",
                Description = "Lead a decisive strike against organized bandit groups.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 85, ReputationChanges = new() { { FactionType.Rangers, 10 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "RangerActivity", Amount = 10 }, new QuestEffect { BarId = "BanditPatrol", Amount = -18 } }
            },

            // ===== FARMING SUPPORT QUESTLINE =====
            new QuestDefinition
            {
                Id = "HelpWithHarvest",
                Title = "Help with the Harvest",
                Description = "Assist farmers in bringing in the autumn crop.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 35, ReputationChanges = new() { { FactionType.Merchants, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "FarmersNeed", Amount = 7 } }
            },
            new QuestDefinition
            {
                Id = "RepairFarmEquipment",
                Title = "Repair Farm Equipment",
                Description = "Fix broken plows, tools, and other farm implements.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 40, ReputationChanges = new() { { FactionType.Merchants, 5 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "FarmersNeed", Amount = 8 } }
            },
            new QuestDefinition
            {
                Id = "ProtectFromWildlife",
                Title = "Protect from Wildlife",
                Description = "Guard fields and livestock from wild animal attacks.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 55, ReputationChanges = new() { { FactionType.Merchants, 6 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "FarmersNeed", Amount = 10 }, new QuestEffect { BarId = "WildlifeEncroach", Amount = -12 } }
            }
        };
    }
}
