## Code Overview (`GreenSpaceSimulation.cs`)
The project's simulation and visual-response logic lives inside the `MetaVerseGreenSpacesVR` namespace in Unity C#, divided into structured data models, core ecosystem management, and visual/event expansion:

* **Data Models & State Types:**
  * `BiomeWeather` defines four environment states: `Clear`, `Rain`, `Drought`, and `SeasonalRecovery`.
  * `PlantedTreeRecord` stores individual tree data (`speciesName`, `growthStage` from `0.0` to `1.0`, `healthScore` from `0` to `100`, and `annualCarbonKg`).
  * Serializable request and response classes (`GeminiRequestPayload`, `GeminiResponse`, and nested content/part classes) structure JSON serialization and response parsing cleanly.

* **`GreenSpaceManager` (Core Simulation & AI Companion):**
  * **Biome Initialization (`InitializeVirtualBiome`):** Starts the simulation, clears prior records, populates initial trees (`Nitrogen-Fixing Acacia` and `Native Rainforest Tree`), and calculates baseline carbon impact.
  * **Tree Planting & Cultivation (`PlantTree`, `NurtureEcosystem`):** Clamps growth/health values within safe bounds, factors soil moisture into new tree health, and updates each tree's estimated annual carbon absorption based on maturity (`BaseCarbonPerMatureTreeKg = 21.5f`), growth stage, and health score.
  * **Climate & Weather Transitions (`UpdateWeatherState`, `AdjustFloraForWeather`):** Updates soil moisture, ambient temperature, and tree health/growth across `Rain`, `Drought`, `SeasonalRecovery`, and `Clear` states.
  * **Carbon Analytics & AI Guidance (`RecalculateCarbonImpact`, `QueryAICompanion`, `BuildSafeGeminiJson`, `ExtractTextFromGeminiJson`):** Sums annual carbon estimates across active trees, provides local ecological advice when no API key is set or if a network request fails, and sends formatted prompts to the Gemini endpoint when configured.

* **`GreenSpaceExpansionModule` (Visuals, Carbon Scaling & Events):**
  * **Species-Specific Carbon Calculation (`CalculateSpeciesCarbon`):** Adjusts base carbon rates by species (`Acacia` vs. default) and applies a temperature stress multiplier when ambient temperature is above `34°C` or below `10°C`.
  * **Scene Spawning & Visual Feedback (`SpawnTreeVisual`, `ApplyWeatherAndCarbonVisuals`):** Instantiates tree prefabs within `spawnRadius` with randomized yaw rotation and growth-based scale, toggles rain and drought particle effects, adjusts directional light intensity, and invokes `UnityEvent` callbacks (`onCarbonStatsUpdated` and `onWeatherChanged`).
