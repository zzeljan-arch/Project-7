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
            },

            // ===== NORTHERN EVOLUTION QUESTS =====
            new QuestDefinition
            {
                Id = "RelightNorthernBraziers",
                Title = "Relight the Northern Braziers",
                Description = "Restore sacred braziers to hold back the deep frost.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 45, ReputationChanges = new() { { FactionType.Clergy, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 7 } },
                ZoneEvolutionEffects = new() { ("EternalWinter", -10), ("DivineOrder", 6) }
            },
            new QuestDefinition
            {
                Id = "SealTheBarrows",
                Title = "Seal the Restless Barrows",
                Description = "Perform rites at old graves to suppress undead resurgence.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 60, ReputationChanges = new() { { FactionType.Rangers, 3 }, { FactionType.Clergy, 3 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "RangerActivity", Amount = 6 } },
                ZoneEvolutionEffects = new() { ("UndeadPower", -12), ("DivineOrder", 3) }
            },
            new QuestDefinition
            {
                Id = "WinterPact",
                Title = "The Winter Pact",
                Description = "Negotiate with frost spirits for temporary protection.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 50, ReputationChanges = new() { { FactionType.Mages, 4 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 4 } },
                ZoneEvolutionEffects = new() { ("EternalWinter", 8) }
            },

            // ===== MERCHANT REPUBLIC QUESTS =====
            new QuestDefinition
            {
                Id = "DesertTradeAccord",
                Title = "Desert Trade Accord",
                Description = "Broker a new inter-regional trade pact for caravan houses.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 70, ReputationChanges = new() { { FactionType.Merchants, 7 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 9, TargetRegion = RegionId.DesertKingdom } },
                ZoneEvolutionEffects = new() { ("MerchantsRepublic", 9) },
                HiddenBarEffects = new() { ("KingsStability", 2.0), ("DawnOfGods", 1.0) }
            },

            // ===== HIDDEN STORY QUESTS =====
            new QuestDefinition
            {
                Id = "InvestigateRumors",
                Title = "Echoes Behind the Rumors",
                Description = "Assemble rumor shards and investigate what is truly happening in the north.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 90, ReputationChanges = new() { { FactionType.Mages, 5 }, { FactionType.Clergy, 5 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 4.0), ("TheVeil", 1.0) }
            },
            new QuestDefinition
            {
                Id = "SevenRelics",
                Title = "Seven Relics",
                Description = "Gather seven ancient relics before they vanish from history forever.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 160, ReputationChanges = new() { { FactionType.Mages, 8 } }, Items = new() { new ItemStack(ItemRegistry.GetItem("AncientRelic"), 1) } },
                HiddenBarEffects = new() { ("DawnOfGods", 6.0), ("OldGods", 4.0) }
            },
            new QuestDefinition
            {
                Id = "CollectorsArchive",
                Title = "The Collector's Archive",
                Description = "Compile Ancient Books, Lost Songs, Runestones, Dragon Bones, Hero Graves records, Legendary Weapons, Masks, Crowns, Religious Relics, Banners, and Monster Trophies.",
                CanStart = world => true,
                RequiredEncounterIds = new(),
                Reward = new QuestReward { Gold = 200, ReputationChanges = new() { { FactionType.Merchants, 4 }, { FactionType.Mages, 4 } } },
                HiddenBarEffects = new() { ("OldGods", 3.0), ("DawnOfGods", 2.0), ("DragonAwakeningHidden", 1.5) }
            },
            new QuestDefinition
            {
                Id = "DragonBonePilgrimage",
                Title = "Dragon Bone Pilgrimage",
                Description = "Follow the mountain lights to a field of ancient dragon remains.",
                CanStart = world => true,
                Reward = new QuestReward { Gold = 95, ReputationChanges = new() { { FactionType.Mages, 5 } }, Items = new() { new ItemStack(ItemRegistry.GetItem("DragonBone"), 1) } },
                EventBarEffects = new() { new QuestEffect { BarId = "RangerActivity", Amount = 6 } },
                ZoneEvolutionEffects = new() { ("DawnOfGodsHidden", 8), ("EternalWinter", 2) },
                HiddenBarEffects = new() { ("DragonAwakeningHidden", 8.0), ("DawnOfGods", 3.0) }
            },
            new QuestDefinition
            {
                Id = "SilentFlockInquiry",
                Title = "The Silent Flock",
                Description = "Investigate why birds avoid the roads near Stone Haven.",
                CanStart = world => true,
                Reward = new QuestReward { Gold = 80, ReputationChanges = new() { { FactionType.Rangers, 4 }, { FactionType.Clergy, 2 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "WildlifeEncroach", Amount = -10 } },
                ZoneEvolutionEffects = new() { ("UndeadPower", 3), ("DawnOfGodsHidden", 5) },
                HiddenBarEffects = new() { ("TheVeil", 7.0), ("AncientEvil", 3.0) }
            },
            new QuestDefinition
            {
                Id = "RiverDoubleCrossInvestigation",
                Title = "The River Double-Cross",
                Description = "Track the impossible merchant route and uncover the crown's hidden courier network.",
                CanStart = world => true,
                Reward = new QuestReward { Gold = 85, ReputationChanges = new() { { FactionType.Merchants, 5 } } },
                EventBarEffects = new() { new QuestEffect { BarId = "MerchantCaravan", Amount = 8, TargetRegion = RegionId.DesertKingdom } },
                ZoneEvolutionEffects = new() { ("MerchantsRepublic", 5), ("DawnOfGodsHidden", 4) },
                HiddenBarEffects = new() { ("KingsStability", 7.0), ("DawnOfGods", 2.0) }
            },
            new QuestDefinition
            {
                Id = "AgelessPriestInquiry",
                Title = "The Ageless Priest",
                Description = "Unravel whether the priest's longevity is miracle, curse, or cultic bargain.",
                CanStart = world => true,
                Reward = new QuestReward { Gold = 90, ReputationChanges = new() { { FactionType.Clergy, 5 }, { FactionType.Mages, 3 } }, Items = new() { new ItemStack(ItemRegistry.GetItem("ReligiousRelic"), 1) } },
                EventBarEffects = new() { new QuestEffect { BarId = "TempleActivity", Amount = 9 } },
                ZoneEvolutionEffects = new() { ("DivineOrder", 7), ("DawnOfGodsHidden", 7) },
                HiddenBarEffects = new() { ("OldGods", 8.0), ("DawnOfGods", 4.0) }
            },

            // ===== DAWN OF THE GODS MYTHIC QUESTS =====
            new QuestDefinition
            {
                Id = "RestoreForgottenShrine",
                Title = "Restore the Forgotten Shrine",
                Description = "Recover scattered runestones and restore an ancient shrine with full rites.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 95, ReputationChanges = new() { { FactionType.Clergy, 6 }, { FactionType.Rangers, 3 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 1.0) }
            },
            new QuestDefinition
            {
                Id = "LastSkald",
                Title = "The Last Skald",
                Description = "Find the surviving skald and recover forgotten verses of the old world.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 120, ReputationChanges = new() { { FactionType.Clergy, 5 }, { FactionType.Rangers, 5 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 0.8) }
            },
            new QuestDefinition
            {
                Id = "HonorTheFallen",
                Title = "Honor the Fallen",
                Description = "Restore forgotten burial sites without looting the dead.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 70, ReputationChanges = new() { { FactionType.Rangers, 6 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 0.2) }
            },
            new QuestDefinition
            {
                Id = "WorldTreeSapling",
                Title = "The World Tree Sapling",
                Description = "Protect and transplant a young sapling believed tied to Yggdrasil.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 140, ReputationChanges = new() { { FactionType.Clergy, 8 }, { FactionType.Rangers, 8 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 1.0) }
            },
            new QuestDefinition
            {
                Id = "ForgeOfTheDwarves",
                Title = "Forge of the Dwarves",
                Description = "Restore an ancient dwarven forge and rekindle forgotten craft rites.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 160, ReputationChanges = new() { { FactionType.Merchants, 4 }, { FactionType.Rangers, 6 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 1.0) }
            },
            new QuestDefinition
            {
                Id = "FindBrokenHarp",
                Title = "Find the Broken Harp",
                Description = "Search frozen ruins for the harp tied to the song that ends winter.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 60 },
                HiddenBarEffects = new() { ("DawnOfGods", 0.2) }
            },
            new QuestDefinition
            {
                Id = "FindBlindSkald",
                Title = "Find the Blind Skald",
                Description = "Locate the blind skald who still remembers fragments of the true song.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 70, ReputationChanges = new() { { FactionType.Clergy, 3 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 0.2) }
            },
            new QuestDefinition
            {
                Id = "RecoverLostVerseI",
                Title = "Recover the First Lost Verse",
                Description = "Recover the first surviving verse from a weathered runestone.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 55 },
                HiddenBarEffects = new() { ("DawnOfGods", 0.2) }
            },
            new QuestDefinition
            {
                Id = "RecoverLostVerseII",
                Title = "Recover the Second Lost Verse",
                Description = "Track the second verse through skald fragments and shrine carvings.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 55 },
                HiddenBarEffects = new() { ("DawnOfGods", 0.2) }
            },
            new QuestDefinition
            {
                Id = "RecoverLostVerseIII",
                Title = "Recover the Third Lost Verse",
                Description = "Claim the final verse from a barrow protected by oath-bound spirits.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 65 },
                HiddenBarEffects = new() { ("DawnOfGods", 0.5) }
            },
            new QuestDefinition
            {
                Id = "ClimbFrostpeak",
                Title = "Climb Frostpeak",
                Description = "Climb Frostpeak and prepare the ancient rite before the final song.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 90, ReputationChanges = new() { { FactionType.Rangers, 4 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 0.5) }
            },
            new QuestDefinition
            {
                Id = "SingTheAncientSong",
                Title = "Sing the Ancient Song",
                Description = "Perform the complete song atop Frostpeak and choose the world's memory.",
                CanStart = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Reward = new QuestReward { Gold = 150, ReputationChanges = new() { { FactionType.Clergy, 10 }, { FactionType.Rangers, 7 } } },
                HiddenBarEffects = new() { ("DawnOfGods", 1.0) }
            }
        };
    }
}
