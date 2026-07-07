using System;
using System.Collections.Generic;

namespace PG.Gameplay.Loot
{
    /// <summary>
    /// Defines a legendary item and its path-specific variants.
    /// In Godot: Export as a Resource with nested variant Resources.
    /// Example setup:
    ///   res://data/legendaries/Dominion.tres (LegendaryDefinition Resource)
    ///     └─ Variants (Dictionary)
    ///        ├─ AgeOfRaiders → "Conqueror's Dominion" (LegendaryVariant)
    ///        ├─ EternalWinter → "Frostbrand Dominion" (LegendaryVariant)
    ///        └─ ... (one for each path it appears in)
    /// </summary>
    public class LegendaryDefinition
    {
        /// <summary>
        /// Unique identifier (e.g., "dominion", "frostbite").
        /// </summary>
        public string LegendaryId { get; set; }

        /// <summary>
        /// Base name visible to players (e.g., "Dominion").
        /// </summary>
        public string BaseName { get; set; }

        /// <summary>
        /// Item type (Weapon, Armor, Accessory).
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// Flavor text describing the legendary (lore).
        /// </summary>
        public string BaseDescription { get; set; }

        /// <summary>
        /// Target classes for this item (e.g., Warrior, Ranger).
        /// Empty = usable by all classes.
        /// </summary>
        public List<string> TargetClasses { get; set; }

        /// <summary>
        /// Variants of this legendary per evolution path.
        /// GODOT: Dictionary<EvolutionPath, LegendaryVariant>
        /// If a path isn't in this dictionary, this legendary doesn't appear in that path.
        /// </summary>
        public Dictionary<PG.World.Simulation.EvolutionPath, LegendaryVariant> Variants { get; set; }

        public LegendaryDefinition()
        {
            TargetClasses = new List<string>();
            Variants = new Dictionary<PG.World.Simulation.EvolutionPath, LegendaryVariant>();
        }
    }

    /// <summary>
    /// Path-specific variant of a legendary item.
    /// Exported to Godot as a nested Resource within LegendaryDefinition.
    /// </summary>
    public class LegendaryVariant
    {
        /// <summary>
        /// Unique name for this variant (e.g., "Frostbrand Dominion").
        /// </summary>
        public string VariantName { get; set; }

        /// <summary>
        /// Path this variant belongs to.
        /// </summary>
        public PG.World.Simulation.EvolutionPath EvolutionPath { get; set; }

        /// <summary>
        /// Flavor text specific to this variant (why it's different).
        /// </summary>
        public string FlavorText { get; set; }

        /// <summary>
        /// Affix IDs this variant has (e.g., "freeze_on_hit", "divine_strike").
        /// GODOT: Array of strings referencing AffinityDefinition Resources.
        /// </summary>
        public List<string> AffixIds { get; set; }

        /// <summary>
        /// Stat multiplier for base stats (1.0 = no change, 1.2 = +20%).
        /// </summary>
        public float StatMultiplier { get; set; }

        /// <summary>
        /// Glow/visual color for this variant in the world.
        /// GODOT: Color or hex string (e.g., "#FF6600" for orange glow).
        /// </summary>
        public string GlowColor { get; set; }

        /// <summary>
        /// Special visual effect name (e.g., "frostbrand_trail", "divine_aura").
        /// References a particle system or shader.
        /// </summary>
        public string VisualEffect { get; set; }

        /// <summary>
        /// Minimum tier this legendary can drop at (usually Tier 3+).
        /// </summary>
        public PG.World.Simulation.WorldTier MinimumDropTier { get; set; }

        /// <summary>
        /// Unique passive ability this legendary grants (if any).
        /// Example: "When you deal damage, nearby allies gain +5% damage for 5 seconds"
        /// GODOT: String name referencing an AbilityDefinition or null.
        /// </summary>
        public string UniqueAbilityId { get; set; }

        public LegendaryVariant()
        {
            AffixIds = new List<string>();
        }
    }

    /// <summary>
    /// Item types supported.
    /// </summary>
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Consumable,
        Quest
    }

    /// <summary>
    /// Represents a single affix (power) that can appear on items.
    /// Exported to Godot as a Resource.
    /// Example: res://data/affixes/freeze_on_hit.tres
    /// </summary>
    public class AffixDefinition
    {
        /// <summary>
        /// Unique ID (e.g., "freeze_on_hit").
        /// </summary>
        public string AffixId { get; set; }

        /// <summary>
        /// Display name (e.g., "Freeze on Hit").
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description for tooltips.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Which item types can have this affix (Weapon, Armor, etc.).
        /// </summary>
        public List<ItemType> ValidItemTypes { get; set; }

        /// <summary>
        /// Base numeric value for this affix (scales with tier/rarity).
        /// Example: 0.10 = 10% freeze chance.
        /// </summary>
        public float BaseValue { get; set; }

        /// <summary>
        /// How this affix scales (None, Linear, Exponential).
        /// Used during loot generation to adjust value.
        /// </summary>
        public ScalingType ScalingType { get; set; }

        /// <summary>
        /// Paths this affix is restricted to (null = appears in all paths).
        /// Example: "divine_strike" only on Ragnarök items.
        /// </summary>
        public List<PG.World.Simulation.EvolutionPath> RestrictedPaths { get; set; }

        /// <summary>
        /// Rarity requirement (affix only appears on this rarity or higher).
        /// </summary>
        public PG.Gameplay.Combat.ItemRarityType MinimumRarity { get; set; }

        public AffixDefinition()
        {
            ValidItemTypes = new List<ItemType>();
            RestrictedPaths = new List<PG.World.Simulation.EvolutionPath>();
        }
    }

    public enum ScalingType
    {
        None,           // Fixed value
        Linear,         // +10% per tier
        Exponential,    // +20% per tier
        Multiplicative  // Base * (1 + tier_bonus)
    }

    /// <summary>
    /// Loot table entry: references what item and how often it drops.
    /// Used by LootTable to build a weighted pool.
    /// </summary>
    public class LootEntry
    {
        /// <summary>
        /// ID of item to drop (legendary ID, template ID, or currency ID).
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Relative weight (e.g., 5 = 5x more likely than weight 1).
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Minimum tier required for this item to drop.
        /// </summary>
        public PG.World.Simulation.WorldTier MinimumTier { get; set; }

        /// <summary>
        /// How stats scale based on tier (multiplier per tier).
        /// Example: 1.3 = +30% per tier.
        /// </summary>
        public float TierScaling { get; set; }

        /// <summary>
        /// Max quantity that can drop in a single instance.
        /// </summary>
        public int MaxQuantity { get; set; }
    }

    /// <summary>
    /// Loot table for a specific region + path combination.
    /// In Godot: Export as a Resource or load from CSV.
    /// Example: res://data/loot/northern_realm/age_of_raiders_loot.tres
    /// </summary>
    public class LootTable
    {
        /// <summary>
        /// Region this loot table applies to.
        /// </summary>
        public PG.World.Simulation.RegionId Region { get; set; }

        /// <summary>
        /// Evolution path this loot table applies to.
        /// </summary>
        public PG.World.Simulation.EvolutionPath Path { get; set; }

        /// <summary>
        /// Items that drop guaranteed (always in loot).
        /// Example: Currency, vendor items.
        /// </summary>
        public List<LootEntry> GuaranteedDrops { get; set; }

        /// <summary>
        /// Rare items that drop 25% of the time.
        /// </summary>
        public List<LootEntry> RareDrops { get; set; }

        /// <summary>
        /// Legendary items that drop 2-5% of the time.
        /// </summary>
        public List<LootEntry> LegendaryDrops { get; set; }

        /// <summary>
        /// Drop rate multiplier for this path (1.0 = normal, 1.2 = 20% more loot).
        /// </summary>
        public float LootMultiplier { get; set; }

        public LootTable()
        {
            GuaranteedDrops = new List<LootEntry>();
            RareDrops = new List<LootEntry>();
            LegendaryDrops = new List<LootEntry>();
            LootMultiplier = 1.0f;
        }
    }

    /// <summary>
    /// Data for transmog (reshaping legendary stats).
    /// In Godot: Managed as a configuration or recipe Resource.
    /// </summary>
    public class TransmogRecipe
    {
        /// <summary>
        /// Cost to reroll affixes (materials needed).
        /// GODOT: Dictionary<string, int> where key is material ID, value is quantity.
        /// </summary>
        public Dictionary<string, int> MaterialCost { get; set; }

        /// <summary>
        /// Cost in currency (gold/resources).
        /// </summary>
        public int CurrencyCost { get; set; }

        /// <summary>
        /// Which stats can be rerolled (e.g., "damage", "health", "crit").
        /// </summary>
        public List<string> RollableStats { get; set; }

        /// <summary>
        /// Number of times a player can reroll per item (or -1 for unlimited).
        /// </summary>
        public int RerollsAllowed { get; set; }

        public TransmogRecipe()
        {
            MaterialCost = new Dictionary<string, int>();
            RollableStats = new List<string>();
        }
    }
}
