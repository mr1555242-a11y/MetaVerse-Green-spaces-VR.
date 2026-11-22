# MetaVerse GreenSpaces VR

An interactive 3D virtual reality and simulation module built in Unity (C#) under the `MetaVerseGreenSpacesVR` namespace. It simulates biome reforestation, 3D tree growth, weather states with particle effects, carbon sequestration analytics, and live VR world-space UI updates.

## Key Features
- **3D Tree Planting & Scaling (`PlantTree3D`, `WaterTrees`):** Spawns 3D tree prefabs at designated planting spots, tracks growth progress and hydration levels, and dynamically scales 3D tree models as they grow.
- **Climate & Weather Simulation (`SetWeather`):** Supports `Clear`, `Rain`, `Drought`, and `SeasonalRecovery` states, playing or stopping a 3D rain `ParticleSystem` when rain is active.
- **Carbon Impact Analytics (`RecalculateCarbonImpact`):** Calculates individual and total carbon sequestered (`kg CO2`) based on active 3D tree growth.
- **VR World-Space UI (`UpdateVRDisplay`):** Updates a `TextMeshProUGUI` display with current biome name, weather state, active 3D tree count, and total carbon captured.

## Repository Structure
- `GreenSpaceManager.cs` — Core 3D simulation, weather controller, carbon calculator, and UI updater (`MetaVerseGreenSpacesVR` namespace).
- `README.md` — Project overview, setup instructions, and verification guide.

## Unity Setup
1. Attach `GreenSpaceManager.cs` to an empty `GameObject` in your Unity scene.
2. Assign your 3D tree model to **Tree Prefab** in the Inspector.
3. Add target `Transform` locations to **Planting Spots**.
4. Optionally assign a **Rain Particle FX** (`ParticleSystem`) and a **Status Text UI** (`TextMeshProUGUI`) on a World Space Canvas.
5. Enter **Play Mode** to verify initialization, 3D planting, watering/scaling, weather changes, carbon calculation, and UI updates in the Scene/Game view and Console.

## Verification Output
When Play Mode starts, the Console logs the simulation flow:
- `[MetaVerse VR] Initialized Biome: Emerald Canopy Sector`
- `[Reforestation] Planted 3D Nitrogen-Fixing Acacia at ... Total Trees: 1`
- `[Cultivation] Watered all active 3D trees and updated 3D scale.`
- `[Climate Engine] Weather changed to: Rain`
- `[Impact Analytics] Total Carbon Sequestered: ... kg CO2`
