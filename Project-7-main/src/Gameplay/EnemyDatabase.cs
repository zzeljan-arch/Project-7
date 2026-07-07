using System.Collections.Generic;
using PG.World.Simulation;

namespace PG.Gameplay.Combat
{
    public class EnemyDatabase
    {
        private readonly Dictionary<string, EnemyArchetype> _archetypes;

        public EnemyDatabase()
        {
            _archetypes = new Dictionary<string, EnemyArchetype>();
            LoadArchetypes();
        }

        private void LoadArchetypes()
        {
            var bandit = new EnemyArchetype
            {
                ArchetypeName = "Bandit",
                HomeRegion = RegionId.NorthernRealm,
                AssociatedPath = EvolutionPath.AgeOfRaiders,
                TierProgression = new Dictionary<WorldTier, EnemyTierData>
                {
                    [WorldTier.Tier1] = new EnemyTierData { BaseHealth = 100, BaseDamage = 18, Armor = 5, MovementSpeed = 4.2f, AbilityIds = new List<string> { "slash" }, LootTableIds = new List<string> { "age_of_raiders" }, LootRarity = ItemRarityType.Common },
                    [WorldTier.Tier2] = new EnemyTierData { BaseHealth = 150, BaseDamage = 25, Armor = 8, MovementSpeed = 4.4f, AbilityIds = new List<string> { "slash", "dash_attack" }, LootTableIds = new List<string> { "age_of_raiders" }, LootRarity = ItemRarityType.Uncommon }
                }
            };

            var frostWolf = new EnemyArchetype
            {
                ArchetypeName = "Frost Wolf",
                HomeRegion = RegionId.NorthernRealm,
                AssociatedPath = EvolutionPath.EternalWinter,
                TierProgression = new Dictionary<WorldTier, EnemyTierData>
                {
                    [WorldTier.Tier1] = new EnemyTierData { BaseHealth = 120, BaseDamage = 20, Armor = 3, MovementSpeed = 5.0f, AbilityIds = new List<string> { "bite" }, LootTableIds = new List<string> { "eternal_winter" }, LootRarity = ItemRarityType.Common },
                    [WorldTier.Tier2] = new EnemyTierData { BaseHealth = 170, BaseDamage = 28, Armor = 6, MovementSpeed = 5.2f, AbilityIds = new List<string> { "bite", "frost_aura" }, LootTableIds = new List<string> { "eternal_winter" }, LootRarity = ItemRarityType.Uncommon }
                }
            };

            var sandSlasher = new EnemyArchetype
            {
                ArchetypeName = "Sand Slasher",
                HomeRegion = RegionId.DesertKingdom,
                AssociatedPath = EvolutionPath.AgeOfMerchants,
                TierProgression = new Dictionary<WorldTier, EnemyTierData>
                {
                    [WorldTier.Tier1] = new EnemyTierData { BaseHealth = 110, BaseDamage = 22, Armor = 4, MovementSpeed = 4.8f, AbilityIds = new List<string> { "slash" }, LootTableIds = new List<string> { "age_of_merchants" }, LootRarity = ItemRarityType.Common },
                    [WorldTier.Tier2] = new EnemyTierData { BaseHealth = 160, BaseDamage = 30, Armor = 7, MovementSpeed = 5.0f, AbilityIds = new List<string> { "slash", "spin_attack" }, LootTableIds = new List<string> { "age_of_merchants" }, LootRarity = ItemRarityType.Uncommon }
                }
            };

            _archetypes[bandit.ArchetypeName] = bandit;
            _archetypes[frostWolf.ArchetypeName] = frostWolf;
            _archetypes[sandSlasher.ArchetypeName] = sandSlasher;
        }

        public EnemyArchetype GetArchetypeForRegion(RegionId regionId, EvolutionPath path, WorldTier tier)
        {
            foreach (var archetype in _archetypes.Values)
            {
                if (archetype.HomeRegion == regionId && archetype.AssociatedPath == path)
                    return archetype;
            }

            foreach (var archetype in _archetypes.Values)
            {
                if (archetype.HomeRegion == regionId)
                    return archetype;
            }

            return null;
        }
    }
}
