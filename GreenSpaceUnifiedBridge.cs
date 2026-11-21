using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace MetaVerseGreenSpacesVR
{
    /// <summary>
    /// Compatibility & Telemetry Bridge:
    /// Connects existing GreenSpaceManager, GreenSpaceVRInputBridge, GreenSpaceUIConnector,
    /// and EcologicalReportUploader scripts without deleting or rewriting prior files.
    /// </summary>
    public enum BiomeWeatherState
    {
        Clear = BiomeWeather.Clear,
        Rain = BiomeWeather.Rain,
        Drought = BiomeWeather.Drought,
        SeasonalRecovery = BiomeWeather.SeasonalRecovery
    }

    [Serializable]
    public class BiomeResourceSnapshot
    {
        public float waterLevel = 60f;
        public float soilNutrients = 70f;
        public float sunlightLevel = 80f;
        public float temperatureC = 24f;
    }

    [Serializable]
    public class EcologicalReport
    {
        public string biomeName = "Emerald Canopy Sector";
        public string weather = "Clear";
        public int totalFloraCount = 0;
        public int speciesDiversityCount = 2;
        public float averageVitality = 90f;
        public float estimatedAnnualCarbonKg = 0f;
        public BiomeResourceSnapshot resources = new BiomeResourceSnapshot();
        public string timestampUtc = "";
    }

    [Serializable]
    public class EcologicalReportEvent : UnityEvent<EcologicalReport> { }

    [DisallowMultipleComponent]
    public class GreenSpaceUnifiedBridge : MonoBehaviour
    {
        [Header("Core References")]
        [SerializeField] private GreenSpaceManager coreManager;
        [SerializeField] private GreenSpaceExpansionModule expansionModule;

        [Header("Simulated Resource Tracking")]
        [Range(0f, 100f)] [SerializeField] private float soilNutrients = 75f;
        [Range(0f, 100f)] [SerializeField] private float sunlightLevel = 85f;
        [SerializeField] private int trackedFloraCount = 50;
        [SerializeField] private float trackedVitality = 90f;

        [Header("Unified Telemetry Events")]
        public EcologicalReportEvent onEcologicalReportUpdated = new EcologicalReportEvent();
        public UnityEvent<string> onMentorReplyReady = new UnityEvent<string>();

        private void Awake()
        {
            if (coreManager == null)
                coreManager = FindFirstObjectByType<GreenSpaceManager>();

            if (expansionModule == null)
                expansionModule = FindFirstObjectByType<GreenSpaceExpansionModule>();
        }

        private void Start()
        {
            if (coreManager != null)
            {
                trackedFloraCount = Mathf.Max(0, coreManager.initialTreeCount);
            }
            PublishSynchronizedReport();
        }

        // Matches GreenSpaceVRInputBridge -> PlantSpecimen(...)
        public void PlantSpecimen(string species, bool nitrogenFixing, float initialGrowth)
        {
            if (coreManager == null) return;

            string finalSpecies = nitrogenFixing && string.IsNullOrWhiteSpace(species)
                ? "Nitrogen-Fixing Acacia"
                : species;

            coreManager.PlantTree(finalSpecies, initialGrowth);
            trackedFloraCount++;

            if (nitrogenFixing)
            {
                soilNutrients = Mathf.Clamp(soilNutrients + 6f, 0f, 100f);
            }

            if (expansionModule != null)
            {
                expansionModule.SpawnTreeVisual(initialGrowth);
            }

            PublishSynchronizedReport();
        }

        // Matches GreenSpaceVRInputBridge -> ApplyCultivationAction(...)
        public void ApplyCultivationAction(float waterDelta, float nutrientDelta, float sunlightDelta)
        {
            if (coreManager == null) return;

            soilNutrients = Mathf.Clamp(soilNutrients + nutrientDelta, 0f, 100f);
            sunlightLevel = Mathf.Clamp(sunlightLevel + sunlightDelta, 0f, 100f);
            trackedVitality = Mathf.Clamp(trackedVitality + (waterDelta * 0.2f) + (nutrientDelta * 0.15f), 0f, 100f);

            float growthBoost = Mathf.Clamp((nutrientDelta + sunlightDelta) * 0.005f, 0.02f, 0.2f);
            coreManager.NurtureEcosystem(waterDelta, growthBoost);

            PublishSynchronizedReport();
        }

        // Matches GreenSpaceVRInputBridge -> SetWeatherState(...)
        public void SetWeatherState(BiomeWeatherState state)
        {
            if (coreManager == null) return;

            BiomeWeather mappedWeather = (BiomeWeather)state;
            coreManager.UpdateWeatherState(mappedWeather);

            switch (mappedWeather)
            {
                case BiomeWeather.Rain:
                    sunlightLevel = Mathf.Clamp(sunlightLevel - 15f, 20f, 100f);
                    trackedVitality = Mathf.Clamp(trackedVitality + 4f, 0f, 100f);
                    break;
                case BiomeWeather.Drought:
                    sunlightLevel = Mathf.Clamp(sunlightLevel + 15f, 0f, 100f);
                    trackedVitality = Mathf.Clamp(trackedVitality - 10f, 0f, 100f);
                    break;
                case BiomeWeather.SeasonalRecovery:
                    sunlightLevel = 80f;
                    trackedVitality = Mathf.Clamp(trackedVitality + 7f, 0f, 100f);
                    break;
                case BiomeWeather.Clear:
                default:
                    sunlightLevel = 85f;
                    trackedVitality = Mathf.Clamp(trackedVitality + 2f, 0f, 100f);
                    break;
            }

            PublishSynchronizedReport();
        }

        // Matches GreenSpaceVRInputBridge & GreenSpaceUIConnector -> AskEcologicalMentor(...)
        public async Task<string> AskEcologicalMentor(string prompt)
        {
            if (coreManager == null)
                return "[AI Companion] Core simulation manager is not active in the scene.";

            string reply = await coreManager.QueryAICompanion(prompt);
            onMentorReplyReady?.Invoke(reply);
            return reply;
        }

        // Generates and broadcasts a complete EcologicalReport for UI & Cloud Uploader
        public EcologicalReport PublishSynchronizedReport()
        {
            float carbonKg = coreManager != null ? coreManager.RecalculateCarbonImpact() : 0f;

            EcologicalReport report = new EcologicalReport
            {
                biomeName = coreManager != null ? coreManager.biomeName : "Emerald Canopy Sector",
                weather = coreManager != null ? coreManager.currentWeather.ToString() : "Clear",
                totalFloraCount = trackedFloraCount,
                speciesDiversityCount = 2,
                averageVitality = trackedVitality,
                estimatedAnnualCarbonKg = carbonKg,
                timestampUtc = DateTime.UtcNow.ToString("o"),
                resources = new BiomeResourceSnapshot
                {
                    waterLevel = coreManager != null ? coreManager.soilMoisture : 60f,
                    soilNutrients = soilNutrients,
                    sunlightLevel = sunlightLevel,
                    temperatureC = coreManager != null ? coreManager.ambientTemperatureC : 24f
                }
            };

            if (expansionModule != null && coreManager != null)
            {
                expansionModule.ApplyWeatherAndCarbonVisuals(
                    coreManager.currentWeather,
                    carbonKg,
                    trackedFloraCount
                );
            }

            onEcologicalReportUpdated?.Invoke(report);
            return report;
        }
    }
}
