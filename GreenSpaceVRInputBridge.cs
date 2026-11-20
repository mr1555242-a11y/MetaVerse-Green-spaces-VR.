

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


