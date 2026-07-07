# Minimal Playable Prototype

**Status:** Foundation design and starter code skeletons  
**Scope:** Local prototype with headless server logic and simple client integration

---

## Goal

Create the smallest playable slice that proves the core loop:
- Generate world state from evolution simulation
- Spawn region-based enemies
- Hit enemies, deal damage, and kill them
- Generate loot drops
- Accept a quest and make progress
- Keep the simulation deterministic and data-driven

This prototype is not final gameplay. It is a lightweight engine test bed for systems integration.

---

## Prototype Scope

### Minimum viable features

- `GameInstance` orchestrates world and game systems
- `RegionManager` holds world region state and selected path data
- `EnemySpawner` loads `EnemyArchetype` definitions and chooses enemies for the current region
- `LootGenerator` rolls from `LootTable` data and creates item instances
- `QuestManager` generates and tracks one simple quest
- Local headless server mode with a simple client-facing API

### Explicit exclusions

- No advanced AI behavior
- No full UI system
- No synchronized multiplayer world state
- No full inventory/equipment system
- No player class progression tree

---

## Architecture

### Core components

- `PrototypeGameManager` — root server loop and tick.
- `RegionManager` — region selection, path state, and tier information.
- `EnemyDatabase` — enemy archetype lookup and tier progression.
- `LootDatabase` — loot table lookup and item roll logic.
- `QuestManager` — quest template, quest assignment, and objective tracking.

### Data flow

1. `PrototypeGameManager` initializes the world with `RegionManager`.
2. `EnemyDatabase` provides enemies for a selected region and tier.
3. `EnemySpawner` places enemies and starts combat simulation.
4. On death, `LootDatabase` generates loot items.
5. `QuestManager` tracks quest objectives and rewards.

---

## Implementation order

1. Build world state loader from `RegionManager`.
2. Load `EnemyArchetype` and `LootTable` resources.
3. Create `PrototypeGameManager` loop and a basic `Tick(float deltaTime)` method.
4. Implement enemy spawn / kill / loot generation flow.
5. Add the first quest type: simple kill objective.
6. Connect prototype systems to a local test harness.
7. Validate with deterministic seed-based runs.

---

## Prototype acceptance criteria

- [ ] The game manager can initialize a world for seed-based deterministic runs.
- [ ] The engine can spawn an enemy in a region and track its health.
- [ ] Enemy kills properly invoke loot generation.
- [ ] A quest can be accepted and updated when kills occur.
- [ ] The data pipeline is configurable via resource-like classes.
- [ ] Basic server-client separation is in place, even if the client remains stubbed.

---

## Next step

Use this doc to build the actual prototype in Godot. The next implementation will add the first playable test scene and wire the game manager to a simple player node.
