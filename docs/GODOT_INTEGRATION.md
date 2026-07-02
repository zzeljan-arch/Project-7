# Godot Integration Guide

This repository now includes a minimal Godot 4 Mono project that uses the existing prototype code in `src/`.

## What is included

- `GodotProject/project.godot` - Godot project file with Mono enabled.
- `GodotProject/ProceduralGameGodot.csproj` - Godot C# project referencing `../src/ProceduralGame.csproj`.
- `GodotProject/Main.tscn` - a simple UI scene with a `Label`.
- `GodotProject/Scripts/PrototypeRunner.cs` - a Godot `Control` script that instantiates `PrototypeGameManager` and prints the prototype state.
- `ProceduralGame.sln` - root solution containing the simulation, prototype, and Godot projects.

## How it connects

- `GodotProject/ProceduralGameGodot.csproj` references `src/ProceduralGame.csproj`.
- `src/ProceduralGame.csproj` already references the simulation project in `Simulation/ProceduralGameSimulation.csproj`.
- The Godot script imports `PG.Systems` and uses `PrototypeGameManager`, so the data flow is shared instead of duplicated.

## Opening the project

1. Install Godot 4 with Mono support.
2. Install .NET 6 SDK.
3. Open `GodotProject/project.godot` in Godot.
4. If Godot asks to restore C# assemblies, allow it.
5. Set the main scene to `res://Main.tscn` if it is not already set.
6. Run the project.

## If the Godot C# project fails to restore

1. Make sure Godot 4 with Mono support is installed.
2. Open `GodotProject/project.godot` in the Godot editor.
3. Allow the editor to restore the C# project and build the assemblies.
4. If Godot still cannot resolve `Godot.NET.Sdk`, run:
   ```powershell
   dotnet restore GodotProject\ProceduralGameGodot.csproj
   ```
5. Reopen the project in Godot.

## What you should see

- A simple UI with prototype output text.
- The game manager state from `PrototypeGameManager` should display the current region, path, tier, enemy kills, loot, and quest progress.

## Next steps

- Replace the `Label` UI with Godot controls for actual gameplay.
- Add a `Scene` graph for region presentation and enemy interactions.
- Wire the Godot scene to the same `PrototypeGameManager` logic for live playtesting.
