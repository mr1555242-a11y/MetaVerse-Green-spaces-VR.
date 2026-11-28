# Metaverse Green Spaces VR

**Metaverse Green Spaces VR** is an interactive virtual reality experience built with **Unity**, **C#**, and the **XR Interaction Toolkit** to transform environmental restoration from an abstract concept into a clear, step-by-step simulation.

## Simulation & Demo Flow

- **Starting State (Degraded Environment):** Begins with parched, cracked earth and sparse shrubs, showing **25°C** temperature, **15%** soil moisture, and **1%** green coverage.
- **Reforestation & Water Renewal:** Young saplings grow into a denser forest canopy while rainfall and a clear stream return to the environment.
- **Step-by-Step Hydration Progression:** Real-time metrics track hydration climbing from **40% → 55% → 95% → 100%**, updating **Water Quality**, **Air Purity**, and **Biodiversity** indicators.
- **Thriving Ecosystem:** Reaches **Habitat Balance: Optimal** and **"Ecosystem Status: Thriving,"** featuring clear water with swimming fish, blooming flowers, and a blue butterfly.

## Step-by-Step Development, Challenges & Solutions

1. **Degraded Baseline & Environmental Indicators**
   - **What We Built:** Set up the initial dry terrain and live readouts for `25°C`, `15%` soil moisture, and `1%` green coverage.
   - **Challenge:** Showing environmental stress clearly without cluttering the view.
   - **Solution:** Structured initial state presets (`EcosystemPresets.InitialDegradedState`) paired with clean UI/HUD readouts.
2. **Forest Growth, Rainfall & Stream Flow**
   - **What We Built:** Added progressive canopy scaling, rainfall particle effects, and clear stream activation.
   - **Challenge:** Keeping visual transitions and frame performance smooth during changes.
   - **Solution:** Used time-based coroutine interpolation (`Time.deltaTime`) and modular toggles for scene and particle objects.
3. **Hydration Milestones & Thriving Habitat**
   - **What We Built:** Progressed hydration through `40% → 55% → 95% → 100%` and enabled biodiversity elements (`swimming fish`, `blooming flowers`, `blue butterfly`) at **Habitat Balance: Optimal** and **"Ecosystem Status: Thriving."**
   - **Challenge:** Keeping metric numbers and environmental visuals aligned at every milestone.
   - **Solution:** Centralised milestone values and UI updates inside `RestorationController`.

## Current Role, Future Vision & Global Impact

- **Current Role:** Serves as an interactive educational and awareness simulation that helps learners observe cause-and-effect ecological recovery firsthand.
- **Future Vision:** Can expand into schools, environmental training sessions, and sustainability workshops with additional biomes and localized environmental scenarios.
- **Potential Global Impact:** Greater awareness and understanding of land care, reforestation, and water renewal can encourage more responsible environmental habits and long-term ecological stewardship.

## Technical Overview

- **Engine & Language:** Unity, C#
- **XR Framework:** XR Interaction Toolkit
- **Key Scripts:**
  - `Assets/Scripts/EcosystemState.cs` — Data structures and preset states (`InitialDegradedState`, `ThrivingState`).
  - `Assets/Scripts/RestorationController.cs` — Step-by-step restoration sequence, hydration milestones, visual updates, and HUD readouts.
  - `Assets/Scripts/VRRestorationTrigger.cs` — XR interaction event listener to start or reset the simulation.

## Setup & Usage

1. Open the project in a compatible **Unity** version with **XR Interaction Toolkit** and **TextMeshPro** installed.
2. Load the main VR simulation scene with terrain, canopy, rain/stream objects, biodiversity elements, and UI references configured.
3. Assign the scene references in the Inspector for `RestorationController` and `VRRestorationTrigger`, then run in Play Mode or deploy to your XR target.
