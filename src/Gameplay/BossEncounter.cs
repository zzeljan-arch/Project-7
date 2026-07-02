using System;
using System.Collections.Generic;

namespace PG.Gameplay.Combat
{
    /// <summary>
    /// Defines a boss encounter for a specific region and evolution path.
    /// In Godot: Export as a Resource or load from structured data (JSON/CSV).
    /// Example: res://data/bosses/northern_realm/age_of_raiders_story_boss.tres
    /// </summary>
    public class BossEncounter
    {
        /// <summary>
        /// Unique boss ID (e.g., "warlord_jarl", "frost_king").
        /// </summary>
        public string BossId { get; set; }

        /// <summary>
        /// Display name (e.g., "Warlord Jarl").
        /// </summary>
        public string BossName { get; set; }

        /// <summary>
        /// Boss category.
        /// </summary>
        public BossType Type { get; set; }

        /// <summary>
        /// Region this boss appears in.
        /// </summary>
        public PG.World.Simulation.RegionId Region { get; set; }

        /// <summary>
        /// Evolution path this boss is associated with.
        /// </summary>
        public PG.World.Simulation.EvolutionPath EvolutionPath { get; set; }

        /// <summary>
        /// Minimum tier at which this boss appears.
        /// </summary>
        public PG.World.Simulation.WorldTier MinimumTier { get; set; }

        /// <summary>
        /// Lore/flavor text for this boss.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Base health (scales with tier).
        /// </summary>
        public float BaseHealth { get; set; }

        /// <summary>
        /// Base damage.
        /// </summary>
        public float BaseDamage { get; set; }

        /// <summary>
        /// Armor/damage reduction.
        /// </summary>
        public float Armor { get; set; }

        /// <summary>
        /// List of boss-exclusive abilities.
        /// GODOT: Array of ability IDs (e.g., "ability/cleave", "ability/warlord_fury").
        /// </summary>
        public List<string> AbilityIds { get; set; }

        /// <summary>
        /// Phases the boss goes through (e.g., 50% HP = phase 2).
        /// Null if single-phase boss.
        /// </summary>
        public List<BossPhase> Phases { get; set; }

        /// <summary>
        /// Guaranteed loot drops from defeating this boss.
        /// GODOT: Array of LootEntry Resources.
        /// </summary>
        public List<PG.Gameplay.Loot.LootEntry> GuaranteedLoot { get; set; }

        /// <summary>
        /// Legendary item this boss has a chance to drop (path-specific).
        /// </summary>
        public string LegendaryDropId { get; set; }

        /// <summary>
        /// Chance to drop the legendary (0.0-1.0).
        /// </summary>
        public float LegendaryDropChance { get; set; }

        /// <summary>
        /// Number of players recommended for this encounter.
        /// </summary>
        public int RecommendedPartySize { get; set; }

        /// <summary>
        /// Location where this boss spawns (level ID, coordinates, or named area).
        /// GODOT: Could be a scene path or coordinate string.
        /// </summary>
        public string SpawnLocation { get; set; }

        public BossEncounter()
        {
            AbilityIds = new List<string>();
            Phases = new List<BossPhase>();
            GuaranteedLoot = new List<PG.Gameplay.Loot.LootEntry>();
            LegendaryDropChance = 0.02f;  // Default 2%
            RecommendedPartySize = 1;
        }
    }

    /// <summary>
    /// Boss encounter type.
    /// </summary>
    public enum BossType
    {
        Story,      // Region progression boss (one per region)
        Optional,   // Hidden or optional boss (one per path)
        WorldBoss   // Raid-style encounter (Tier 4+)
    }

    /// <summary>
    /// Represents a phase transition in a multi-phase boss.
    /// Example: At 50% health, boss enters "Enraged" phase with new abilities.
    /// </summary>
    public class BossPhase
    {
        /// <summary>
        /// Health threshold (0.5 = at 50% health).
        /// </summary>
        public float HealthThreshold { get; set; }

        /// <summary>
        /// Name/description of this phase.
        /// </summary>
        public string PhaseName { get; set; }

        /// <summary>
        /// Additional abilities available in this phase.
        /// </summary>
        public List<string> PhaseAbilityIds { get; set; }

        /// <summary>
        /// Stat modifiers for this phase (e.g., 1.5 = +50% damage).
        /// </summary>
        public float DamageMultiplier { get; set; }

        public BossPhase()
        {
            PhaseAbilityIds = new List<string>();
            DamageMultiplier = 1.0f;
        }
    }

    /// <summary>
    /// Defines a single ability a boss or enemy can use.
    /// In Godot: Export as a Resource.
    /// Example: res://data/abilities/cleave.tres
    /// </summary>
    public class AbilityDefinition
    {
        /// <summary>
        /// Unique ability ID (e.g., "cleave", "fireball", "heal").
        /// </summary>
        public string AbilityId { get; set; }

        /// <summary>
        /// Display name (e.g., "Cleave").
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description/tooltip.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ability type (damage, crowd_control, heal, buff, debuff).
        /// </summary>
        public AbilityType Type { get; set; }

        /// <summary>
        /// Cooldown in seconds.
        /// </summary>
        public float Cooldown { get; set; }

        /// <summary>
        /// Base damage/effect value (scales with creature stats).
        /// </summary>
        public float BaseValue { get; set; }

        /// <summary>
        /// How this ability scales with the creature's stats.
        /// Example: "strength" = scales with Strength attribute.
        /// </summary>
        public string ScalingStat { get; set; }

        /// <summary>
        /// Animation name to play when ability is used.
        /// GODOT: Could reference an AnimationPlayer animation name.
        /// </summary>
        public string AnimationName { get; set; }

        /// <summary>
        /// Visual effect to spawn (particle system, spell effect).
        /// </summary>
        public string VisualEffectId { get; set; }

        public AbilityDefinition() { }
    }

    public enum AbilityType
    {
        Damage,
        CrowdControl,
        Heal,
        Buff,
        Debuff,
        Summon
    }
}
