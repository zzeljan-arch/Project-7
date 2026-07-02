using System.Collections.Generic;
using PG.Systems;
using PG.World.Simulation;

namespace PG.Gameplay.Combat
{
    public class LootDatabase
    {
        private readonly Dictionary<string, LootTable> _lootTables;

        public LootDatabase()
        {
            _lootTables = new Dictionary<string, LootTable>();
            LoadLootTables();
        }

        private void LoadLootTables()
        {
            var raidersTable = new LootTable
            {
                LootTableId = "age_of_raiders",
                GuaranteedDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "bandit_coin", Weight = 100, MinimumTier = WorldTier.Tier1 }
                },
                RareDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "raider_sabre", Weight = 10, MinimumTier = WorldTier.Tier2 }
                },
                LegendaryDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "raider_blessing", Weight = 1, MinimumTier = WorldTier.Tier2 }
                }
            };

            var winterTable = new LootTable
            {
                LootTableId = "eternal_winter",
                GuaranteedDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "frost_fang", Weight = 100, MinimumTier = WorldTier.Tier1 }
                },
                RareDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "ice_shard", Weight = 10, MinimumTier = WorldTier.Tier2 }
                },
                LegendaryDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "glacier_core", Weight = 1, MinimumTier = WorldTier.Tier2 }
                }
            };

            var merchantTable = new LootTable
            {
                LootTableId = "age_of_merchants",
                GuaranteedDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "trade_token", Weight = 100, MinimumTier = WorldTier.Tier1 }
                },
                RareDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "merchant_gold", Weight = 10, MinimumTier = WorldTier.Tier2 }
                },
                LegendaryDrops = new List<LootEntry>
                {
                    new LootEntry { ItemId = "merchant_seal", Weight = 1, MinimumTier = WorldTier.Tier2 }
                }
            };

            _lootTables[raidersTable.LootTableId] = raidersTable;
            _lootTables[winterTable.LootTableId] = winterTable;
            _lootTables[merchantTable.LootTableId] = merchantTable;
        }

        public List<ItemInstance> GenerateLoot(RegionState region, EnemyArchetype enemy, WorldTier tier)
        {
            var result = new List<ItemInstance>();
            if (!enemy.TierProgression.TryGetValue(tier, out var tierData))
                return result;

            var lootTableId = tierData.LootTableIds.Count > 0 ? tierData.LootTableIds[0] : string.Empty;
            if (string.IsNullOrEmpty(lootTableId) || !_lootTables.TryGetValue(lootTableId, out var table))
                return result;

            var rng = new DeterministicRandom(GetStableSeed(region, enemy, tier));

            foreach (var entry in table.GuaranteedDrops)
            {
                if (entry.MinimumTier <= tier)
                {
                    result.Add(new ItemInstance { ItemId = entry.ItemId, DisplayName = entry.ItemId, Rarity = ItemRarityType.Common, Value = 10 });
                }
            }

            if (table.RareDrops.Count > 0 && rng.Probability(0.25))
            {
                var rare = table.RareDrops[0];
                result.Add(new ItemInstance { ItemId = rare.ItemId, DisplayName = rare.ItemId, Rarity = ItemRarityType.Uncommon, Value = 40 });
            }

            if (table.LegendaryDrops.Count > 0 && rng.Probability(0.05))
            {
                var legendary = table.LegendaryDrops[0];
                result.Add(new ItemInstance { ItemId = legendary.ItemId, DisplayName = legendary.ItemId, Rarity = ItemRarityType.Rare, Value = 90 });
            }

            return result;
        }

        private static ulong GetStableSeed(RegionState region, EnemyArchetype enemy, WorldTier tier)
        {
            var hash = (ulong)((int)region.RegionId * 73856093 ^ (int)region.CurrentPath * 19349663 ^ (int)tier * 83492791);
            foreach (var c in enemy.ArchetypeName)
            {
                hash = (hash ^ (ulong)c) * 1099511628211ul;
            }

            return hash;
        }
    }

    public class LootTable
    {
        public string LootTableId { get; set; }
        public List<LootEntry> GuaranteedDrops { get; set; }
        public List<LootEntry> RareDrops { get; set; }
        public List<LootEntry> LegendaryDrops { get; set; }

        public LootTable()
        {
            GuaranteedDrops = new List<LootEntry>();
            RareDrops = new List<LootEntry>();
            LegendaryDrops = new List<LootEntry>();
        }
    }

    public class LootEntry
    {
        public string ItemId { get; set; }
        public int Weight { get; set; }
        public WorldTier MinimumTier { get; set; }
    }
}
