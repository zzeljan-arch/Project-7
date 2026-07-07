Godot 4 C# Coding Standards & Naming Conventions

Goals
- Readability and predictability across growing teams
- Small, testable units of logic
- Clear module ownership and consistent use of Godot idioms

General rules
- Language: C# targeting .NET 6+ compatible with Godot 4's C# integration.
- Formatting: Use `dotnet format`. Keep lines <= 120 chars.
- Comments: Use XML doc comments for public C# APIs. Document exported `Resource`/Node properties for designers.
- TODOs: Use TODO/TICKET tags linking to issue tracker IDs.

Folder and namespace mapping
- `src/Core` -> namespace `PG.Core`.
- `src/World` -> namespace `PG.World`.
- `src/Gameplay` -> namespace `PG.Gameplay`.
- Keep namespaces matching folder structure.

Naming conventions
- Classes / Structs: PascalCase (e.g., `InventoryManager`, `RegionEvolutionEngine`).
- Interfaces: `I` prefix (e.g., `IService`, `IRegionProvider`).
- Methods: PascalCase (e.g., `ApplyDamage`).
- Fields: private `_camelCase` for non-exported fields; exported fields use `PascalCase` and `[Export]` attribute.
- Properties: PascalCase (e.g., `CurrentTier`).
- Signals/Events: use Godot `Signal` for node-scoped events; for global events use `On` prefix (e.g., `OnRegionEvolved`).
- Enums: PascalCase values (e.g., `WorldTier.Tier1`).

Godot-specific patterns
- Use `Resource` subclasses for data assets (equivalent to Unity's ScriptableObjects).
- Use `PackedScene` for actor prefabs.
- Use Autoload singletons for core services (`EventBus`, `SaveService`, `TimeService`).
- Keep engine-dependent code inside Godot `Node` scripts; keep core logic as POCO C# classes for easy unit testing.

Component & System design
- Favor small `Node`/`Node2D` subclasses and POCO C# classes where possible.
- Keep behavior in systems (C# classes) that operate on data models; keep scene scripts thin and designer-friendly.

Data-driven approach
- Use `Resource` and `PackedScene` for designer-facing content. Keep JSON/CSV importers for batch content ingestion.

Events & Messaging
- Implement a C# `IEventBus` for server-side decoupling. Use Godot `Signal` for scene-level events and UI.

Dependency injection
- Godot lacks a built-in DI system; use a simple Service Locator for Autoload singletons or integrate a lightweight DI container if desired.

Testing
- Unit tests: use `dotnet test` for pure C# modules. Keep simulation core engine-agnostic for testability.
- Integration tests: use Godot's headless runs or Godot Test Runner for smoke tests.

Version Control / Repo rules
- Single repo monorepo for game code and tools.
- `main` branch protected; use feature branches and PRs.
- Commit messages: `TYPE(scope): short description` (e.g., `feat(world): add evolution path seed logic`).

Builds & CI
- CI runs `dotnet format`, `dotnet test`, and headless Godot smoke tests on PR.
- Use Godot CLI for headless export and automated server builds.
