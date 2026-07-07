Engine Decision — Godot 4 (C#)

Decision: Build this project using Godot 4 with C# for core systems.

Why Godot 4 with C#?
- Lightweight and fast iteration for 2D/2.5D games.
- Scene-based workflow with `PackedScene` and `Resource` fits the data-driven approach.
- C# support in Godot 4 (dotnet 6+) enables robust, testable server and simulation code.
- Open-source, friendly tooling and easy integration with external tools for content pipelines.

Networking approach
- Server-authoritative model: dedicated server runs authoritative Evolution Engine and gameplay rules.
- Transport: use ENet (Godot's default) or a supported third-party transport for reliability.
- Sync model: replicate minimal necessary state; use deterministic simulation for world evolution and reconcile via snapshots for gameplay state.

Prototype workflow
1. Rapid iteration: prototype evolution rules and statistical tests in Python or standalone C# (dotnet). Use these prototypes to tune probabilities and variance metrics.
2. Port stable logic into Godot C# modules under `src/World` for runtime and integration with `Resource` data.
3. Keep designer-facing data in `Resource` subclasses or JSON importers so content teams iterate without recompiling C# code.

Tools & pipeline recommendations
- Use `dotnet` tooling for C# builds and unit tests.
- Use Godot CLI for headless server runs and automated exports.
- Create Editor plugins (C# EditorScripts) and custom inspectors to simplify content authoring.

Tradeoffs & mitigations
- Godot networking is less full-featured than some engines: mitigate with careful server design and small, testable networking layers.
- C# in Godot requires managing both Godot lifecycle and .NET lifetimes—write pure C# core logic where possible and keep Godot-specific glue thin.

Next step
- Implement a headless deterministic world-evolution simulator in C# (recommended) or Python (for faster iteration). Confirm preference to begin.