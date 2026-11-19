
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MetaVerseGreenSpacesVR
{
    public enum BiomeWeather { Clear, Rain, Drought, SeasonalRecovery }

    [Serializable]
    public class PlantedTreeRecord
    {
        public string speciesName;
        public float growthStage;      // 0.0 (Seed) to 1.0 (Mature)
        public float healthScore;      // 0 to 100
        public float annualCarbonKg;   // Estimated annual CO2 absorption
    }

    public class GreenSpaceManager : MonoBehaviour
    {
        [Header("Biome & Ecosystem Configuration")]
        public string biomeName = "Emerald Canopy Sector";
        public int initialTreeCount = 50;
        public float soilMoisture = 60f;          // 0% to 100%
        public float ambientTemperatureC = 24f;   // Celsius
        public BiomeWeather currentWeather = BiomeWeather.Clear;

        [Header("AI Companion Settings")]
        [SerializeField] private string geminiApiKey = "";
        private const string GeminiEndpoint =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        private readonly List<PlantedTreeRecord> activeTrees = new List<PlantedTreeRecord>();
        private bool isSimulationActive = false;
        private const float BaseCarbonPerMatureTreeKg = 21.5f;

        private void Start()
        {
            InitializeVirtualBiome();
        }

        // 1. Immersion & Biome Initialization
        public void InitializeVirtualBiome()
        {
            isSimulationActive = true;
            activeTrees.Clear();

            for (int i = 0; i < initialTreeCount; i++)
            {
                activeTrees.Add(new PlantedTreeRecord
                {
                    speciesName = (i % 3 == 0) ? "Nitrogen-Fixing Acacia" : "Native Rainforest Tree",
                    growthStage = 0.6f,
                    healthScore = 90f,
                    annualCarbonKg = BaseCarbonPerMatureTreeKg * 0.6f
                });
            }

            RecalculateCarbonImpact();
            Debug.Log($"[MetaVerse VR] Biome '{biomeName}' initialized with {activeTrees.Count} trees.");
        }

        // 2. Interactive Reforestation & Cultivation
        public void PlantTree(string speciesName = "Native Rainforest Tree", float initialGrowth = 0.25f)
        {
            if (!isSimulationActive) InitializeVirtualBiome();

            float moistureBonus = Mathf.Clamp01(soilMoisture / 70f);
            float initialHealth = Mathf.Clamp(65f + (moistureBonus * 35f), 20f, 100f);

            PlantedTreeRecord newTree = new PlantedTreeRecord
            {
                speciesName = speciesName,
                growthStage = Mathf.Clamp01(initialGrowth),
                healthScore = initialHealth,
                annualCarbonKg = BaseCarbonPerMatureTreeKg * Mathf.Clamp01(initialGrowth)
            };

            activeTrees.Add(newTree);
            RecalculateCarbonImpact();
            Debug.Log($"[Reforestation] Planted '{speciesName}'. Total trees: {activeTrees.Count}");
        }

        public void NurtureEcosystem(float waterAdded = 15f, float growthBoost = 0.1f)
        {
            soilMoisture = Mathf.Clamp(soilMoisture + waterAdded, 0f, 100f);

            foreach (var tree in activeTrees)
            {
                tree.healthScore = Mathf.Clamp(tree.healthScore + (waterAdded * 0.3f), 0f, 100f);
                tree.growthStage = Mathf.Clamp01(tree.growthStage + growthBoost);
                tree.annualCarbonKg = BaseCarbonPerMatureTreeKg * tree.growthStage * (tree.healthScore / 100f);
            }

            RecalculateCarbonImpact();
            Debug.Log($"[Cultivation] Soil moisture now {soilMoisture:F1}%. Flora updated.");
        }

        // 3. Real-Time Climate & Weather Simulation
        public void UpdateWeatherState(BiomeWeather newWeather)
        {
            currentWeather = newWeather;

            switch (currentWeather)
            {
                case BiomeWeather.Rain:
                    soilMoisture = Mathf.Clamp(soilMoisture + 25f, 0f, 100f);
                    ambientTemperatureC = 19f;
                    AdjustFloraForWeather(healthDelta: 5f, growthDelta: 0.08f);
                    break;

                case BiomeWeather.Drought:
                    soilMoisture = Mathf.Clamp(soilMoisture - 30f, 0f, 100f);
                    ambientTemperatureC = 37f;
                    AdjustFloraForWeather(healthDelta: -12f, growthDelta: -0.05f);
                    break;

                case BiomeWeather.SeasonalRecovery:
                    soilMoisture = Mathf.Clamp(soilMoisture + 10f, 0f, 100f);
                    ambientTemperatureC = 23f;
                    AdjustFloraForWeather(healthDelta: 8f, growthDelta: 0.12f);
                    break;

                case BiomeWeather.Clear:
                default:
                    ambientTemperatureC = 25f;
                    AdjustFloraForWeather(healthDelta: 2f, growthDelta: 0.04f);
                    break;
            }

            RecalculateCarbonImpact();
            Debug.Log($"[Climate Engine] Weather: {currentWeather} | Temp: {ambientTemperatureC}C | Moisture: {soilMoisture:F1}%");
        }

        private void AdjustFloraForWeather(float healthDelta, float growthDelta)
        {
            foreach (var tree in activeTrees)
            {
                tree.healthScore = Mathf.Clamp(tree.healthScore + healthDelta, 0f, 100f);
                tree.growthStage = Mathf.Clamp01(tree.growthStage + growthDelta);
                tree.annualCarbonKg = BaseCarbonPerMatureTreeKg * tree.growthStage * (tree.healthScore / 100f);
            }
        }

        // 4. Impact Bridging & Carbon Analytics
        public float RecalculateCarbonImpact()
        {
            float totalCarbon = 0f;
            foreach (var tree in activeTrees)
            {
                totalCarbon += tree.annualCarbonKg;
            }

            Debug.Log($"[Impact Analytics] Trees: {activeTrees.Count} | Estimated Annual Carbon Offset: {totalCarbon:F1} kg CO2/year");
            return totalCarbon;
        }

        // 5. AI Ecological Companion (Live Gemini API if key is set, otherwise contextual ecological guidance)
        public async Task<string> QueryAICompanion(string ecologicalQuery)
        {
            float totalCarbon = RecalculateCarbonImpact();
            string statusContext = $"Biome: {biomeName}, Weather: {currentWeather}, Soil Moisture: {soilMoisture:F1}%, " +
                                 $"Trees: {activeTrees.Count}, Annual Carbon Offset: {totalCarbon:F1} kg CO2/yr.";

            if (string.IsNullOrWhiteSpace(geminiApiKey))
            {
                if (soilMoisture < 35f || currentWeather == BiomeWeather.Drought)
                {
                    return $"[AI Companion] {statusContext} Soil moisture is low—recommend irrigating and planting drought-resilient or nitrogen-fixing flora.";
                }
                return $"[AI Companion] {statusContext} Ecosystem is stable—continue balanced reforestation and monitor seasonal shifts.";
            }

            string prompt = $"Act as an ecological mentor for MetaVerse GreenSpaces VR. Context: {statusContext}. Query: {ecologicalQuery}";
            string jsonBody = "{\"contents\":[{\"parts\":[{\"text\":\"" + EscapeJson(prompt) + "\"}]}]}";

            using (UnityWebRequest request = new UnityWebRequest(GeminiEndpoint + "?key=" + geminiApiKey, "POST"))
            {
                byte[] payload = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(payload);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var op = request.SendWebRequest();
                while (!op.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }

                return $"[AI Companion Fallback] {statusContext} Could not reach live API ({request.error}); local guidance active.";
            }
        }

        private string EscapeJson(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", " ");
        }
    }
}
