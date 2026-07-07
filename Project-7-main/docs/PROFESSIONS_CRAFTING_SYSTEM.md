# Professions & Crafting Integration

**Status:** Foundation design with implementation pointers  
**Scope:** 5 crafting professions, region-resource binding, cross-region recipe chains  
**Goal:** Incentivize region exploration, create interdependency between paths, reduce item poverty

---

## 1. Profession Overview

### Five Core Professions

| Profession | Primary Output | Materials From | Progression |
|-----------|----------------|-----------------|-------------|
| **Weaponsmith** | Weapons (swords, axes, bows) | Ore (Iron, Mithril, Starforge) | T1-T5 weapon tiers |
| **Armorcraft** | Armor (chest, helm, legs, gloves, boots) | Leather (Tanned, Resilient, Dragonhide) | T1-T5 armor tiers |
| **Alchemist** | Potions & Elixirs (healing, damage buff, resistance) | Reagents (herbs, crystals, rare essences) | T1-T5 potion potency |
| **Enchanter** | Affixes & Runes (attach to weapons/armor) | Essences (extracted from enemies, bosses) | T1-T5 affix power |
| **Runecarver** | Special items (shields, rings, amulets) & Transmog stones | Runestones (found in T3+ zones, boss drops) | T1-T5 rune complexity |

### Optional Gathering Professions (Future)
- **Mining** → Ore deposits in certain regions
- **Herbalism** → Herb nodes in certain regions
- **Butchering** → Leather from enemy drops
- **Essence Extraction** → Essences from enemy loot

---

## 2. Resource Binding by Region & Evolution Path

### Resource Distribution Matrix

Each of 7 regions spawns 2-3 **signature materials** per evolution path. This creates a **supply chain** that forces region/path diversity.

#### Northern Realm

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Age of Raiders** | Raider Iron (weapon ore) | Common Leather, Wolf Fang (crafting component) | T1-T2 |
| **Eternal Winter** | Mithril Ore (superior ore) | Glacial Shard (enchanting essence), Frost Herb | T2-T3 |
| **Sunken Halls** | Deep Iron (reinforced ore) | Abyssal Pearl (rare essence), Luminous Reagent | T3-T4 |
| **Dominion of Decay** | Starforge Metal (legendary ore) | Death Essence (debuff potions), Decay Stone (runecraft) | T4-T5 |

#### Infernal Lands

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Volcanic Catastrophe** | Volcanic Ore (fire affinity) | Sulfur Powder, Lava Stone (resistance) | T1-T2 |
| **Magma Crown** | Obsidian Shards (sharper ore) | Flame Essence, Molten Leather | T2-T3 |
| **Ash Bringer** | Cinderstone (forged ore) | Ash Ember (transmog material), Infernal Reagent | T3-T4 |
| **Eternal Flame** | Sunstone Metal (legendary fire ore) | Phoenix Essence, Firelight Runestone | T4-T5 |

#### Emerald Forest

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Nature's Descent** | Greenwood Ore (nature-attuned) | Thornvine, Moss Extract (potion) | T1-T2 |
| **Blighted Bloom** | Veildust Ore (corrupted ore) | Blight Essence, Decay Petal (poison) | T2-T3 |
| **Overgrowth** | Rootbark Metal (living ore) | Spore Clusters, Vitality Sap (healing potion) | T3-T4 |
| **Withering Dusk** | Shadowwood Metal (legendary nature ore) | Void Essence, Twilight Runestone | T4-T5 |

#### Crystal Wastes

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Frozen Frontier** | Glacial Crystal (enchanting ore) | Ice Shards, Permafrost Dust | T1-T2 |
| **Shattered Spire** | Resonant Crystal (high-essence ore) | Spell Shard, Arcane Powder | T2-T3 |
| **Fractured Realm** | Dimensional Crystal (rare ore) | Rift Essence, Astral Dust (transmog) | T3-T4 |
| **Eternal Winter's Reach** | Starlight Crystal (legendary ore) | Celestial Essence, Cosmic Runestone | T4-T5 |

#### Twilight Marshes

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Cursed Swamp** | Murk Iron (corrosion-resistant ore) | Bog Leather, Swamp Toxin (poison potion) | T1-T2 |
| **Nightmare Rising** | Haunted Metal (spectral ore) | Spirit Essence, Phantom Dust | T2-T3 |
| **Abyssal Rift** | Void Metal (protective ore) | Shadow Essence, Necrotic Runestone | T3-T4 |
| **Eternal Dread** | Void's Heart (legendary ore) | Oblivion Essence, Death Runestone | T4-T5 |

#### Dragon Peaks

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Skyborn Legacy** | Skyforge Ore (wind-attuned) | Feather Leather, Wind Crystal | T1-T2 |
| **Dragon's Ascent** | Dragonhide Ore (durable ore) | Dragon Leather, Scale Essence | T2-T3 |
| **Sky Dominion** | Celestial Metal (high-tier ore) | Star Essence, Heavenly Dust | T3-T4 |
| **Eternal Sky** | Starlight Ore (legendary ore) | Cosmic Essence, Sky Runestone | T4-T5 |

#### Sunken Temple

| Evolution Path | Signature Material | Secondary Materials | Tier Range |
|---|---|---|---|
| **Forgotten Civilization** | Ruin Iron (ancient ore) | Ancient Leather, Time Dust (alchemical) | T1-T2 |
| **Lost Prophecy** | Mystic Metal (arcane ore) | Rune Leather, Prophecy Essence | T2-T3 |
| **Echoing Void** | Ethereal Metal (phased ore) | Ethereal Leather, Void Essence (transmog) | T3-T4 |
| **Eternal Void** | Voidstone Metal (legendary ore) | Primordial Essence, Eternal Runestone | T4-T5 |

---

## 3. Crafting Recipes & Cross-Region Dependencies

### Recipe Tiers & Progression

Each profession has **5 tiers** matching world tiers:

- **Tier 1:** Common materials, basic recipes (armor +5% stats)
- **Tier 2:** Uncommon materials + 1-2 secondary materials from other regions (armor +10% stats)
- **Tier 3:** Rare materials + 3-4 secondary materials (armor +15% stats, unique properties)
- **Tier 4:** Epic materials + 4-5 secondary materials (armor +20% stats, powerful properties)
- **Tier 5:** Legendary materials + exotic materials (armor +25% stats, transformative properties)

### Example Recipe Chain: Tier 3 Dragonplate Armor

**Goal:** To craft high-quality armor that requires exploration of 4 different regions/paths.

```
Dragonplate Armor (Tier 3, Armorcraft)
├─ 3× Dragonhide Ore (Primary: Dragon's Ascent path, Tier 3)
├─ 2× Resilient Leather (Primary material type: ANY region's Armorcraft output)
├─ 1× Celestial Essence (Secondary: ANY region's Enchanter output, Tier 2+)
├─ 1× Frost Herb (Secondary: Eternal Winter path, Tier 1+)
└─ 1× Infernal Reagent (Secondary: Ash Bringer or Eternal Flame paths, Tier 2+)

Crafting XP Gained: 250 (tier 3 recipe)
Crafting Time: 30 seconds (at workbench)
Required Skill Level: Armorcraft Level 30 (unlocks at ~20 recipes crafted)
```

**Why this design:**
- Player must visit Dragon Peaks (Dragonhide source)
- Player must visit 2+ other regions for essences/herbs
- Creates incentive to farm multiple paths for materials
- Economic opportunity: crafters can specialize in one region, sell materials to others

### Example Recipe: Tier 2 Healing Potion (Alchemist)

```
Greater Healing Potion (Tier 2, Alchemist)
├─ 2× Vitality Sap (Secondary: Overgrowth path, Tier 2+)
├─ 1× Permafrost Dust (Secondary: Frozen Frontier path, Tier 1+)
└─ 1× Crystal Base (Crafted item: Runecarver, Tier 1)

Crafting XP Gained: 100 (tier 2 recipe)
Crafting Time: 10 seconds (at alchemical bench)
Potion Effect: Restore 200 HP over 5 seconds
Cooldown: 15 seconds (can chain heals but risky)
```

### Example Recipe: Tier 4 Transmog Stone (Runecarver)

**Purpose:** Transmog stones allow reshaping of legendary items (change affixes/stats).

```
Eternal Transmog Stone (Tier 4, Runecarver)
├─ 2× Runestones (Primary: T3+ zone drops, or craft from Tier 3 recipes)
├─ 1× Astral Dust (Secondary: Fractured Realm path, Tier 3+)
├─ 1× Void Essence (Secondary: Eternal Dread or Echoing Void paths, Tier 3+)
├─ 5× Luminous Reagent (Tertiary: Sunken Halls path, Tier 2+)
└─ 500 Gold (Currency cost)

Crafting XP Gained: 500 (tier 4 recipe)
Crafting Time: 60 seconds (at rune altar)
Transmog Effect: One legendary item can reroll 3 affixes (choose which 3)
Uses: 1 (consumed on use)
```

---

## 4. Skill Progression System

### Crafting Skill Mechanics

Each profession has a **skill level** (0-100) that:
- Unlocks higher-tier recipes
- Reduces crafting failure chance
- Increases material yield from salvaging
- Increases speed of crafting

```csharp
public class CraftingSkill
{
    public string ProfessionId { get; set; }  // "weaponsmith", "alchemist", etc.
    public int SkillLevel { get; set; }       // 0-100
    public int ExperiencePoints { get; set; } // 0-1000 per level
    
    // Unlocked recipes based on skill level
    public List<string> UnlockedRecipeIds { get; set; }
}

public enum SkillTier
{
    Apprentice = 0,    // Level 0-20 (Tier 1 recipes)
    Journeyman = 1,    // Level 20-40 (Tier 1-2 recipes)
    Artisan = 2,       // Level 40-60 (Tier 2-3 recipes)
    Master = 3,        // Level 60-80 (Tier 3-4 recipes)
    Legendary = 4      // Level 80-100 (Tier 4-5 recipes)
}
```

### XP Progression Example

| Level | Cumulative XP | Tier Unlocked | Key Milestone |
|-------|---------------|---------------|---------------|
| 0-5 | 0-200 | Tier 1 | Learn basic recipes |
| 6-20 | 200-2000 | Tier 2 | First journeyman recipes |
| 21-40 | 2000-6000 | Tier 2-3 | Master common recipes |
| 41-60 | 6000-12000 | Tier 3-4 | Artisan rank obtained |
| 61-80 | 12000-20000 | Tier 4-5 | Master rank obtained |
| 81-100 | 20000-30000 | Tier 5 | Legendary rank obtained |

**XP gain per craft:**
- Successful craft: 50 XP (common), 75 XP (uncommon), 100+ XP (rare/epic/legendary)
- Failed craft: 10 XP (partial credit)
- Salvaging items: 5 XP per item

---

## 5. Workbench System

### Crafting Workbenches (per region)

Each region has **at least one workbench per profession**:

```csharp
public class Workbench
{
    public string RegionId { get; set; }
    public string ProfessionId { get; set; }  // "weaponsmith", "alchemist", etc.
    public Vector2 WorldPosition { get; set; }
    public string NpcCrafter { get; set; }     // "Blacksmith Agnes", "Alchemist Morrow"
    
    // Recipes this bench can craft
    public List<string> AvailableRecipeIds { get; set; }
    
    // Tier of materials this bench can handle (T1, T2, T3, T4, T5)
    public int MaximumTierSupported { get; set; }
}
```

**Rules:**
- Player must travel to a workbench to craft (not crafting from inventory)
- Each profession's benches are in different regions (encourages travel)
- Higher-tier benches are found in T3+ zones (requires progression to access)
- NPCs provide flavor, quest hooks, and material trading

### Workbench Locations by Profession

**Weaponsmith:**
- Northern Realm (Age of Raiders) - Tier 1-2
- Dragon Peaks (Skyborn Legacy) - Tier 2-3
- Infernal Lands (Volcanic Catastrophe) - Tier 3-4
- Crystal Wastes (Fractured Realm) - Tier 4-5

**Armorcraft:**
- Emerald Forest (Nature's Descent) - Tier 1-2
- Dragon Peaks (Dragon's Ascent) - Tier 2-3
- Twilight Marshes (Cursed Swamp) - Tier 3-4
- Sunken Temple (Lost Prophecy) - Tier 4-5

**Alchemist:**
- Northern Realm (Eternal Winter) - Tier 1-2
- Infernal Lands (Magma Crown) - Tier 2-3
- Emerald Forest (Overgrowth) - Tier 3-4
- Dragon Peaks (Sky Dominion) - Tier 4-5

**Enchanter:**
- Crystal Wastes (Frozen Frontier) - Tier 1-2
- Twilight Marshes (Nightmare Rising) - Tier 2-3
- Sunken Temple (Echoing Void) - Tier 3-4
- Northern Realm (Dominion of Decay) - Tier 4-5

**Runecarver:**
- Dragon Peaks (Dragon Peaks tier 1-2) - Tier 1-2
- Sunken Temple (Forgotten Civilization) - Tier 2-3
- All T4+ regions have Tier 4-5 benches

---

## 6. Material Economy & Trading

### Vendor System

**Material Vendors** at each region buy and sell materials at dynamic prices:

```csharp
public class MaterialVendor
{
    public string VendorId { get; set; }
    public string RegionId { get; set; }
    public string ProfessionId { get; set; }
    
    // Materials this vendor specializes in
    public Dictionary<string, int> BuyPrices { get; set; }      // gold per unit
    public Dictionary<string, int> SellPrices { get; set; }     // gold per unit
    
    // Supply/demand tracking (for dynamic pricing)
    public Dictionary<string, int> MaterialInventory { get; set; }
    public Dictionary<string, int> MaxInventoryCap { get; set; }
}

// Vendor behavior
public class DynamicPricing
{
    // If supply is low (< 20% of max), raise buy price 20%, lower sell price 10%
    // If supply is high (> 80% of max), lower buy price 10%, raise sell price 20%
    // Prevents market collapse while encouraging resource circulation
    
    public static int CalculateBuyPrice(
        MaterialVendor vendor,
        string materialId,
        int basePrice)
    {
        float supplyRatio = vendor.MaterialInventory[materialId] / 
                           (float)vendor.MaxInventoryCap[materialId];
        
        if (supplyRatio < 0.2f)
            return (int)(basePrice * 1.2f);  // Short supply: pay more
        if (supplyRatio > 0.8f)
            return (int)(basePrice * 0.9f);  // Over-supplied: pay less
        
        return basePrice;  // Normal supply
    }
}
```

### Player-to-Player Trading (Future, Post-Launch)

- Players can trade materials in social hubs
- Trade tax: 5% of transaction value goes to system gold sink
- Prevents RMT (real money trading) by controlling pricing
- Material supply is tightly controlled (can't farm infinite legendaries)

---

## 7. Crafting Failure & Success Rates

### Failure Mechanics

Higher-tier recipes have **failure chances** based on skill:

```csharp
public class CraftingAttempt
{
    public Recipe Recipe { get; set; }
    public int PlayerSkillLevel { get; set; }
    
    public float CalculateSuccessRate()
    {
        // Base success rate for recipe tier
        float baseSuccess = Recipe.Tier switch
        {
            1 => 0.95f,  // 95% success for tier 1
            2 => 0.85f,  // 85% for tier 2
            3 => 0.70f,  // 70% for tier 3
            4 => 0.50f,  // 50% for tier 4
            5 => 0.30f,  // 30% for tier 5
            _ => 0.50f
        };
        
        // Skill bonus: each 10 levels = +5% success
        float skillBonus = (PlayerSkillLevel / 10) * 0.05f;
        
        return Math.Min(baseSuccess + skillBonus, 0.99f);  // Cap at 99%
    }
    
    public void Resolve()
    {
        float roll = Random.value;  // 0-1
        float successRate = CalculateSuccessRate();
        
        if (roll < successRate)
        {
            // SUCCESS: Produce item, gain XP
            ProduceItem();
            GainXP(Recipe.XpReward);
        }
        else
        {
            // FAILURE: Lose 50% of materials, gain 10 XP consolation
            ConsumeMaterials(0.5f);
            GainXP(10);
        }
    }
}
```

**Why this design:**
- Tier 5 recipes are **always risky** (even at max skill, only 99% success)
- Failure is common (50% at skill 0) but possible with skill (85% at max skill)
- Creates tension: craft rare materials or save them?
- Legendary crafters can attempt impossible recipes that others can't

---

## 8. Legendary Crafting & Unique Items

### Special Recipes (Path-Exclusive Crafts)

Certain paths allow crafting of **unique items** unavailable elsewhere:

```csharp
// Example: Eternal Winter path allows Frostbite weapon crafting
public class FrostbiteWeapon : LegendaryItem
{
    public string CraftingPath => "eternal_winter";
    public int RequiredCrafterLevel => 60;  // Artisan+
    
    public Recipe GetCraftingRecipe()
    {
        return new Recipe()
        {
            Name = "Frostbite Greatsword",
            Materials = new[]
            {
                ("Mithril Ore", 5),
                ("Glacial Shard", 3),
                ("Frost Herb", 2),
                ("Celestial Essence", 1)
            },
            XpReward = 500,
            SuccessRate = 0.30f  // Very difficult
        };
    }
}
```

**Path-Exclusive Unique Items:**
- Frostbite (Eternal Winter) - freezes enemies on hit
- Infernal Blade (Eternal Flame) - burns enemies on hit
- Vitality Shroud (Overgrowth) - heals on kill
- Voidstep Cloak (Eternal Void) - dash ability
- (One unique per path = 28 unique items across all paths)

---

## 9. Salvaging & Disenchanting

### Breaking Down Items for Materials

Players can salvage gear to recover materials:

```csharp
public class SalvageSystem
{
    public static List<(string MaterialId, int Quantity)> SalvageItem(
        Item item,
        int crafterSkillLevel)
    {
        // Legendary items give more materials
        float materialYield = item.Rarity switch
        {
            ItemRarityType.Common => 0.25f,      // 25% of crafting cost returned
            ItemRarityType.Uncommon => 0.35f,    // 35%
            ItemRarityType.Rare => 0.50f,        // 50%
            ItemRarityType.Epic => 0.65f,        // 65%
            ItemRarityType.Legendary => 0.80f,   // 80%
            _ => 0.25f
        };
        
        // Skill bonus: higher skill increases yield by 5% per 20 levels
        float skillBonus = (crafterSkillLevel / 20) * 0.05f;
        materialYield = Math.Min(materialYield + skillBonus, 0.95f);
        
        // Return materials based on item tier
        var materials = new List<(string, int)>();
        foreach (var material in item.CompiledMaterials)
        {
            int recovered = (int)(material.Quantity * materialYield);
            materials.Add((material.MaterialId, recovered));
        }
        
        return materials;
    }
}
```

**Disenchanting** (Enchanter specialty):
- Salvage legendary affixes from items
- Get back 1-3 of the essence used to create them
- Allows reuse of powerful affixes on new items

---

## 10. Material Scarcity & Economy Balancing

### Finite Resources

Materials are **not infinitely farmable**:

```csharp
public class RegionalResourceNodes
{
    // Each region has finite nodes per session
    public int CrystalWastesFrozenFrontier => 12;  // 12 gatherable nodes per 1-hour session
    public int InfernalLandsVolcanicCatastrophe => 15;
    
    // Node respawn: Every 30 minutes (encourages return trips)
    public int NodeRespawnIntervalMinutes => 30;
    
    // Yields per node: 2-5 materials (small amounts)
    // Tier increases: 1-2 extras per tier
}
```

**Why limited resources?**
- Prevents farming→selling loops that crash economy
- Creates scarcity: materials become valuable
- Encourages cooperation: parties share materials for group crafts
- Maintains progression pacing: can't skip tiers with infinite gold

### Material Sinks (Consume Resources)

- **Transmog stones** consume materials (one-time use)
- **Crafting failures** consume materials (teach careful planning)
- **Legendary transmogs** require expensive recipes (costly to reshape)

### Material Sources

- **Enemy drops** (primary, guaranteed on every kill)
- **Boss drops** (secondary, rarer high-tier materials)
- **Regional nodes** (tertiary, finite respawning sources)
- **Vendor trades** (convert one material to another at cost)

---

## 11. Cross-Region Questline (Incentivize Exploration)

### Master Crafter Questline

**Quest Chain:** Complete orders from NPCs in different regions to unlock exclusive recipes.

```csharp
public class MasterCrafterQuest
{
    public string QuestId => "master_crafter_northern_realm";
    public string Region => "Northern Realm";
    public string Crafter => "Blacksmith Agnes";
    
    public List<QuestObjective> Objectives => new()
    {
        new() { 
            Description = "Gather 10× Raider Iron from Age of Raiders path",
            RequiredItem = "Raider Iron",
            RequiredQuantity = 10
        },
        new() { 
            Description = "Craft 5× Tier 1 axes at Agnes's forge",
            RequiredRecipe = "Basic Battle Axe",
            RequiredQuantity = 5
        },
        new() { 
            Description = "Deliver 3 axes to the Raider camp at North Gate",
            RequiredItem = "Basic Battle Axe",
            RequiredQuantity = 3,
            DeliveryLocation = "Raider Camp, North Gate"
        }
    };
    
    public List<string> RewardRecipes => new()
    {
        "Raider's Greataxe",  // Tier 2 exclusive weapon
        "Northern Forge Upgrade"  // Passive: +10% crafting speed at Agnes's forge
    };
}
```

**Questline Structure:**
- 5 regional quest chains (one per accessible region)
- Each chain: gather materials → craft items → deliver
- Rewards: exclusive recipes + crafting speed bonuses at that region

---

## 12. Data Schemas (C# Pointers for Implementation)

### Recipe Data Schema

```csharp
namespace PG.Gameplay.Crafting
{
    public class Recipe
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }  // 1-5
        public string ProfessionId { get; set; }  // "weaponsmith", "alchemist", etc.
        
        // Materials required
        public List<(string MaterialId, int Quantity)> MaterialCosts { get; set; }
        public int GoldCost { get; set; }
        
        // Output item
        public string OutputItemId { get; set; }
        public int OutputQuantity { get; set; }
        
        // Crafting mechanics
        public int CraftingTimeSeconds { get; set; }
        public int MinimumSkillLevel { get; set; }
        public int XpReward { get; set; }
        
        // Requirements
        public List<string> RequiredRegions { get; set; }  // e.g., ["Dragon Peaks", "Infernal Lands"]
        public List<string> RequiredPaths { get; set; }     // e.g., ["Dragon's Ascent", "Eternal Flame"]
        
        // GODOT: Create as Resource, import from CSV
        // res://data/crafting/recipes/weaponsmith_tier_3.tres
    }
    
    public class CraftingRecipeDatabase
    {
        private static Dictionary<string, Recipe> _recipes;
        
        public static Recipe GetRecipe(string recipeId)
        {
            // Load from Resources or cached dictionary
            return _recipes.TryGetValue(recipeId, out var recipe) ? recipe : null;
        }
        
        public static List<Recipe> GetRecipesForProfession(string professionId, int playerSkillLevel)
        {
            // Return only recipes player can craft (skill level check)
            return _recipes.Values
                .Where(r => r.ProfessionId == professionId && r.MinimumSkillLevel <= playerSkillLevel)
                .OrderBy(r => r.Tier)
                .ToList();
        }
    }
}
```

### Material Data Schema

```csharp
public class Material
{
    public string MaterialId { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int Tier { get; set; }
    public string PrimaryRegion { get; set; }  // Where it primarily drops
    public string PrimaryPath { get; set; }    // Which path it comes from
    public int BaseGoldValue { get; set; }     // Vendor selling price
    
    // Rarity of material drops
    public ItemRarityType MaterialRarity { get; set; }  // Common/Uncommon/Rare/Epic
    
    // Used in which professions
    public List<string> UsedInProfessions { get; set; }
}

public class MaterialDatabase
{
    private static Dictionary<string, Material> _materials;
    
    public static Material GetMaterial(string materialId)
    {
        return _materials.TryGetValue(materialId, out var material) ? material : null;
    }
    
    public static List<Material> GetMaterialsForRegion(
        string regionId,
        string pathId,
        int tier)
    {
        // Return materials available in this region/path/tier combo
        return _materials.Values
            .Where(m => m.PrimaryRegion == regionId && m.PrimaryPath == pathId && m.Tier == tier)
            .ToList();
    }
}
```

### Crafter State Schema

```csharp
public class CrafterState
{
    public string PlayerId { get; set; }
    
    // Profession levels (0-100 for each)
    public Dictionary<string, int> ProfessionSkills { get; set; }
    public Dictionary<string, int> ProfessionXP { get; set; }
    
    // Crafted items count (for stat tracking)
    public Dictionary<string, int> ItemsCraftedByRecipe { get; set; }
    
    // Materials in inventory
    public Dictionary<string, int> MaterialInventory { get; set; }
    
    // Active crafting (in progress)
    public CraftingProgress OngoingCraft { get; set; }
    
    // Unlocked recipes
    public List<string> UnlockedRecipeIds { get; set; }
}

public class CraftingProgress
{
    public string RecipeId { get; set; }
    public long StartTimeMs { get; set; }
    public int DurationMs { get; set; }
    public float ProgressPercent { get; set; }
    
    public bool IsComplete => ProgressPercent >= 1.0f;
}
```

---

## 13. Implementation Roadmap

### Phase 1: Foundation (Week 1)
- [ ] Create Recipe and Material data schemas
- [ ] Build CraftingRecipeDatabase and MaterialDatabase
- [ ] Implement basic recipe crafting (success/failure logic)
- [ ] Tie to world state (materials drop based on region/path)

### Phase 2: Professions & Skills (Week 2)
- [ ] Implement CrafterState and skill progression
- [ ] Add skill level checks to recipe access
- [ ] Implement workbenches in regions
- [ ] Add crafting NPCs with dialogue

### Phase 3: Economy & Vendors (Week 3)
- [ ] Implement material vendors with dynamic pricing
- [ ] Add salvage system
- [ ] Create material vendor behavior (buy/sell based on supply)
- [ ] Link vendor prices to player market activity

### Phase 4: Advanced Features (Week 4)
- [ ] Path-exclusive unique item recipes
- [ ] Master Crafter questlines
- [ ] Legendary transmog crafting
- [ ] Cross-region recipe requirements with UI guides

---

## 14. Testing Checklist

Before declaring professions complete:

- [ ] Recipe crafting succeeds at appropriate rates (95% tier 1, 30% tier 5 at skill 0)
- [ ] Skill progression is sensible (100 crafts to reach skill level 20)
- [ ] Materials are recoverable via salvage (80% of legendary material cost)
- [ ] Vendor prices change based on supply (low supply = higher buy price)
- [ ] Path-exclusive recipes only available in correct regions
- [ ] Crafting time scales by recipe tier (30s tier 3, 60s tier 4)
- [ ] Players can't craft without visiting correct workbench
- [ ] Material costs are proportional to recipe tier (tier 5 costs 3-5× tier 1)
- [ ] Cross-region recipes force region/path diversity
- [ ] Quest rewards unlock appropriate exclusive recipes

---

## 15. Economy Constraints & Balancing

### Anti-Inflation Measures

1. **Material Costs Scale Exponentially:**
   - Tier 1 recipe: 5 materials
   - Tier 2: 7-10 materials
   - Tier 3: 10-15 materials
   - Tier 4: 15-20 materials
   - Tier 5: 20-30+ materials

2. **Failure Cost Breaks Farming Loops:**
   - Failed crafts consume 50% materials
   - At tier 4 (50% success), average material cost = recipe cost × 2
   - Prevents RMT by making material farming inefficient

3. **Transmog Cost Creates Gold Sink:**
   - Transmog stone: 500 gold + 5 materials
   - Reshaping legendary: 1000 gold + transmog stone + 10+ materials
   - Forces players to actually hunt legendaries (not just buy from farmers)

4. **Limited Resource Nodes:**
   - Max 15 gatherable nodes per region per session
   - Respawn every 30 minutes
   - Can't farm infinitely; encourages multiplayer cooperation

---

## 16. Configuration (Example)

```csharp
public class CraftingConfig
{
    // Skill progression
    public int MaxSkillLevel = 100;
    public int XPPerLevel = 1000;
    public float SkillBonusPerTenLevels = 0.05f;  // +5% success per 10 levels
    
    // Recipe tiers
    public Dictionary<int, float> BaseSuccessRateByTier = new()
    {
        { 1, 0.95f }, { 2, 0.85f }, { 3, 0.70f }, { 4, 0.50f }, { 5, 0.30f }
    };
    
    // Material economy
    public float SalvageYieldCommon = 0.25f;
    public float SalvageYieldLegendary = 0.80f;
    public float MaterialScarcityMultiplier = 1.0f;  // Adjust to make farming harder/easier
    
    // Vendors
    public float LowSupplyBuyBonus = 1.2f;   // Buy 20% more when supply low
    public float HighSupplySellPenalty = 1.2f; // Sell for 20% more when supply high
    public float SupplyThreshold = 0.2f;     // Below this ratio = low supply
}
```

---

**Next Phase:** Professions & Crafting foundation created. Ready for procedural quests & NPC migration (systems that use crafting materials for quest rewards) or prototype implementation?
