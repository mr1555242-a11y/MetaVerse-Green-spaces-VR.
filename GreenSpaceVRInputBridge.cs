

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

