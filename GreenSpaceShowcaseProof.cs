using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MetaVerseGreenSpacesVR
{
    /// <summary>
    /// End-to-End Showcase Verification Runner for MetaVerse GreenSpaces VR.
    /// Produces a structured, auditable execution report covering simulation state,
    /// carbon estimation, AI/offline fallback behavior, and report serialization.
    /// </summary>
    public class GreenSpaceShowcaseProof : MonoBehaviour
    {
        [Header("Scene References (Auto-Resolved if Left Empty)")]
        [SerializeField] private GreenSpaceManager coreManager;
        [SerializeField] private GreenSpaceUnifiedBridge unifiedBridge;
        [SerializeField] private EcologicalReportUploader reportUploader;
        [SerializeField] private GreenSpaceUIConnector uiConnector;

        [Header("Run Options")]
        [SerializeField] private bool runAutomaticallyOnStart = true;

        private bool reportReceived;

        private void Awake()
        {
            if (coreManager == null)
                coreManager = FindFirstObjectByType<GreenSpaceManager>();

            if (unifiedBridge == null)
                unifiedBridge = FindFirstObjectByType<GreenSpaceUnifiedBridge>();

            if (reportUploader == null)
                reportUploader = FindFirstObjectByType<EcologicalReportUploader>();

            if (uiConnector == null)
                uiConnector = FindFirstObjectByType<GreenSpaceUIConnector>();
        }

        private void OnEnable()
        {
            if (unifiedBridge != null)
            {
                unifiedBridge.onEcologicalReportUpdated.AddListener(OnReportUpdated);
            }
        }

        private void OnDisable()
        {
            if (unifiedBridge != null)
            {
                unifiedBridge.onEcologicalReportUpdated.RemoveListener(OnReportUpdated);
            }
        }

        private void OnReportUpdated(EcologicalReport report)
        {
            if (report != null)
            {
                reportReceived = true;
            }
        }

        private async void Start()
        {
            if (!runAutomaticallyOnStart) return;
            await RunShowcaseProof();
        }

        [ContextMenu("Run Showcase Proof")]
        public async Task RunShowcaseProof()
        {
            Stopwatch watch = Stopwatch.StartNew();

            if (coreManager == null)
            {
                Debug.LogError("[SHOWCASE PROOF] Missing GreenSpaceManager in active scene.");
                return;
            }

            float startCarbon = coreManager.RecalculateCarbonImpact();
            float startMoisture = coreManager.soilMoisture;
            string biomeLabel = coreManager.biomeName;

            if (unifiedBridge != null)
            {
                unifiedBridge.PlantSpecimen("Nitrogen-Fixing Acacia", true, 0.5f);
                unifiedBridge.ApplyCultivationAction(waterDelta: 15f, nutrientDelta: 10f, sunlightDelta: 5f);
                unifiedBridge.SetWeatherState(BiomeWeatherState.SeasonalRecovery);
                EcologicalReport report = unifiedBridge.PublishSynchronizedReport();

                if (reportUploader != null && report != null)
                {
                    reportUploader.UploadReportData(report);
                }
            }
            else
            {
                coreManager.PlantTree("Nitrogen-Fixing Acacia", 0.5f);
                coreManager.NurtureEcosystem(15f, 0.1f);
                coreManager.UpdateWeatherState(BiomeWeather.SeasonalRecovery);
            }

            float endCarbon = coreManager.RecalculateCarbonImpact();
            float endMoisture = coreManager.soilMoisture;
            BiomeWeather endWeather = coreManager.currentWeather;

            string mentorReply = unifiedBridge != null
                ? await unifiedBridge.AskEcologicalMentor("Summarize ecological health and next restoration action.")
                : await coreManager.QueryAICompanion("Summarize ecological health and next restoration action.");

            watch.Stop();

            bool checkCarbon = endCarbon > startCarbon;
            bool checkMoisture = Math.Abs(endMoisture - startMoisture) > 0.01f;
            bool checkWeather = endWeather == BiomeWeather.SeasonalRecovery;
            bool checkMentor = !string.IsNullOrWhiteSpace(mentorReply);

            Debug.Log("=================================================================");
            Debug.Log($"METAVERSE GREENSPACES VR — SHOWCASE VERIFICATION REPORT");
            Debug.Log($"Biome: {biomeLabel} | Duration: {watch.ElapsedMilliseconds} ms");
            Debug.Log($"1. Carbon Offset Delta : {startCarbon:F2} -> {endCarbon:F2} kg CO2/yr [Passed: {checkCarbon}]");
            Debug.Log($"2. Soil Moisture Delta : {startMoisture:F1}% -> {endMoisture:F1}% [Passed: {checkMoisture}]");
            Debug.Log($"3. Weather Transition  : {endWeather} [Passed: {checkWeather}]");
            Debug.Log($"4. AI / Fallback Output: {checkMentor} -> {mentorReply}");
            Debug.Log($"5. Module Wiring       : Bridge={unifiedBridge != null}, UI={uiConnector != null}, Uploader={reportUploader != null}, EventFired={reportReceived}");
            Debug.Log($"OVERALL STATUS         : {(checkCarbon && checkMoisture && checkWeather && checkMentor ? "ALL CORE CHECKS VERIFIED" : "PARTIAL — REVIEW LOGS")}");
            Debug.Log("=================================================================");
        }
    }
}
