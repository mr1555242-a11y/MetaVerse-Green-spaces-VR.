using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MetaVerseGreenSpacesVR
{
    /// <summary>
    /// Final End-to-End Execution & Assertion Proof for MetaVerse GreenSpaces VR.
    /// Validates state transitions, carbon calculations, event flow, and AI/offline fallback.
    /// </summary>
    public class GreenSpaceFinalProofRunner : MonoBehaviour
    {
        [Header("Scene Components (Auto-Resolved if Left Empty)")]
        [SerializeField] private GreenSpaceManager coreManager;
        [SerializeField] private GreenSpaceUnifiedBridge unifiedBridge;
        [SerializeField] private EcologicalReportUploader reportUploader;
        [SerializeField] private GreenSpaceUIConnector uiConnector;

        [Header("Execution Settings")]
        [SerializeField] private bool runOnSceneStart = true;

        private bool reportEventFired = false;

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
                unifiedBridge.onEcologicalReportUpdated.AddListener(HandleReportFired);
            }
        }

        private void OnDisable()
        {
            if (unifiedBridge != null)
            {
                unifiedBridge.onEcologicalReportUpdated.RemoveListener(HandleReportFired);
            }
        }

        private void HandleReportFired(EcologicalReport report)
        {
            if (report != null)
            {
                reportEventFired = true;
            }
        }

        private async void Start()
        {
            if (!runOnSceneStart) return;
            await ExecuteFinalProof();
        }

        [ContextMenu("Execute Final Proof Now")]
        public async Task ExecuteFinalProof()
        {
            Stopwatch timer = Stopwatch.StartNew();

            if (coreManager == null)
            {
                Debug.LogError("[FINAL PROOF FAILED] GreenSpaceManager missing in scene.");
                return;
            }

            Debug.Log("==========================================================");
            Debug.Log("[MetaVerse GreenSpaces VR] FINAL EXECUTION PROOF STARTED");
            Debug.Log($"Timestamp (UTC): {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            Debug.Log($"Unity Version: {Application.unityVersion} | Platform: {Application.platform}");
            Debug.Log("==========================================================");

            // 1. Baseline state & carbon check
            int initialCount = Math.Max(0, coreManager.initialTreeCount);
            float initialCarbon = coreManager.RecalculateCarbonImpact();
            float initialMoisture = coreManager.soilMoisture;

            Debug.Log($"[PROOF 1 - BASELINE] Biome='{coreManager.biomeName}', InitialTrees={initialCount}, Moisture={initialMoisture:F1}%, Carbon={initialCarbon:F2} kg CO2/yr");

            // 2. Action execution: Plant specimen + Cultivate + Weather shift
            if (unifiedBridge != null)
            {
                unifiedBridge.PlantSpecimen("Nitrogen-Fixing Acacia", true, 0.45f);
                unifiedBridge.ApplyCultivationAction(waterDelta: 15f, nutrientDelta: 10f, sunlightDelta: 5f);
                unifiedBridge.SetWeatherState(BiomeWeatherState.SeasonalRecovery);
                unifiedBridge.PublishSynchronizedReport();
            }
            else
            {
                coreManager.PlantTree("Nitrogen-Fixing Acacia", 0.45f);
                coreManager.NurtureEcosystem(15f, 0.1f);
                coreManager.UpdateWeatherState(BiomeWeather.SeasonalRecovery);
            }

            float postCarbon = coreManager.RecalculateCarbonImpact();
            float postMoisture = coreManager.soilMoisture;
            BiomeWeather postWeather = coreManager.currentWeather;

            bool carbonIncreased = postCarbon > initialCarbon;
            bool moistureUpdated = Math.Abs(postMoisture - initialMoisture) > 0.01f;
            bool weatherUpdated = postWeather == BiomeWeather.SeasonalRecovery;

            Debug.Log($"[PROOF 2 - STATE DELTA] Weather={postWeather} (Expected=SeasonalRecovery -> {weatherUpdated}), " +
                      $"Moisture={initialMoisture:F1}% -> {postMoisture:F1}% (Changed={moistureUpdated}), " +
                      $"Carbon={initialCarbon:F2} -> {postCarbon:F2} kg/yr (Increased={carbonIncreased})");

            // 3. AI Mentor / Deterministic Offline Fallback check
            string mentorOutput = unifiedBridge != null
                ? await unifiedBridge.AskEcologicalMentor("Provide a concise restoration check for this biome.")
                : await coreManager.QueryAICompanion("Provide a concise restoration check for this biome.");

            bool mentorResponded = !string.IsNullOrWhiteSpace(mentorOutput);
            Debug.Log($"[PROOF 3 - AI / FALLBACK] Responded={mentorResponded} | Output={mentorOutput}");

            // 4. Component wiring & event verification
            bool bridgeWired = unifiedBridge != null;
            bool uploaderWired = reportUploader != null;
            bool uiWired = uiConnector != null;

            Debug.Log($"[PROOF 4 - MODULAR WIRING] Bridge={bridgeWired}, ReportUploader={uploaderWired}, UIConnector={uiWired}, ReportEventFired={reportEventFired}");

            timer.Stop();
            bool allCoreChecksPassed = carbonIncreased && moistureUpdated && weatherUpdated && mentorResponded;

            Debug.Log("==========================================================");
            Debug.Log($"FINAL VERDICT: {(allCoreChecksPassed ? "VERIFIED & PASSED (100% CORE FLOW)" : "CHECK WARNINGS ABOVE")}");
            Debug.Log($"Total Execution Time: {timer.ElapsedMilliseconds} ms");
            Debug.Log("==========================================================");
        }
    }
}
