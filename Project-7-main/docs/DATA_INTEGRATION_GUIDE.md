# Enemy, Loot & Boss System Integration Guide

**Status:** Prototype pointers - ready for Godot resource implementation

This guide shows where and how the enemy, loot, and boss systems integrate.

---

## File Organization in Godot

```
res://
├── data/
│   ├── enemies/
│   │   ├── archetypes/
│   │   │   ├── Bandit.tres (EnemyArchetype Resource)
│   │   │   ├── FrostWolf.tres
│   │   │   ├── IceGoblin.tres
│   │   │   └── ... (all enemy types)
│   │   └── abilities/
│   │       ├── slash.tres (AbilityDefinition)
│   │       ├── dash_attack.tres
│   │       └── ... (all abilities)
│   ├── loot/
│   │   ├── legendaries/
│   │   │   ├── Dominion.tres (LegendaryDefinition with variants)
│   │   │   ├── Frostbite.tres
│   │   │   └── ... (all legendaries)
│   │   ├── affixes/
│   │   │   ├── freeze_on_hit.tres (AffixDefinition)
│   │   │   ├── divine_strike.tres
│   │   │   └── ... (all affixes)
│   │   ├── tables/
│   │   │   ├── northern_realm_
│   │   │   │   ├── age_of_raiders.tres (LootTable)
│   │   │   │   ├── eternal_winter.tres
│   │   │   │   └── ... (4 paths per region)
│   │   │   └── ... (all 7 regions)
│   ├── bosses/
│   │   ├── northern_realm_
│   │   │   ├── age_of_raiders_story.tres (BossEncounter: Warlord Jarl)
│   │   │   ├── age_of_raiders_optional.tres (BossEncounter: Frost King)
│   │   │   └── ... (4 paths)
│   │   └── ... (all 7 regions)
│
├── src/
│   └── Gameplay/
│       ├── EnemyArchetype.cs       ✓ (defined)
│       ├── LegendarySystem.cs      ✓ (defined)
│       ├── BossEncounter.cs        ✓ (defined)
│       ├── EnemyDatabase.cs        ← Next: implement
│       ├── LootDatabase.cs         ← Next: implement
│       └── BossDatabase.cs         ← Next: implement
```

---

## Implementation Skeleton (Pseudocode)

### EnemyDatabase.cs
**Purpose:** Load and cache all enemy archetypes from Resources.

```csharp
namespace PG.Gameplay.Combat
{
    public class EnemyDatabase
    {
        private static EnemyDatabase _instance;
        private Dictionary<string, EnemyArchetype> _archetypes;

        public static EnemyDatabase Instance => _instance ??= new EnemyDatabase();

        public EnemyDatabase()
        {
            _archetypes = new Dictionary<string, EnemyArchetype>();
            LoadAllArchetypes();
        }

        private void LoadAllArchetypes()
        {
            // GODOT: Use ResourceLoader to load all .tres files from res://data/enemies/archetypes/
            // Pseudocode:
            var archetypePaths = new[]
            {
                "res://data/enemies/archetypes/Bandit.tres",
                "res://data/enemies/archetypes/FrostWolf.tres",
                "res://data/enemies/archetypes/IceGoblin.tres",
                // ... load all
            };

            foreach (var path in archetypePaths)
            {
                // GODOT: var resource = GD.Load<Resource>(path) as EnemyArchetype;
                // C# only: var resource = JsonConvert.DeserializeObject<EnemyArchetype>(File.ReadAllText(path));
                
                if (resource != null)
                    _archetypes[resource.ArchetypeName] = resource;
            }
        }

        /// <summary>
        /// Get an enemy archetype for spawning.
        /// </summary>
        public EnemyArchetype GetArchetype(
            PG.World.Simulation.RegionId region,
            PG.World.Simulation.EvolutionPath path,
            PG.World.Simulation.WorldTier tier)
        {
            // Logic: Query which enemies are in this region/path
            // Return a random one weighted by spawn rates
            // For now: simple lookup
            
            string enemyName = GetRandomEnemyForPath(path);  // "Bandit", "FrostWolf", etc.
            return _archetypes.ContainsKey(enemyName) ? _archetypes[enemyName] : null;
        }

        private string GetRandomEnemyForPath(PG.World.Simulation.EvolutionPath path)
        {
            // GODOT: Load a configuration file mapping paths to enemy rosters
            // For now, hardcoded example:
            
            return path switch
            {
                PG.World.Simulation.EvolutionPath.AgeOfRaiders => RandomChoice(new[] { "Bandit", "Wolf", "IceGoblin", "Hunter" }),
                PG.World.Simulation.EvolutionPath.EternalWinter => RandomChoice(new[] { "FrostSprite", "IceWalker", "Mammoth" }),
                // ... etc for all 28 paths
                _ => "Bandit"  // fallback
            };
        }

        private T RandomChoice<T>(T[] options)
        {
            // GODOT: Use Godot's RandomNumberGenerator or local Random
            return options[GD.Randi() % (uint)options.Length];
        }
    }
}
```

---

### LootDatabase.cs
**Purpose:** Manage loot tables for all region/path combinations.

```csharp
namespace PG.Gameplay.Loot
{
    public class LootDatabase
    {
        private static LootDatabase _instance;
        private Dictionary<(RegionId, EvolutionPath), LootTable> _lootTables;
        private Dictionary<string, LegendaryDefinition> _legendaries;

        public static LootDatabase Instance => _instance ??= new LootDatabase();

        public LootDatabase()
        {
            _lootTables = new Dictionary<(RegionId, EvolutionPath), LootTable>();
            _legendaries = new Dictionary<string, LegendaryDefinition>();
            LoadAllLootData();
        }

        private void LoadAllLootData()
        {
            // GODOT: Load all LootTable Resources from res://data/loot/tables/
            // For each region and path combination (7 × 4 = 28 tables)
            
            for (int r = 0; r < 7; r++)
            {
                var regionId = (RegionId)r;
                for (int p = 0; p < 4; p++)
                {
                    var pathId = (EvolutionPath)(r * 4 + p);
                    string path = $"res://data/loot/tables/{regionId}/{pathId}.tres";
                    
                    // GODOT: var lootTable = GD.Load<LootTable>(path);
                    // _lootTables[(regionId, pathId)] = lootTable;
                }
            }

            // Load legendaries
            var legendaryPaths = new[]
            {
                "res://data/loot/legendaries/Dominion.tres",
                "res://data/loot/legendaries/Frostbite.tres",
                // ... all 28+ legendaries
            };

            foreach (var legPath in legendaryPaths)
            {
                // GODOT: var legendary = GD.Load<LegendaryDefinition>(legPath);
                // _legendaries[legendary.LegendaryId] = legendary;
            }
        }

        /// <summary>
        /// Get the loot table for a region/path combo.
        /// </summary>
        public LootTable GetLootTable(
            PG.World.Simulation.RegionId region,
            PG.World.Simulation.EvolutionPath path)
        {
            var key = (region, path);
            return _lootTables.ContainsKey(key) ? _lootTables[key] : null;
        }

        /// <summary>
        /// Generate loot when an enemy dies.
        /// </summary>
        public List<Item> GenerateLoot(
            PG.World.Simulation.RegionState regionState,
            EnemyArchetype enemy,
            PG.World.Simulation.WorldTier tier)
        {
            var lootTable = GetLootTable(regionState.RegionId, regionState.CurrentPath);
            var loot = new List<Item>();

            // 1. Always drop guaranteed items
            foreach (var entry in lootTable.GuaranteedDrops)
            {
                var item = CreateItemFromEntry(entry, tier);
                loot.Add(item);
            }

            // 2. Roll for rare drops (25% chance)
            if (GD.Randf() < 0.25f)
            {
                var entry = lootTable.RareDrops[GD.Randi() % (uint)lootTable.RareDrops.Count];
                var item = CreateItemFromEntry(entry, tier);
                loot.Add(item);
            }

            // 3. Roll for legendary (2% chance)
            if (GD.Randf() < 0.02f)
            {
                var legendary = PickRandomLegendary(regionState.CurrentPath);
                if (legendary != null)
                    loot.Add(CreateLegendaryItem(legendary, tier));
            }

            return loot;
        }

        private Item CreateItemFromEntry(LootEntry entry, PG.World.Simulation.WorldTier tier)
        {
            // TODO: Implement Item creation from entry
            // Apply tier scaling, rarity, etc.
            return new Item();
        }

        private LegendaryDefinition PickRandomLegendary(PG.World.Simulation.EvolutionPath path)
        {
            // Find all legendaries that have a variant for this path
            var validLegendaries = _legendaries.Values
                .Where(l => l.Variants.ContainsKey(path))
                .ToList();

            return validLegendaries.Count > 0
                ? validLegendaries[(int)(GD.Randi() % (uint)validLegendaries.Count)]
                : null;
        }

        private Item CreateLegendaryItem(LegendaryDefinition legendary, PG.World.Simulation.WorldTier tier)
        {
            // TODO: Create an Item with all legendary properties
            return new Item();
        }
    }
}
```

---

### BossDatabase.cs
**Purpose:** Manage all boss encounters.

```csharp
namespace PG.Gameplay.Combat
{
    public class BossDatabase
    {
        private static BossDatabase _instance;
        private Dictionary<string, BossEncounter> _bosses;

        public static BossDatabase Instance => _instance ??= new BossDatabase();

        public BossDatabase()
        {
            _bosses = new Dictionary<string, BossEncounter>();
            LoadAllBosses();
        }

        private void LoadAllBosses()
        {
            // GODOT: Load all BossEncounter Resources from res://data/bosses/
            // There should be 2-3 bosses per path (story, optional, optional world boss)
            // Total: ~28 × 2 = 56 boss definitions
            
            var bossPaths = new[]
            {
                "res://data/bosses/northern_realm/age_of_raiders_story.tres",
                "res://data/bosses/northern_realm/age_of_raiders_optional.tres",
                // ... load all
            };

            foreach (var path in bossPaths)
            {
                // GODOT: var boss = GD.Load<BossEncounter>(path);
                // _bosses[boss.BossId] = boss;
            }
        }

        /// <summary>
        /// Get a story boss for a region/path (progression gatekeeper).
        /// </summary>
        public BossEncounter GetStoryBoss(
            PG.World.Simulation.RegionId region,
            PG.World.Simulation.EvolutionPath path)
        {
            return _bosses.Values
                .FirstOrDefault(b => b.Region == region && b.EvolutionPath == path && b.Type == BossType.Story);
        }

        /// <summary>
        /// Get optional bosses for a path (hidden encounters).
        /// </summary>
        public List<BossEncounter> GetOptionalBosses(
            PG.World.Simulation.RegionId region,
            PG.World.Simulation.EvolutionPath path)
        {
            return _bosses.Values
                .Where(b => b.Region == region && b.EvolutionPath == path && b.Type == BossType.Optional)
                .ToList();
        }

        /// <summary>
        /// Get world bosses (raid-level encounters).
        /// </summary>
        public List<BossEncounter> GetWorldBosses(PG.World.Simulation.WorldTier tier)
        {
            return _bosses.Values
                .Where(b => b.Type == BossType.WorldBoss && b.MinimumTier <= tier)
                .ToList();
        }
    }
}
```

---

## Integration Example: Spawning an Enemy

```csharp
// In some GameManager or SpawnSystem

void SpawnEnemyInRegion(RegionState region)
{
    // 1. Get archetype
    var archetype = EnemyDatabase.Instance.GetArchetype(
        region.RegionId, 
        region.CurrentPath, 
        region.CurrentTier);

    if (archetype == null)
        return;

    // 2. Get tier-specific data
    var tierData = archetype.TierProgression[region.CurrentTier];

    // 3. Create enemy actor with stats
    var enemy = new EnemyActor(archetype, tierData);
    enemy.Spawn(GetRandomSpawnPoint());

    // 4. When enemy dies, generate loot
    enemy.OnDeath += () =>
    {
        var loot = LootDatabase.Instance.GenerateLoot(region, archetype, region.CurrentTier);
        foreach (var item in loot)
        {
            DropLootAtLocation(item, enemy.GlobalPosition);
        }
    };
}
```

---

## Testing Checklist

Before moving to multiplayer scaling:

- [ ] All 28 loot tables load correctly
- [ ] Legendary variants load per path
- [ ] Enemy archetypes tier progression is sensible (stats increase ~40% per tier)
- [ ] Loot drop rates match expected distribution (60% common, 25% uncommon, etc.)
- [ ] Legendary drop rates are ~2%
- [ ] All bosses load and have abilities
- [ ] Boss health/damage scales appropriately with tier

---

## Integration with Simulator

The simulator currently only generates *world state* (which paths appear where). To validate the full system:

1. **Simulator generates world state** (region → path → tier)
2. **Game spawns enemies** from that world state using `EnemyDatabase`
3. **Enemies drop loot** from `LootDatabase` based on region/path
4. **Bosses drop legendaries** from `BossDatabase` per encounter

This creates a **full loop:** 
Simulator → Enemy Generation → Loot Generation → Player Progression

---

**Next Step:** Implement these three Database classes in Godot, then integrate with multiplayer scaling rules (Party Size, Enemy Health, Loot Distribution).
