using System;
using System.Collections.Generic;
using System.Linq;

namespace PG.World.Simulation
{
    /// <summary>Master list of all tradeable items in the game</summary>
    public static class ItemRegistry
    {
        public static readonly List<ItemDefinition> AllItems = new()
        {
            new ItemDefinition { Id = "IronOre", Name = "Iron Ore", Category = "Material", BaseValue = 8 },
            new ItemDefinition { Id = "IronIngot", Name = "Iron Ingot", Category = "Material", BaseValue = 22 },
            new ItemDefinition { Id = "Herb", Name = "Healing Herb", Category = "Consumable", BaseValue = 5 },
            new ItemDefinition { Id = "FreshFish", Name = "Fresh Fish", Category = "Food", BaseValue = 4 },
            new ItemDefinition { Id = "AncientRelic", Name = "Ancient Relic", Category = "Artifact", BaseValue = 60 },
            new ItemDefinition { Id = "RumorFragment", Name = "Rumor Fragment", Category = "Story", BaseValue = 0 },
            new ItemDefinition { Id = "KnightsOathRelic", Name = "Knight's Oath", Category = "Legendary", BaseValue = 250 },
            new ItemDefinition { Id = "GiantTear", Name = "Giant's Tear", Category = "Legendary", BaseValue = 180 },
            new ItemDefinition { Id = "WhiteWolfPelt", Name = "White Wolf Pelt", Category = "Legendary", BaseValue = 120 },
            new ItemDefinition { Id = "DragonBone", Name = "Dragon Bone", Category = "Collector", BaseValue = 140 },
            new ItemDefinition { Id = "Runestone", Name = "Runestone", Category = "Collector", BaseValue = 95 },
            new ItemDefinition { Id = "AncientBook", Name = "Ancient Book", Category = "Collector", BaseValue = 80 },
            new ItemDefinition { Id = "LostSongScroll", Name = "Lost Song Scroll", Category = "Collector", BaseValue = 85 },
            new ItemDefinition { Id = "ReligiousRelic", Name = "Religious Relic", Category = "Collector", BaseValue = 100 }
        };

        public static ItemDefinition GetItem(string id) => AllItems.First(item => item.Id == id);
    }

    /// <summary>Master list of all factions in the game</summary>
    public static class FactionRegistry
    {
        public static readonly List<FactionState> AllFactions = new()
        {
            new FactionState { Type = FactionType.Merchants, Name = "Merchant Guild", Influence = 50, Reputation = 0, HasCamp = false },
            new FactionState { Type = FactionType.Rangers, Name = "Forest Rangers", Influence = 30, Reputation = 0, HasCamp = true },
            new FactionState { Type = FactionType.Bandits, Name = "Bandit Clan", Influence = 25, Reputation = -10, HasCamp = true },
            new FactionState { Type = FactionType.Mages, Name = "Arcane Enclave", Influence = 35, Reputation = 0, HasCamp = false },
            new FactionState { Type = FactionType.Miners, Name = "Mining Consortium", Influence = 28, Reputation = 0, HasCamp = false }
        };
    }
}
