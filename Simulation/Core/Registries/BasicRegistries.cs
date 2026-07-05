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
            new ItemDefinition { Id = "RumorFragment", Name = "Rumor Fragment", Category = "Story", BaseValue = 0 }
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
