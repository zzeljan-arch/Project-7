# Enemy & Loot Generation Rules - Foundation Design

**Version:** 1.0  
**Status:** Foundation & Prototype Pointers (Not Final Implementation)  
**Next Step:** Implement in Godot as `Resource` subclasses and C# data classes

---

## Part 1: Enemy Evolution Trees

Every enemy evolves through 5 tiers, gaining abilities, stat multipliers, and AI complexity.

### Evolution Pattern (Universal)

**Tier 1 → Tier 2 → Tier 3 → Tier 4 → Tier 5**

Each evolution:
- +30-50% base stats (health, damage)
- +1 ability or passive
- Improved AI (more aggressive, smarter pathing)
- Visual upgrade (armor, size, effects)
- +10-20% loot multiplier

### Example: Northern Realm - Bandit Evolution (Age of Raiders Path)

```
Tier 1: Bandit
  - Health: 20
  - Damage: 5
  - Abilities: [Slash]
  - Rarity: Common
  - Loot: Leather Armor, Copper Coins

Tier 2: Veteran Bandit
  - Health: 30 (+50%)
  - Damage: 8 (+60%)
  - Abilities: [Slash, Dash Attack]
  - Rarity: Uncommon
  - Loot: Iron Sword, Silver Coins

Tier 3: Elite Raider
  - Health: 45 (+50%)
  - Damage: 12 (+50%)
  - Abilities: [Slash, Dash Attack, Riposte]
  - Rarity: Rare
  - Loot: Steel Sword, Raider Helm

Tier 4: Runic Raider
  - Health: 70 (+55%)
  - Damage: 18 (+50%)
  - Abilities: [Slash, Dash Attack, Riposte, Rune Strike (magic)]
  - Rarity: Epic
  - Loot: Enchanted Raider Sword, Ancient Runic Armor

Tier 5: Champion Raider
  - Health: 110 (+57%)
  - Damage: 27 (+50%)
  - Abilities: [Slash, Dash Attack, Riposte, Rune Strike, Warlord's Fury (ultimate)]
  - Rarity: Legendary
  - Loot: Legendary Raider Greatsword, Champion Armor Set
```

---

## Part 2: Enemy Roster Per Region & Path

Each region/path combination has a **unique enemy roster** that changes with tiers.

### Northern Realm - Age of Raiders (Tier 1 Roster)

| Enemy | Count | Role | Loot Type |
|-------|-------|------|-----------|
| Bandit | 60% | Melee | Weapons, Coins |
| Wolf | 20% | Ranged/Rush | Fur, Fangs |
| Ice Goblin | 15% | Mage | Mana Crystals |
| Hunter | 5% | Ranged/Boss | Hunting Gear |

**Tier 2 Roster:**
Replace base enemies with evolved versions:
- Bandit → Veteran Bandit
- Wolf → Frost Wolf (new abilities)
- Ice Goblin → Ice Shaman (spells)
- Hunter → Raider Captain

**Design Principle:** Enemies don't get *replaced*; they *evolve*. A player who hunts Bandits in T1 meets progressively harder Bandits through T5.

---

## Part 3: Loot Generation System

### Guaranteed Legendary Pool (Always Available)

Each class has 3-4 signature weapons that ALWAYS appear somewhere across all campaigns:

**Warrior Legendaries:**
- Dominion (Greatsword)
- Bastion (Shield)
- Titan's Fist (Maul)
- Runebrand (Sword)

**Ranger Legendaries:**
- Frostbite (Bow)
- Venom (Dagger)
- Swiftshot (Rifle)
- Piercer (Spear)

**Mage Legendaries:**
- Archmage's Staff
- Spellbinder (Tome)
- Void Wand
- Infinity (Orb)

**Cleric Legendaries:**
- Radiance (Mace)
- Divine Shield
- Holy Wrath (Staff)
- Redemption (Pendant)

---

### Path-Exclusive Legendary Variants

**Same legendary, different affixes based on path:**

**Dominion (Greatsword) Variants:**

| Path | Variant Name | Affix | Flavor |
|------|--------------|-------|--------|
| Age of Raiders | Conqueror's Dominion | +Plunder Bonus (25% more loot) | Stolen from defeated warlords |
| Eternal Winter | Frostbrand Dominion | Freeze on Hit (10%) | Imbued with ancient ice |
| Ragnarök | Godly Dominion | Divine Strike (+20% vs demons) | Crafted by Norse gods |
| Undead North | Necrotic Dominion | Life Drain (25%) | Corrupted by death magic |

**Why?** Players will hunt specific paths for specific builds.

---

## Part 4: Loot Table Architecture

### Loot Table Hierarchy

```
Region Loot Pool
├── Path-Specific Pool
│   ├── Guaranteed Tier (always drops)
│   ├── Rare Tier (25% drop)
│   └── Legendary Tier (2-5% drop)
├── Enemy-Specific Drops
└── Environmental Loot
```

### Example: Northern Realm - Age of Raiders Path

```
Primary Loot (Region + Path):
- Axes (Viking-style)
- Fur Armor
- Trade Goods
- Merchant Records
- Stolen Treasures

Tier Progression:
  T1: Iron Axes, Common Fur Armor, Copper Coins
  T2: Steel Axes, Uncommon Fur Armor, Silver Coins
  T3: Enchanted Axes, Rare Bear Fur, Gold Coins
  T4: Legendary Axes, Epic Viking Armor, Gems
  T5: Champion Axes, Mythic Armor Sets, Ancient Artifacts

Guaranteed Drops:
- Every enemy: Coins (scaled by tier)
- Every boss: One unique item + guaranteed legendary chance

Path-Exclusive Drops:
- Raider Clan Marks (faction item, trade with merchants)
- Naval Charts (unlock fast travel)
- Plunder Maps (lead to treasure chests)
```

---

## Part 5: Boss System

### Boss Categories

**1. Story Boss (one per region)**
- Appears at Tier 3+
- Unlocks next progression phase
- Drops guaranteed unique legendary

**2. Optional Boss (one per path)**
- Appears at Tier 2+
- Hidden in dungeons or events
- Higher loot multiplier than story boss

**3. World Boss (region-wide event)**
- Appears at Tier 4+
- Requires group effort
- Drops exclusive cosmetics

### Example: Northern Realm - Age of Raiders Path

**Story Boss: Warlord Jarl (Tier 2)**
- Health: 300
- Abilities: [Cleave, Shout (buff allies), Shield Bash]
- Loot: Warlord's Crown (helm), Raider's Codex (faction item)

**Optional Boss: Frost King (Tier 3)**
- Health: 500
- Abilities: [Frost Bolt, Ice Wall, Freeze]
- Loot: Crown of Winter, Frostbrand Sword (legendary variant)

**World Boss: Jörmungandr Avatar (Tier 5)**
- Health: 2000
- Requires 4 players
- Abilities: [Tidal Wave, Corruption Bite, Summon Minions]
- Loot: Sea Dragon's Fang (legendary), Ship Captain's Flag (cosmetic)

---

## Part 6: Rarity System

```
Common (60%)
├─ Basic stats
├─ No affixes
└─ Vendor: Low value

Uncommon (25%)
├─ +15% stats
├─ 1 affix (e.g., +Fire Damage)
└─ Vendor: Medium value

Rare (10%)
├─ +30% stats
├─ 2 affixes
└─ Vendor: High value

Epic (4%)
├─ +50% stats
├─ 3 affixes + special visual
└─ Vendor: Very high value

Legendary (1%)
├─ +100% stats
├─ 4 affixes + unique ability
├─ Special visual/glow
└─ Vendor: Highest value
```

---

## Part 7: Legendary Item Mechanics

### Transmog System (Reshape Legendary Stats)

**Problem:** Player finds a Legendary Mage Staff but plays Warrior.

**Solution:** Transmog allows changing:
- Stat distribution (prioritize Strength instead of Intelligence)
- Damage type (Physical → Magical)
- Affix rerolls (limited per item, uses crafting materials)

**Cost:** Region-specific materials (e.g., 10 Infernal Ash + 5 Mana Crystals)

### Affixes (Powers on Items)

**Common Affixes:**
- `+10-20% Damage`
- `+5-10 Health`
- `+5% Critical Chance`
- `+Fire Damage 10-20`

**Path-Exclusive Affixes:**
- `Age of Raiders`: Plunder Bonus, Morale Buff, Trade Discount
- `Eternal Winter`: Freeze, Cold Resist, Movement Speed Penalty
- `Ragnarök`: Divine Strike, Fate Threads, God's Favor
- `Undead North`: Life Drain, Curse, Death Resist

**Legendary Unique Powers:**
- Dominion: "When you deal damage, nearby allies gain +5% damage for 5 seconds"
- Frostbite: "Shots freeze enemies; frozen enemies take +50% damage"
- Archmage's Staff: "Spells cost 20% less mana after landing a hit"

---

## Part 8: Data Schema (Godot Resource Design)

### C# Base Classes (for Godot)

**File:** `src/Gameplay/Loot/LegendaryDefinition.cs`

```csharp
namespace PG.Gameplay.Loot
{
    /// <summary>
    /// Defines a single legendary item and its path variants.
    /// Godot equivalent: Export as a Resource with nested Resources for variants.
    /// </summary>
    public class LegendaryDefinition
    {
        public string LegendaryName { get; set; }  // "Dominion"
        public ItemType Type { get; set; }  // Weapon, Armor, etc.
        public string BaseDescription { get; set; }
        public Dictionary<EvolutionPath, LegendaryVariant> Variants { get; set; }
    }

    public class LegendaryVariant
    {
        public string VariantName { get; set; }  // "Godly Dominion"
        public string FlavorText { get; set; }
        public List<string> Affixes { get; set; }  // ["Divine Strike", "Light Aura"]
        public float StatMultiplier { get; set; }  // 1.2 = 20% bonus
        public Color GlowColor { get; set; }  // Visual identity
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Consumable
    }
}
```

**Godot Resource Structure:**
```
res://data/legendaries/
├── Dominion.tres          (Resource with variants nested)
├── Frostbite.tres
├── Archmage_Staff.tres
└── ...
```

---

### Enemy Definition Schema

**File:** `src/Gameplay/Combat/EnemyArchetype.cs`

```csharp
namespace PG.Gameplay.Combat
{
    /// <summary>
    /// Defines how an enemy evolves across tiers.
    /// Export as Resource in Godot with tier-specific data.
    /// </summary>
    public class EnemyArchetype
    {
        public string ArchetypeName { get; set; }  // "Bandit"
        public RegionId HomeRegion { get; set; }
        public EvolutionPath AssociatedPath { get; set; }
        
        // Tier-specific data (indexed by WorldTier)
        public Dictionary<WorldTier, EnemyTierData> TierProgression { get; set; }
    }

    public class EnemyTierData
    {
        public float Health { get; set; }
        public float Damage { get; set; }
        public List<string> Abilities { get; set; }
        public ItemRarityType LootRarity { get; set; }
        public List<string> LootTable { get; set; }
        public string VisualVariant { get; set; }  // "bandit_tier3_mesh"
    }

    public enum ItemRarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
```

---

### Loot Table Schema

**File:** `src/Gameplay/Loot/LootTable.cs`

```csharp
namespace PG.Gameplay.Loot
{
    /// <summary>
    /// Defines what items drop from a region/path combination.
    /// Godot: Store as Resource + nested Resources or CSV importers.
    /// </summary>
    public class LootTable
    {
        public RegionId Region { get; set; }
        public EvolutionPath Path { get; set; }
        
        public List<LootEntry> GuaranteedDrops { get; set; }
        public List<LootEntry> RareDrops { get; set; }  // 25% chance
        public List<LootEntry> LegendaryDrops { get; set; }  // 2-5% chance
    }

    public class LootEntry
    {
        public string ItemId { get; set; }  // Reference to legendary or template
        public float Weight { get; set; }  // Relative probability
        public WorldTier MinimumTier { get; set; }
        public int StatScaling { get; set; }  // How much tier affects stats
    }
}
```

---

## Part 9: Procedural Generation Hooks

### How the Simulator Integrates with Loot Generation

```csharp
// In Godot gameplay code:
void SpawnEnemy(RegionState region)
{
    // 1. Get enemy archetype from region/path
    var archetype = EnemyDatabase.GetArchetype(
        region.RegionId, 
        region.CurrentPath,
        region.CurrentTier
    );
    
    // 2. Spawn with tier-specific stats
    var enemy = SpawnEnemyWithTierData(archetype, region.CurrentTier);
    
    // 3. When enemy dies, generate loot
    enemy.OnDeath += () => GenerateLoot(region);
}

void GenerateLoot(RegionState region)
{
    // 1. Get loot table for this region/path
    var lootTable = LootDatabase.GetTable(
        region.RegionId, 
        region.CurrentPath
    );
    
    // 2. Roll for legendary
    if (rng.Probability(0.02))  // 2% chance
    {
        var legendary = PickLegendary(region.CurrentPath);
        DropItem(legendary);
    }
    
    // 3. Roll for rare/common
    var commonItem = lootTable.RareDrops[rng.Next(lootTable.RareDrops.Count)];
    DropItem(commonItem);
}
```

---

## Part 10: Design Pointers for Implementation

### What Goes Where

| System | Location | Type | Priority |
|--------|----------|------|----------|
| Enemy Archetypes | `src/Gameplay/Combat/EnemyDatabase.cs` | C# + Godot Resources | P1 |
| Loot Tables | `src/Gameplay/Loot/LootDatabase.cs` | C# + CSV/JSON import | P1 |
| Legendary Definitions | `res://data/legendaries/` | Godot Resources | P1 |
| Boss Specs | `src/Gameplay/Combat/BossDatabase.cs` | C# + Resources | P2 |
| Affixes Engine | `src/Gameplay/Loot/AffixSystem.cs` | C# (pure logic) | P2 |
| Transmog | `src/Gameplay/Loot/TransmogCrafter.cs` | C# + UI | P3 |

### Recommended Implementation Order

1. **Phase 3A (Week 1-2):** Enemy Archetypes + Loot Tables (core data)
2. **Phase 3B (Week 2-3):** Boss Encounters + Legendary Variants
3. **Phase 3C (Week 3-4):** Affixes + Transmog system
4. **Phase 4:** Integrate with multiplayer scaling

---

## Part 11: Validation Checklist

Before moving to multiplayer scaling:

- [ ] All 7 regions have complete enemy rosters (T1-T5)
- [ ] 28 legendary items defined with path variants
- [ ] Loot tables test in simulator (drop rates verified)
- [ ] Boss encounters scaled and tested at T3+ in each region
- [ ] Transmog system accepts valid inputs and prevents cheese
- [ ] No legendary appears in wrong region/path combination
- [ ] Enemy abilities scale sensibly (T1 → T5)
- [ ] Loot rarity distribution matches target (60% common, etc.)

---

## Example Integration Test (Pseudocode)

```csharp
void TestEnemyEvolution()
{
    var northernRaiders = EvolutionRules.GetEnemyRoster(
        RegionId.NorthernRealm,
        EvolutionPath.AgeOfRaiders
    );
    
    foreach (var tier in new[] { Tier1, Tier2, Tier3, Tier4, Tier5 })
    {
        var bandit = northernRaiders["Bandit"][tier];
        Assert.Greater(bandit.Health, previousTier.Health);
        Assert.Greater(bandit.Abilities.Count, previousTier.Abilities.Count);
    }
}

void TestLootDrop()
{
    var region = new RegionState(RegionId.NorthernRealm, Tier2, AgeOfRaiders);
    var loot = GenerateLoot(region);
    
    // Verify loot is from correct pool
    Assert.IsTrue(loot.IsFromPath(AgeOfRaiders));
    
    // Verify rarity distribution (run 100x)
    int legendaryCount = 0;
    for (int i = 0; i < 100; i++)
    {
        var item = GenerateLoot(region);
        if (item.Rarity == Legendary) legendaryCount++;
    }
    Assert.InRange(legendaryCount, 1, 5);  // ~2% legendary
}
```

---

## Next: Multiplayer Scaling

Once this foundation is solid, **Part 12: Multiplayer Scaling** will use these definitions to:
- Scale enemy stats for party size
- Adjust loot multipliers
- Distribute rewards fairly
- Prevent trivial damage floors

---

**Status:** Foundation complete. Ready for Godot resource implementation and linked data structure testing.
