Project Architecture - Procedural Co-op ARPG (Godot 4, C#)

Purpose
- Provide a scalable, maintainable architecture for a 2D/2.5D co-op ARPG built in Godot 4 using C# for core logic.
- Emphasize decoupling, testability, and data-driven systems that can scale as content grows.

Engine decision
- Chosen: Godot 4 with C# (dotnet 6+)
  - Reasons: Lightweight editor, easy iteration for 2D/2.5D, strong support for scene composition, open-source flexibility, and a simpler toolchain for small-to-medium teams. C# support in Godot 4 is mature enough for production-quality systems and makes porting simulation code from Python easier.
  - Tradeoffs: Godot's built-in multiplayer tools are less feature-rich than AAA engines; we'll adopt a server-authoritative model using ENet or third-party transport and implement deterministic headless simulations where necessary.

High-level architecture (layers adapted for Godot)
1. Engine Glue / Core
   - Godot runtime wrappers, custom C# services, and low-level utilities.
   - Key types: `Node`, `Node2D`/`Node3D`, `SceneTree`, `Autoload` singletons (services).
2. Core Services Layer
   - Save/Load, Event Bus (Signals + C# events), Time/Simulation stepping, Logging, Asset loading (PackedScene/Resource).
   - Implement as Autoload singletons (`/autoload/`) to provide global services.
3. World Systems Layer
   - Region Manager, Evolution Engine (deterministic simulation), Procedural Generator, Event System.
   - Implement core logic in C# classes (no direct Node dependencies) and expose data via `Resource` subclasses for designers.
4. Gameplay Layer
   - Combat, AI (Behavior Trees via addons or custom), Inventory, Skills, Loot, Crafting.
   - Use Scenes (`PackedScene`) for actor prefabs; keep gameplay logic in C# classes that operate on data models.
5. Networking Layer
   - Server-authoritative dedicated server (headless Godot or .NET host) with ENet transport; clients are thin and render local state.
   - Use RPCs sparingly; prefer deterministic simulations or state reconciliation for world evolution.
6. Presentation Layer (UI & Input)
   - Godot UI (Control nodes) and InputMap; keep UI logic mostly in C# scripts and scenes.
7. Tools & Editor Layer
   - Editor plugins, custom inspector scripts, and content validation tools (GDNative/C# EditorScripts where needed).

Design patterns & principles (Godot-adapted)
- Data-driven design: use `Resource`/`PackedScene` and external JSON or CSV for bulk content import.
- Composition: small, focused nodes and POCO data objects; avoid massive monolithic scenes.
- Service/Subsystem separation: Autoload singletons for global services, modular systems for game features.
- Signals + C# events: use Godot `Signal` for scene-level events and a C# `IEventBus` for decoupled server-side messaging.
- Deterministic simulation: implement evolution engine as pure C# logic (seedable RNG) runnable in headless mode or outside Godot for analysis.
- Content ownership: maintain clear ownership (World, Gameplay, Net, Tools) and keep designer-friendly data formats.

Code organization and folders
- `src/Core` - core utilities, deterministic simulation helpers, math, time
- `src/World` - evolution engine, region manager, procedural generator
- `src/Gameplay` - combat, inventory, loot, skills
- `src/Net` - networking adapters, replication helpers, serialization utilities
- `src/Tools` - editor plugins and content pipeline tools
- `res://scenes` - PackedScene prefabs and level seeds
- `res://data` - Resource/JSON data assets

Data flow and dependencies
- Data (`Resource` / JSON) -> World Systems -> Gameplay -> Presentation
- Dedicated server or headless simulator runs authoritative evolution; clients render replicated state

First milestone rationale
- Create the project skeleton, C# module layout, deterministic world-evolution simulator (standalone C# or Godot headless) and simple data schema for regions.
- Reason: evolution engine defines content types and constraints; validate it early to reduce rework in content and gameplay systems.

Next steps
1. Create Godot project skeleton (res://scenes, res://data, src/ modules) and CI for headless runs.
2. Define data schemas (`Resource` subclasses or JSON schema) for regions, paths, enemies, loot.
3. Implement a headless deterministic simulator (prototype in C# or Python for fast iteration, then integrate into `src/World`).

