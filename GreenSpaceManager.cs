using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MetaVerseGreenSpacesVR
{
    public enum BiomeWeather
    {
        Clear,
        Rain,
        Drought,
        SeasonalRecovery
    }

    [Serializable]
    public class PlantedTreeRecord
    {
        public string speciesName;
        public float growthStage;      // 0.0 (Seed) to 1.0 (Mature)
        public float healthScore;      // 0 to 100
        public float annualCarbonKg;   // Estimated annual CO2 absorption
    }

    [Serializable]
    public class GeminiResponse
    {
        public GeminiCandidate[] candidates;
    }

    [Serializable]
    public class GeminiCandidate
    {
        public GeminiContent content;
    }

    [Serializable]
    public class GeminiContent
    {
        public GeminiPart[] parts;
    }

    [Serializable]
    public class GeminiPart
    {
        public string text;
    }

    [Serializable]
    public class GeminiRequestPayload
    {
        public GeminiRequestContent[] contents;
    }

    [Serializable]
    public class GeminiRequestContent
    {
        public GeminiRequestPart[] parts;
    }

    [Serializable]
    public class GeminiRequestPart
    {
        public string text;
    }

    public class GreenSpaceManager : MonoBehaviour
    {
        [Header("Biome & Ecosystem Configuration")]
        public string biomeName = "Emerald Canopy Sector";
        public int initialTreeCount = 50;
        [Range(0f, 100f)] public float soilMoisture = 60f;
        public float ambientTemperatureC = 24f;
        public BiomeWeather currentWeather = BiomeWeather.Clear;

        [Header("AI Companion Settings")]
        [Tooltip("Leave empty for offline ecological guidance.")]
        [SerializeField] private string geminiApiKey = "";
        private const string GeminiModelUrl =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        private readonly List<PlantedTreeRecord> activeTrees = new List<PlantedTreeRecord>();
        private bool isSimulationActive = false;
        private const float BaseCarbonPerMatureTreeKg = 21.5f;

        public int ActiveTreeCount => activeTrees.Count;

        private void Start()
        {
            InitializeVirtualBiome();
        }

        public void InitializeVirtualBiome()
        {
            isSimulationActive = true;
            activeTrees.Clear();
            int safeCount = Mathf.Max(0, initialTreeCount);

            for (int i = 0; i < safeCount; i++)
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

        public void PlantTree(string speciesName = "Native Rainforest Tree", float initialGrowth = 0.25f)
        {
            if (!isSimulationActive)
            {
                InitializeVirtualBiome();
            }

            float clampedGrowth = Mathf.Clamp01(initialGrowth);
            float moistureBonus = Mathf.Clamp01(soilMoisture / 70f);
            float initialHealth = Mathf.Clamp(65f + (moistureBonus * 35f), 20f, 100f);

            PlantedTreeRecord newTree = new PlantedTreeRecord
            {
                speciesName = string.IsNullOrWhiteSpace(speciesName) ? "Native Rainforest Tree" : speciesName,
                growthStage = clampedGrowth,
                healthScore = initialHealth,
                annualCarbonKg = BaseCarbonPerMatureTreeKg * clampedGrowth * (initialHealth / 100f)
            };

            activeTrees.Add(newTree);
            RecalculateCarbonImpact();
            Debug.Log($"[Reforestation] Planted '{newTree.speciesName}'. Total trees: {activeTrees.Count}");
        }

        public void NurtureEcosystem(float waterAdded = 15f, float growthBoost = 0.1f)
        {
            soilMoisture = Mathf.Clamp(soilMoisture + waterAdded, 0f, 100f);

            foreach (PlantedTreeRecord tree in activeTrees)
            {
                tree.healthScore = Mathf.Clamp(tree.healthScore + (waterAdded * 0.3f), 0f, 100f);
                tree.growthStage = Mathf.Clamp01(tree.growthStage + growthBoost);
                tree.annualCarbonKg = BaseCarbonPerMatureTreeKg * tree.growthStage * (tree.healthScore / 100f);
            }

            RecalculateCarbonImpact();
            Debug.Log($"[Cultivation] Soil moisture now {soilMoisture:F1}%. Flora updated.");
        }

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
            foreach (PlantedTreeRecord tree in activeTrees)
            {
                tree.healthScore = Mathf.Clamp(tree.healthScore + healthDelta, 0f, 100f);
                tree.growthStage = Mathf.Clamp01(tree.growthStage + growthDelta);
                tree.annualCarbonKg = BaseCarbonPerMatureTreeKg * tree.growthStage * (tree.healthScore / 100f);
            }
        }

        public float RecalculateCarbonImpact()
        {
            float totalCarbon = 0f;
            foreach (PlantedTreeRecord tree in activeTrees)
            {
                totalCarbon += tree.annualCarbonKg;
            }

            Debug.Log($"[Impact Analytics] Trees: {activeTrees.Count} | Estimated Annual Carbon Offset: {totalCarbon:F1} kg CO2/year");
            return totalCarbon;
        }

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
            string jsonBody = BuildSafeGeminiJson(prompt);

            using (UnityWebRequest request = new UnityWebRequest(GeminiModelUrl, "POST"))
            {
                byte[] payload = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(payload);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("x-goog-api-key", geminiApiKey.Trim());

                UnityWebRequestAsyncOperation op = request.SendWebRequest();
                while (!op.isDone)
                {
                    if (this == null)
                    {
                        return string.Empty;
                    }
                    await Task.Yield();
                }

                if (this == null)
                {
                    return string.Empty;
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string parsed = ExtractTextFromGeminiJson(request.downloadHandler.text);
                    return string.IsNullOrWhiteSpace(parsed) ? request.downloadHandler.text : parsed;
                }

                return $"[AI Companion Fallback] {statusContext} Could not reach live API ({request.error}); local guidance active.";
            }
        }

        public string BuildSafeGeminiJson(string promptText)
        {
            GeminiRequestPayload payload = new GeminiRequestPayload
            {
                contents = new[]
                {
                    new GeminiRequestContent
                    {
                        parts = new[] { new GeminiRequestPart { text = promptText } }
                    }
                }
            };
            return JsonUtility.ToJson(payload);
        }

        private string ExtractTextFromGeminiJson(string rawJson)
        {
            if (string.IsNullOrWhiteSpace(rawJson))
            {
                return string.Empty;
            }

            try
            {
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(rawJson);
                if (response?.candidates != null && response.candidates.Length > 0)
                {
                    GeminiPart[] parts = response.candidates[0].content?.parts;
                    if (parts != null && parts.Length > 0)
                    {
                        return parts[0].text;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AI Companion] JSON parse warning: {ex.Message}");
            }

            return string.Empty;
        }
    }

    public class GreenSpaceExpansionModule : MonoBehaviour
    {
        [Header("VR Visuals & Weather Effects")]
        public GameObject treePrefab;
        public Transform forestRoot;
        public float spawnRadius = 25f;
        public ParticleSystem rainEffect;
        public ParticleSystem droughtDustEffect;
        public Light directionalSunLight;

        [Header("VR Dashboard Events")]
        public UnityEvent<float, int> onCarbonStatsUpdated;
        public UnityEvent<BiomeWeather> onWeatherChanged;

        public float CalculateSpeciesCarbon(string speciesName, float growthStage, float healthScore, float ambientTempC)
        {
            float baseRateKg = (!string.IsNullOrEmpty(speciesName) && speciesName.Contains("Acacia")) ? 18.2f : 22.8f;
            float tempStressFactor = (ambientTempC > 34f || ambientTempC < 10f) ? 0.75f : 1.0f;
            return baseRateKg * Mathf.Clamp01(growthStage) * (Mathf.Clamp(healthScore, 0f, 100f) / 100f) * tempStressFactor;
        }

        public GameObject SpawnTreeVisual(float growthStage)
        {
            if (treePrefab == null)
            {
                return null;
            }

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = (forestRoot != null ? forestRoot.position : transform.position) +
                               new Vector3(randomCircle.x, 0f, randomCircle.y);

            GameObject instance = Instantiate(
                treePrefab,
                spawnPos,
                Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f),
                forestRoot
            );
            instance.transform.localScale = Vector3.one * Mathf.Lerp(0.35f, 1.35f, Mathf.Clamp01(growthStage));
            return instance;
        }

        public void ApplyWeatherAndCarbonVisuals(BiomeWeather weather, float totalCarbonKg, int treeCount)
        {
            if (rainEffect != null)
            {
                if (weather == BiomeWeather.Rain)
                {
                    rainEffect.Play();
                }
                else
                {
                    rainEffect.Stop();
                }
            }

            if (droughtDustEffect != null)
            {
                if (weather == BiomeWeather.Drought)
                {
                    droughtDustEffect.Play();
                }
                else
                {
                    droughtDustEffect.Stop();
                }
            }

            if (directionalSunLight != null)
            {
                directionalSunLight.intensity = weather == BiomeWeather.Rain ? 0.6f :
                                                weather == BiomeWeather.Drought ? 1.45f : 1.0f;
            }

            onWeatherChanged?.Invoke(weather);
            onCarbonStatsUpdated?.Invoke(totalCarbonKg, treeCount);
        }
    }
}
