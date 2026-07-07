# Multiplayer Scaling & Network Architecture

**Status:** Foundation design with implementation pointers  
**Scope:** 1-5 player co-op, server-authoritative, deterministic world state  
**Target:** Seamless difficulty scaling, fair loot distribution, responsive client prediction

---

## 1. Party System

### Party Composition

**Maximum Party Size:** 5 players  
**Minimum to Progress:** 1 player (solo-friendly)  
**Optimal Size:** 2-3 players (designed with this in mind)

**Party Roles (optional, not enforced):**
- Attacker (high damage, high cooldowns)
- Ranger (medium damage, mobility)
- Tank (high HP, crowd control immunity windows)
- Support (healing, buffs, utility)

**Recommendations in UI:**
- "1-2 players: Easy experience"
- "3 players: Intended difficulty"
- "4-5 players: Challenging"

---

### Party Formation Rules

**Host creates world** → **Other players join**

1. **World Persistence:**
   - World is owned by host (server instance running on host's machine or dedicated server)
   - World exists for entire session (or indefinitely for persistent servers)
   - Evolution path is locked at world creation (cannot change mid-session)
   - Tier advances every 30 mins of playtime (or configurable)

2. **Player Slots:**
   - 1-5 simultaneous players allowed
   - Joining mid-session allowed (new player spawns at party location or waypoint)
   - Leaving mid-session allowed (world continues, enemies do not reset)
   - Re-joining allowed (player character restored with last inventory state from save)

3. **Inventory Ownership:**
   - Each player has personal inventory (no shared stash)
   - Drop loot anywhere; any player can pick up
   - If player drops loot and leaves, loot despawns after 30 mins

---

## 2. Server-Authoritative Architecture

### Authority Model

**Server owns:**
- World state (region data, evolution path, current tier)
- NPC positions, states, AI decisions
- Enemy spawns, health, status effects
- Loot drops and item spawns
- Damage calculations and stat buffs
- RNG seeds for procedural generation

**Clients predict locally:**
- Player movement (within prediction window)
- Ability animations and VFX
- Enemy death animations (server confirms after 1s)
- Damage numbers (server confirms actual damage)

**Server reconciles:**
- Every 200ms: Receive client input, recalculate world, send state delta
- Every 100ms: Send critical state (player health, status effects, nearby NPC positions)
- On desync: Client requests full state snapshot, rolls back to confirmed state

### Network Update Frequency

| Data Type | Frequency | Max Latency Tolerated |
|-----------|-----------|----------------------|
| Player input (movement) | Every frame (client prediction) | 500ms client-side |
| Server world state | Every 200ms | 250ms (1 frame @ 60Hz) |
| Critical state (health, CC) | Every 100ms | 100ms |
| Positional corrections | On deviation > 1 unit | Varies |
| Ability activation | On fire (server validates) | 200ms |
| Loot spawns | Immediately on enemy death | 500ms |

---

## 3. Enemy Scaling by Party Size

### Base Formula

For an enemy with **base HP** and **base damage**, scale by party size:

$$\text{Adjusted HP} = \text{Base HP} \times (1.0 + 0.4 \times (P - 1))$$
$$\text{Adjusted Damage} = \text{Base Damage} \times (1.0 + 0.2 \times (P - 1))$$

Where **P = number of players**.

**Rationale:**
- HP scales more aggressively (40% per player) to prevent stunlock/burst death
- Damage scales less (20% per player) to avoid one-shotting support players
- Solo remains reasonably challenging; 5-player still requires coordination

### Example (Tier 3 Bandit vs Different Party Sizes)

| Party Size | Base HP | Scaled HP | Base DMG | Scaled DMG |
|-----------|---------|-----------|----------|-----------|
| 1 | 100 | 100 | 30 | 30 |
| 2 | 100 | 140 | 30 | 36 |
| 3 | 100 | 180 | 30 | 42 |
| 4 | 100 | 220 | 30 | 48 |
| 5 | 100 | 260 | 30 | 54 |

**Combat Time:** ~1.5 - 2.5 seconds (adjusted so pacing doesn't change drastically)

---

### Enemy Count Adjustment

Spawn more enemies for larger parties to maintain engagement:

$$\text{Enemy Count} = \text{Base Spawn} \times (1.0 + 0.6 \times (P - 1))$$

**Example:**
- 1 player: 3 enemies
- 2 players: 4.8 enemies (round to 5)
- 3 players: 6.6 enemies (round to 7)
- 5 players: 10.8 enemies (round to 11)

**Spawn Rules:**
- Never spawn more than 20 enemies at once (performance hard cap)
- Spawn new waves when 50% of current wave is defeated
- Maximum 3 simultaneous waves per region

---

## 4. Loot Distribution

### Loot Quantity Scaling

Amount of loot drops increases with party size:

$$\text{Loot Drop Multiplier} = 1.0 + 0.3 \times (P - 1)$$

**Example (from a Tier 3 elite enemy):**
- 1 player: 3 items drop (2 common, 1 uncommon)
- 2 players: 3.9 items → 4 items
- 3 players: 4.8 items → 5 items
- 5 players: 6.2 items → 6 items

### Loot Quality is Not Scaled

- **Rarity distribution stays constant** (60% common, 25% uncommon, 10% rare, 4% epic, 1% legendary)
- Only quantity increases
- This ensures legendary items remain challenging to obtain even in large groups

### Loot Allocation Algorithm (Fair Distribution)

**When an enemy dies:**

1. **Roll total loot** (count and rarity using unmodified tables)
2. **For each legendary item dropped:**
   - Randomly assign to one player
   - If player is offline/disconnected, hold for 2 mins, then despawn
3. **For rare/epic items:**
   - 70% assigned randomly
   - 30% dropped in world (first-come-first-served)
4. **For common/uncommon:**
   - 100% dropped in world (encourages cooperation and sharing)

**Rationale:** Legendaries are guaranteed personal; rares incentivize communication; commons are plentiful.

---

## 5. Network Message Types

### Data Structures (C# pointers)

```csharp
// In src/Net/Messages/

// Client → Server
public class PlayerInputMessage
{
    public uint PlayerId { get; set; }
    public Vector2 MovementDirection { get; set; }
    public float LookAngle { get; set; }
    public List<AbilityActivation> QueuedAbilities { get; set; }
    public uint FrameNumber { get; set; }  // For client-side prediction rollback
}

public class AbilityActivation
{
    public string AbilityId { get; set; }
    public Vector2 TargetPosition { get; set; }
    public uint TargetEntityId { get; set; }
}

// Server → Clients
public class WorldStateSnapshot
{
    public uint FrameNumber { get; set; }
    public long TimestampMs { get; set; }
    
    // All players' positions, health, buffs/debuffs
    public List<PlayerState> Players { get; set; }
    
    // All enemies in render distance
    public List<EnemyState> Enemies { get; set; }
    
    // All items/loot on ground
    public List<ItemDrop> ItemDrops { get; set; }
    
    // Party-wide events (quest progress, evolution tier change)
    public List<GameEvent> Events { get; set; }
}

public class PlayerState
{
    public uint PlayerId { get; set; }
    public Vector2 Position { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public List<StatusEffect> ActiveEffects { get; set; }
    public bool IsAlive { get; set; }
    public uint InventoryHash { get; set; }  // For item pickup validation
}

public class EnemyState
{
    public uint EnemyId { get; set; }
    public string ArchetypeId { get; set; }
    public Vector2 Position { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public List<StatusEffect> ActiveEffects { get; set; }
    public List<AbilityActivation> QueuedAbilities { get; set; }
}

public class ItemDrop
{
    public uint ItemInstanceId { get; set; }
    public string ItemId { get; set; }
    public ItemRarityType Rarity { get; set; }
    public Vector2 Position { get; set; }
    public uint OwnerId { get; set; }  // Null if world-loot
    public long ExpirationTimeMs { get; set; }
}

public class GameEvent
{
    public enum EventType { TierAdvance, PathUnlock, BossSpawned, LegendaryDropped, PlayerDied, PlayerRevived }
    public EventType Type { get; set; }
    public string Message { get; set; }
    public uint TargetPlayerId { get; set; }
}
```

---

### Message Flow Example (Single Ability Cast)

```
[Client] PlayerInputMessage
    ↓ (includes ability_id="slash", target_position=(100, 50))
[Server] Receives, validates ability cooldown and resources
    ↓
[Server] Executes ability on server world state
    ↓ (server RNG rolls for hit/crit, applies damage)
[Server] Broadcasts WorldStateSnapshot with:
    - Updated enemy health
    - Ability animation cue
    - Damage number (deterministic from seed)
    ↓
[Client] Receives snapshot, rolls back prediction, applies real state
    ↓
[Client] Displays damage number + VFX (matches server RNG result)
```

---

## 6. Client-Side Prediction

### Movement Prediction

**Goal:** Avoid rubber-banding when latency spikes

1. **Client sends** movement input continuously
2. **Client predicts** player position locally (apply movement immediately)
3. **Server receives** input, validates (check for speed hacks), calculates real position
4. **Server sends** authoritative position back
5. **If deviation > 0.5 units:** Client snaps to server position (noticeable but not jarring)
6. **If deviation < 0.5 units:** Client interpolates to server position over 200ms

**Configuration in code:**

```csharp
public class ClientPredictionConfig
{
    public float MaxPositionDeviation = 0.5f;  // units
    public float SnapThreshold = 1.0f;          // units (teleport if > this)
    public float InterpolationTime = 0.2f;      // seconds
    public float MaxClientMovementSpeed = 10f;  // units/sec (validate server-side)
}
```

### Ability Prediction

**Limitation:** Cannot fully predict ability because server owns RNG and status effects

1. **Client fires** ability immediately (show animation, VFX)
2. **Server receives** ability activation, validates cooldown/mana
3. **Server calculates** hit/miss/damage using world RNG
4. **Server sends** confirmed outcome
5. **If confirmed:** Animation continues, show actual damage number
6. **If rejected:** Client plays "fizzle" animation, ability goes on 2s cooldown (retry penalty)

**Why this design?**
- Prevents ability spam cheats (server validates all casts)
- Feels responsive for most abilities (instant feedback)
- Rare ability fails are clear feedback for latency/invalid state

---

## 7. Desynchronization & Recovery

### Detection

Server detects desync when:
- Client position deviation > 2.0 units from expected
- Client claims item pickup but server shows item to someone else
- Client health mismatch > 10% from expected
- Ability execution on dead enemy (client didn't receive death yet)

### Recovery Sequence

```
[Server] Detects desync
    ↓
[Server] Sends FullStateSnapshot (all entities, items, effects)
    ↓
[Client] Receives snapshot, rolls back local state
    ↓
[Client] Re-predicts from snapshot + input buffer (last 500ms of inputs)
    ↓
[Client] Resume normal operation
```

**Rollback Time:** ~500ms input window (enough for 60Hz feedback without old input lag)

---

## 8. Disconnection Handling

### Graceful Disconnect (Player Leaves Intentionally)

1. **Client sends** `DisconnectMessage` to server
2. **Server removes** player from world (NPC continues, doesn't reset)
3. **Server saves** player inventory and position
4. **Remaining players** continue playing
5. **Disconnected player's corpse** despawns after 30 seconds

### Ungraceful Disconnect (Timeout)

1. **Server waits** 5 seconds for reconnect attempt
2. **If no reconnect:** Remove player, save character state
3. **After 30 seconds:** Player can rejoin (respawns at last safe waypoint)
4. **Inventory** restored from auto-save (updated every 30 seconds)

### Reconnection

- Player can rejoin same world within 5 minutes
- Character respawns at nearest waypoint
- Inventory restored from last save
- If > 5 mins: Character is moved to "offline queue" (world continues without them; can rejoin on next session)

---

## 9. Stat Calculation (Server-Authoritative Example)

### Base Enemy Damage Calculation

```csharp
public class DamageCalculation
{
    public static float CalculateEnemyDamage(
        EnemyArchetype archetype,
        WorldTier tier,
        int partySize,
        List<StatusEffect> activeEffects)
    {
        // 1. Base damage from archetype tier data
        var tierData = archetype.TierProgression[tier];
        float baseDamage = tierData.BaseDamage;

        // 2. Scale by party size
        float partyScale = 1.0f + 0.2f * (partySize - 1);
        float scaledDamage = baseDamage * partyScale;

        // 3. Apply active status effects
        foreach (var effect in activeEffects)
        {
            if (effect.Type == StatusEffectType.Weakness)
                scaledDamage *= 0.7f;  // 30% damage reduction
            if (effect.Type == StatusEffectType.Enrage)
                scaledDamage *= 1.5f;  // 50% damage boost
        }

        // 4. Variance (small RNG to avoid predictable combat)
        float variance = 1.0f + (DeterministicRandom.Next(-10, 10) / 100f);

        return scaledDamage * variance;
    }

    public static float CalculatePlayerDamage(
        PlayerCharacter player,
        string abilityId,
        List<StatusEffect> activeEffects,
        int partySize)
    {
        // 1. Base damage from ability
        var abilityDef = AbilityDatabase.Instance.GetAbility(abilityId);
        float baseDamage = abilityDef.BaseValue;

        // 2. Scale by player stats (equipment, level)
        float statScale = 1.0f + (player.TotalDamage / 100f);
        float scaledDamage = baseDamage * statScale;

        // 3. Apply active buffs
        foreach (var effect in activeEffects)
        {
            if (effect.Type == StatusEffectType.Empower)
                scaledDamage *= 1.2f;  // 20% damage boost
        }

        // 4. Critical hit check
        float critChance = player.CritChance;
        if (DeterministicRandom.Next(100) < critChance)
            scaledDamage *= 1.5f;  // 1.5x damage multiplier

        return scaledDamage;
    }
}
```

### Health Calculation

```csharp
public static float CalculateEnemyHealth(
    EnemyArchetype archetype,
    WorldTier tier,
    int partySize)
{
    var tierData = archetype.TierProgression[tier];
    float baseHealth = tierData.BaseHealth;
    
    // Scale aggressively by party (prevent one-shot)
    float partyScale = 1.0f + 0.4f * (partySize - 1);
    
    return baseHealth * partyScale;
}

public static float CalculatePlayerHealth(PlayerCharacter player)
{
    // Base health from character class
    float baseHealth = player.BaseHealth;
    
    // Add scaling from equipment/stats
    float bonusHealth = player.TotalVitality * 5;  // 5 HP per vitality point
    
    return baseHealth + bonusHealth;
}
```

---

## 10. Edge Cases & Special Rules

### Simultaneous Damage (Both Deal Lethal Damage in Same Frame)

**Rule:** Defender dies, attacker survives (server determines order based on frame number)

### Player Ability Hits Dead Enemy

**Server-side check:**
- If enemy died < 200ms ago, ability still hits (client prediction lag)
- If enemy died > 200ms ago, ability fizzles (goes on 1s cooldown)

### Player Picks Up Someone Else's Legendary

**Rule:** Allowed, but:
- Original owner is notified ("Player X picked up your legendary")
- Player can request return in social menu (manual trade)
- Prevents ninja looting via UI design (drops grouped per player in minimap)

### World Tier Advances Mid-Combat

**Rule:** Tier advance is **not** retroactive during session
- All spawned enemies stay at current tier
- Next spawn wave uses new tier
- This avoids sudden difficulty spikes mid-fight

### Player Dies, Last Enemy Dies Simultaneously

**Rule:** Party succeeds (loot drops, XP awarded), dead player respawns at waypoint

---

## 11. Data Synchronization (Multi-Region Worlds)

### Evolutionary Tier Progression (Happens Server-Side)

Every 30 minutes of active play:
- Server advances tier by 1 (if any player in world)
- Server recalculates evolution path influence on tier spread
- Sends `TierAdvancedEvent` to all clients
- All spawned enemies keep current HP (do not heal on tier advance)

**Message:**
```csharp
public class TierAdvancedEvent : GameEvent
{
    public WorldTier NewTier { get; set; }
    public List<RegionState> UpdatedRegions { get; set; }
}
```

---

## 12. Implementation Roadmap

### Phase 1: Single-Player Network Foundation (Week 1)
- [ ] Implement `PlayerInputMessage` and `WorldStateSnapshot`
- [ ] Build ENet transport layer (connect/disconnect)
- [ ] Server-side player state management
- [ ] Basic movement sync (no prediction yet)

### Phase 2: Multi-Player Scaling (Week 2)
- [ ] Party formation logic
- [ ] Enemy scaling formulas (health/damage/count)
- [ ] Loot distribution algorithm
- [ ] Party-size stat cache (pre-calculate for speed)

### Phase 3: Client Prediction & Smoothing (Week 3)
- [ ] Movement prediction + interpolation
- [ ] Ability prediction (instant feedback, server validation)
- [ ] Desync detection + recovery
- [ ] Position validation (speed-hack prevention)

### Phase 4: Advanced Features (Week 4)
- [ ] Disconnection recovery
- [ ] Full state snapshots for late joiners
- [ ] Tier advancement synchronization
- [ ] Cross-client ability VFX synchronization

---

## 13. Testing Checklist

Before declaring multiplayer foundation complete:

- [ ] 2 players can connect to same world
- [ ] Enemy damage scales correctly for party size 1-5
- [ ] Enemy HP scales correctly for party size 1-5
- [ ] Loot drops increase in quantity (not rarity) for larger parties
- [ ] Loot assignment: legendary items assigned randomly, commons dropped in world
- [ ] Player movement syncs within 500ms latency
- [ ] Ability activations validated server-side (prevented if on cooldown/out of mana)
- [ ] Damage calculations use DeterministicRandom (reproducible with same seed)
- [ ] Disconnection saves character state
- [ ] Reconnection restores character state correctly
- [ ] Tier advancement is communicated to all clients
- [ ] No stat calculation desyncs (server vs client agree on damage)
- [ ] Party size can scale from 1 to 5 without crashes
- [ ] World state persists correctly during 30-minute test session

---

## 14. Performance Targets

### Network Bandwidth
- **Per-client update rate:** 200ms (5 updates/second)
- **Typical message size:** 2-5 KB per snapshot
- **Total bandwidth per player:** ~10-25 KB/sec (minimal)
- **Server CPU:** Can handle 20+ simultaneous players on modern hardware

### Prediction Latency Tolerance
- **Acceptable latency:** < 500ms for smooth movement
- **Noticeable latency:** 500ms - 1000ms (visible rubber-banding)
- **Unplayable latency:** > 1000ms (frequent rollbacks)

### Desync Frequency Target
- **Goal:** < 1 desync event per 10 minutes of play
- **Recovery time:** < 1 second (full state snapshot)

---

## 15. Configuration (Example in Code)

```csharp
public class MultiplayerConfig
{
    // Party
    public int MaxPartySize = 5;
    public int OptimalPartySize = 3;

    // Scaling
    public float EnemyHealthScalePerPlayer = 0.4f;      // 40% per additional player
    public float EnemyDamageScalePerPlayer = 0.2f;      // 20% per additional player
    public float EnemySpawnScalePerPlayer = 0.6f;       // 60% more enemies per player
    public float LootQuantityScalePerPlayer = 0.3f;     // 30% more loot per player

    // Network
    public int WorldStateUpdateFrequencyMs = 200;
    public int CriticalStateUpdateFrequencyMs = 100;
    public float MaxPositionDeviation = 0.5f;
    public float SnapPositionThreshold = 1.0f;
    public float InterpolationTimeMs = 200f;

    // Disconnection
    public int DisconnectTimeoutSeconds = 5;
    public int CharacterSaveIntervalSeconds = 30;
    public int ReconnectWindowSeconds = 300;  // 5 minutes

    // Tier Progression
    public int TierAdvanceIntervalSeconds = 1800;  // 30 minutes
}
```

---

**Next Phase:** Multiplayer foundation created. Ready for professions & crafting integration (cross-region resource chains) or prototype implementation?
