

using UnityEngine;

namespace MetaVerseGreenSpacesVR
{
    public class GreenSpaceVRInputBridge : MonoBehaviour
    {
        [Header("Target Manager")]
        [SerializeField] private GreenSpaceManager greenSpaceManager;

        [Header("Default Planting Settings")]
        [SerializeField] private string defaultSpecies = "Native Rainforest Tree";
        [SerializeField] private bool defaultNitrogenFixing = false;
        [Range(0f, 1f)] [SerializeField] private float defaultInitialGrowth = 0.25f;

        private void Awake()
        {
            if (greenSpaceManager == null)
            {
                greenSpaceManager = FindFirstObjectByType<GreenSpaceManager>();
            }
        }

        public void TriggerPlantTree()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.PlantSpecimen(defaultSpecies, defaultNitrogenFixing, defaultInitialGrowth);
        }

        public void TriggerPlantAcacia()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.PlantSpecimen("Nitrogen-Fixing Acacia", true, 0.3f);
        }

        public void TriggerCultivate()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.ApplyCultivationAction(waterDelta: 15f, nutrientDelta: 10f, sunlightDelta: 5f);
        }

        public void TriggerWeatherRain()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.SetWeatherState(BiomeWeatherState.Rain);
        }

        public void TriggerWeatherDrought()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.SetWeatherState(BiomeWeatherState.Drought);
        }

        public void TriggerWeatherRecovery()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.SetWeatherState(BiomeWeatherState.SeasonalRecovery);
        }

        public void TriggerWeatherClear()
        {
            if (greenSpaceManager == null) return;
            greenSpaceManager.SetWeatherState(BiomeWeatherState.Clear);
        }

        public async void TriggerAskMentor(string promptText = "What should we do next for this biome?")
        {
            if (greenSpaceManager == null) return;
            string reply = await greenSpaceManager.AskEcologicalMentor(promptText);
            Debug.Log($"[VR Mentor Response] {reply}");
        }
    }
}
*GreenSpaceVRInputBridge.cs (VR Input Bridge)
​To make this script work seamlessly in Unity, the following dependencies, events, and settings should be configured:
​Required Classes & Enum (in MetaVerseGreenSpacesVR namespace):
​BiomeWeatherState enum with values: Clear, Rain, Drought, and SeasonalRecovery.
​GreenSpaceManager (MonoBehaviour) containing:
​public void PlantSpecimen(string species, bool nitrogenFixing, float initialGrowth)
​public void ApplyCultivationAction(float waterDelta, float nutrientDelta, float sunlightDelta)
​public void SetWeatherState(BiomeWeatherState state)
​public async Task<string> AskEcologicalMentor(string prompt) (using System.Threading.Tasks)
​Scene Setup: Place an active GreenSpaceManager GameObject in the scene and a GameObject with GreenSpaceVRInputBridge attached, assigning the manager reference in the Inspector.
​Event Bindings:
​Bind TriggerPlantTree(), TriggerPlantAcacia(), TriggerCultivate(), TriggerWeatherRain(), TriggerWeatherDrought(), TriggerWeatherClear(), TriggerWeatherRecovery(), and TriggerAskMentor(string) to VR UI Button On Click () events, or to XRSimpleInteractable (Select Entered) / InputActionReference (Performed) events.
​Project Settings:
​Use a Unity version supporting FindFirstObjectByType (Unity 2023.1+ / Unity 6), include an EventSystem with XR UI Input Module for VR UI clicks, and enable Internet Access plus proper AI API/service setup for AskEcologicalMentor*
## Project Summary & Code Alignment

**MetaVerse GreenSpaces VR** is a Unity (C#) spatial simulation prototype focused on interactive reforestation, dynamic weather and soil-moisture states, estimated annual $\text{CO}_2$ offset tracking ($\text{kg CO}_2/\text{year}$), and AI-assisted ecological guidance with a deterministic offline fallback.

### Implemented Modules in the Codebase
- **`GreenSpaceManager`:** Initializes the virtual biome (`Emerald Canopy Sector`), tracks planted trees (`Native Rainforest Tree` and `Nitrogen-Fixing Acacia`), updates soil moisture, temperature, and weather states (`Clear`, `Rain`, `Drought`, `SeasonalRecovery`), calculates estimated annual carbon absorption, and queries the Gemini API with local fallback guidance when offline or unconfigured.
- **`GreenSpaceExpansionModule`:** Computes species- and temperature-adjusted carbon estimates, spawns tree prefabs within a configured radius, toggles rain/drought particle effects and directional light intensity, and dispatches `UnityEvent` updates.
- **`GreenSpaceUnifiedBridge`:** Synchronizes simulation state, resource metrics (`waterLevel`, `soilNutrients`, `sunlightLevel`, `temperatureC`), and `EcologicalReport` snapshots across UI, visual, and reporting components.
- **`GreenSpaceVRInputBridge` & `GreenSpaceUIConnector`:** Connect planting, cultivation, weather transitions, and mentor queries to Unity events and formatted status/mentor text outputs.
- **`EcologicalReportUploader`:** Listens for report updates and serializes `EcologicalReport` payloads into JSON for logging or external integration.
- **Verification Runners (`GreenSpaceSimulationVerification`, `GreenSpaceFinalProofRunner`, `GreenSpaceShowcaseProof`):** Execute runtime checks for baseline values, state/carbon changes, mentor/fallback responses, and module wiring.

### Scope & Future Roadmap
- **Current Scope:** Core C# ecosystem simulation, carbon estimation, event-driven UI/report wiring, AI/offline mentor logic, and automated runtime verification scripts.
- **Planned Expansion:** Deeper Unity scene/XR interaction integration, multiplayer sessions, cloud persistence, and biometric/environmental telemetry as each feature is implemented and verified.

