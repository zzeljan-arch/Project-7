Coding Standards & Naming Conventions (C# / Unity focused)

Goals
- Readability and predictability across large teams
- Small, testable units of code
- Clear ownership and minimal coupling

General rules
- Language: C# 10+ (if Unity, match Unity-supported C# version)
- Formatting: Use EditorConfig and the project's C# formatting settings. Keep lines <= 120 chars.
- Comments: Use XML doc comments for public APIs. Use short inline comments only when the intent isn't clear.
- TODOs: Use TODO/TICKET tags linking to issue tracker IDs.

Folder and namespace mapping
- `src/Core` -> namespace `PG.Core`.
- `src/Gameplay/Combat` -> `PG.Gameplay.Combat`.
- Match folder layout to namespaces 1:1.

Naming conventions
- Classes / Structs: PascalCase (e.g., `InventoryManager`, `RegionEvolutionEngine`).
- Interfaces: `I` prefix (e.g., `IService`, `IRegionProvider`).
- Methods: PascalCase (e.g., `ApplyDamage`).
- Fields: private fields `_camelCase` with leading underscore; serialize private with `[SerializeField]` if needed.
- Properties: PascalCase (e.g., `CurrentTier`).
- Events: `On` prefix for delegates (e.g., `OnRegionEvolved`).
- Enums: PascalCase values (e.g., `WorldTier.Tier1`).
- Constants: PascalCase (e.g., `DefaultMaxPlayers`).

Component & System design
- Components are data-only where possible (POCOs / structs). MonoBehaviours should be thin glue to engine.
- Systems implement behavior operating on components and services.
- Prefer pure C# classes for game logic; keep Unity API calls in adapter classes.

ScriptableObjects & Data
- Use ScriptableObjects for static content data (enemy templates, loot tables, evolution paths).
- Keep ScriptableObjects purely data; implement behavior in systems.

Events & Messaging
- Use a central event bus `IEventBus` for cross-cutting events.
- Avoid direct component references across systems; use message passing with well-defined event types.

Dependency injection
- Use constructor injection in pure C# classes.
- For MonoBehaviours, use a minimal DI container (Zenject or Unity's DI) for services.

Testing
- Unit tests: NUnit for core logic (deterministic simulation, evolution engine).
- Integration tests: run headless simulations to verify variant metrics.
- Keep tests fast and deterministic.

Version Control / Repo rules
- Single repo monorepo for game code and tools.
- `main` branch protected; use feature branches and PRs.
- Commit messages: `TYPE(scope): short description` (e.g., `feat(world): add evolution path seed logic`).

Builds & CI
- CI runs linters, unit tests, and headless simulation smoke tests on PR.
- Automated artifact builds for engine project (Unity CLI build steps).

