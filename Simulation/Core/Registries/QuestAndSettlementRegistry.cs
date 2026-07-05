using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Master list of the 5 quest lines (chains of related quests)</summary>
    public static class QuestLineRegistry
    {
        public static readonly List<QuestLineDefinition> AllQuestLines = new()
        {
            new QuestLineDefinition
            {
                Id = "MerchantGuild",
                Title = "Merchant Guild Ascendant",
                Description = "Support merchant trade and build prosperity throughout the realm.",
                CanStart = world => true,
                PrerequisiteQuestLineIds = new(),
                QuestIds = new() { "RepairMerchantCart", "EscortMerchant", "InvestigateAttacks", "DestroyBanditCamp", "DefendCaravan" },
                FinalReward = new QuestReward
                {
                    Gold = 200,
                    ReputationChanges = new() { { FactionType.Merchants, 20 } },
                    UnlockedQuestLineIds = new() { "TradeExpansion" }
                }
            },
            new QuestLineDefinition
            {
                Id = "TradeExpansion",
                Title = "Trade Expansion",
                Description = "Expand trade networks and establish new merchant routes.",
                CanStart = world => true,
                PrerequisiteQuestLineIds = new() { "MerchantGuild" },
                QuestIds = new() { "SupplyDistrict", "SecureRoute", "BuildTradingPost" },
                FinalReward = new QuestReward
                {
                    Gold = 150,
                    ReputationChanges = new() { { FactionType.Merchants, 15 } }
                }
            },
            new QuestLineDefinition
            {
                Id = "ClericTraining",
                Title = "Clerical Vocation",
                Description = "Aid the temple in training a new generation of clerics and priests.",
                CanStart = world => true,
                PrerequisiteQuestLineIds = new(),
                QuestIds = new() { "CollectHolyRelics", "TrainNovices", "DefendTemple" },
                FinalReward = new QuestReward
                {
                    Gold = 120,
                    ReputationChanges = new() { { FactionType.Clergy, 15 } }
                }
            },
            new QuestLineDefinition
            {
                Id = "RangerRecruit",
                Title = "The Ranger's Path",
                Description = "Join the rangers and protect travelers from wilderness dangers.",
                CanStart = world => true,
                PrerequisiteQuestLineIds = new(),
                QuestIds = new() { "HuntWildbeasts", "PatrolTradRoutes", "EliminateBandits" },
                FinalReward = new QuestReward
                {
                    Gold = 140,
                    ReputationChanges = new() { { FactionType.Rangers, 18 } }
                }
            },
            new QuestLineDefinition
            {
                Id = "FarmingSupport",
                Title = "Harvest Moon",
                Description = "Support the local farmers and ensure a bountiful harvest.",
                CanStart = world => true,
                PrerequisiteQuestLineIds = new(),
                QuestIds = new() { "HelpWithHarvest", "RepairFarmEquipment", "ProtectFromWildlife" },
                FinalReward = new QuestReward
                {
                    Gold = 100,
                    ReputationChanges = new() { { FactionType.Merchants, 12 } }
                }
            }
        };
    }

    public static class SettlementRegistry
    {
        public static readonly List<SettlementDefinition> AllSettlements = new()
        {
            new SettlementDefinition
            {
                Id = "StoneHaven",
                Name = "Stone Haven",
                Type = "Town",
                Region = RegionId.NorthernRealm,
                PopulationStart = 45,
                WealthStart = 60,
                SafetyStart = 70,
                InitialBuildings = new() { "Tavern", "Blacksmith", "Market" },
                InitialQuestLineIds = new() { "MerchantGuild" },
                Properties = new() { { "HasMarketplace", true }, { "HasInn", true }, { "HasSmith", true } }
            },
            new SettlementDefinition
            {
                Id = "SilverStream",
                Name = "Silver Stream",
                Type = "Village",
                Region = RegionId.NorthernRealm,
                PopulationStart = 25,
                WealthStart = 40,
                SafetyStart = 50,
                InitialBuildings = new() { "Farm", "Mill" },
                InitialQuestLineIds = new() { "FarmingSupport" },
                Properties = new() { { "HasFarm", true }, { "HasMill", true } }
            },
            new SettlementDefinition
            {
                Id = "CrossRoads",
                Name = "Crossroads",
                Type = "Village",
                Region = RegionId.NorthernRealm,
                PopulationStart = 35,
                WealthStart = 50,
                SafetyStart = 45,
                InitialBuildings = new() { "Inn", "Stables" },
                InitialQuestLineIds = new() { "RangerRecruit" },
                Properties = new() { { "HasInn", true }, { "HasStables", true } }
            },
            new SettlementDefinition
            {
                Id = "HolyTemple",
                Name = "Holy Temple",
                Type = "Monastery",
                Region = RegionId.NorthernRealm,
                PopulationStart = 20,
                WealthStart = 55,
                SafetyStart = 80,
                InitialBuildings = new() { "Temple", "Library" },
                InitialQuestLineIds = new() { "ClericTraining" },
                Properties = new() { { "HasTemple", true }, { "HasLibrary", true } }
            }
        };
    }
}
