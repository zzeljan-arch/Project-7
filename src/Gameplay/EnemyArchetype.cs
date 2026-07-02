using System;
using System.Collections.Generic;

namespace PG.Gameplay.Combat
{
    /// <summary>
    /// Defines how an enemy archetype evolves across tiers.
    /// In Godot: Export this as a Resource with nested ScriptableObjects for each tier.
    /// Example Godot setup:
    ///   res://data/enemies/archetypes/Bandit.tres (EnemyArchetype Resource)
    ///     └─ TierProgression (Dictionary)
    ///        ├─ Tier1Data (EnemyTierData Resource)
    ///        ├─ Tier2Data (EnemyTierData Resource)
    ///        └─ ... (continues to Tier5)
    /// </summary>
    public class EnemyArchetype
    {
        /// <summary>
        /// Unique archetype name (e.g., "Bandit", "Frost Wolf").
        /// </summary>
        public string ArchetypeName { get; set; }

        /// <summary>
        /// Home region where this enemy primarily appears.
        /// </summary>
        public PG.World.Simulation.RegionId HomeRegion { get; set; }

        /// <summary>
        /// Evolution path this enemy is associated with.
        /// Null if enemy appears in multiple paths.
        /// </summary>
        public PG.World.Simulation.EvolutionPath? AssociatedPath { get; set; }

        /// <summary>
        /// Tier-specific data: Map WorldTier to enemy stats/abilities.
        /// GODOT: Dictionary<WorldTier, EnemyTierData>
        /// </summary>
        public Dictionary<PG.World.Simulation.WorldTier, EnemyTierData> TierProgression { get; set; }

        /// <summary>
        /// XP reward for defeating this enemy (scales with tier).
        /// </summary>
        public int BaseXPReward { get; set; }

        /// <summary>
        /// Visual prefab or mesh to use (relative path or asset ID).
        /// GODOT: Can be a relative path like "res://models/enemies/bandit.tscn"
        /// </summary>
        public string PrefabPath { get; set; }

        public EnemyArchetype()
        {
            TierProgression = new Dictionary<PG.World.Simulation.WorldTier, EnemyTierData>();
        }
    }

    /// <summary>
    /// Data for a single tier of an enemy archetype.
    /// Exported to Godot as a nested Resource.
    /// </summary>
    public class EnemyTierData
    {
        /// <summary>
        /// Base health for this tier. Scales linearly; next tier = +40-50%.
        /// </summary>
        public float BaseHealth { get; set; }

        /// <summary>
        /// Base damage per hit.
        /// </summary>
        public float BaseDamage { get; set; }

        /// <summary>
        /// Armor/damage reduction (0-100, where 100 = immunity).
        /// </summary>
        public float Armor { get; set; }

        /// <summary>
        /// Movement speed multiplier (1.0 = base speed).
        /// </summary>
        public float MovementSpeed { get; set; }

        /// <summary>
        /// List of ability names this enemy can use.
        /// Reference to ability definitions (e.g., "ability/slash", "ability/dash_attack").
        /// GODOT: Array of strings or references to AbilityDefinition Resources.
        /// </summary>
        public List<string> AbilityIds { get; set; }

        /// <summary>
        /// Rarity of loot this enemy drops.
        /// </summary>
        public ItemRarityType LootRarity { get; set; }

        /// <summary>
        /// Loot table IDs for what this enemy can drop.
        /// References to LootEntry or LootTable data.
        /// GODOT: Array of strings (loot table IDs).
        /// </summary>
        public List<string> LootTableIds { get; set; }

        /// <summary>
        /// Visual variant for this tier (e.g., "bandit_leather" for T1, "bandit_steel" for T3).
        /// Affects appearance, armor, glow effects, size.
        /// GODOT: Could reference a Material, a submesh, or a visual state name.
        /// </summary>
        public string VisualVariant { get; set; }

        /// <summary>
        /// Tags for classification (e.g., "humanoid", "cursed", "flying").
        /// Used for loot generation and ability interaction.
        /// </summary>
        public List<string> Tags { get; set; }

        public EnemyTierData()
        {
            AbilityIds = new List<string>();
            LootTableIds = new List<string>();
            Tags = new List<string>();
        }
    }

    /// <summary>
    /// Rarity tiers for loot.
    /// </summary>
    public enum ItemRarityType
    {
        Common,      // 60% drop rate, +0% stats
        Uncommon,    // 25% drop rate, +15% stats
        Rare,        // 10% drop rate, +30% stats
        Epic,        // 4% drop rate, +50% stats
        Legendary    // 1% drop rate, +100% stats
    }

    /// <summary>
    /// Helper to get stat scaling multiplier based on rarity.
    /// </summary>
    public static class RarityExtensions
    {
        public static float GetStatMultiplier(this ItemRarityType rarity)
        {
            return rarity switch
            {
                ItemRarityType.Common => 1.0f,
                ItemRarityType.Uncommon => 1.15f,
                ItemRarityType.Rare => 1.30f,
                ItemRarityType.Epic => 1.50f,
                ItemRarityType.Legendary => 2.0f,
                _ => 1.0f
            };
        }

        public static float GetDropRate(this ItemRarityType rarity)
        {
            return rarity switch
            {
                ItemRarityType.Common => 0.60f,
                ItemRarityType.Uncommon => 0.25f,
                ItemRarityType.Rare => 0.10f,
                ItemRarityType.Epic => 0.04f,
                ItemRarityType.Legendary => 0.01f,
                _ => 0.0f
            };
        }
    }
}
