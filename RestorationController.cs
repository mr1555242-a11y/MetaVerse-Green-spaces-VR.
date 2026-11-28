using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MetaverseGreenSpacesVR
{
    public class RestorationController : MonoBehaviour
    {
        [Header("Environment & Visual Objects")]
        [SerializeField] private GameObject crackedEarthObject;
        [SerializeField] private GameObject sparseShrubsObject;
        [SerializeField] private Transform forestCanopyGroup;
        [SerializeField] private GameObject clearStreamObject;
        [SerializeField] private ParticleSystem rainfallParticles;
        [SerializeField] private GameObject biodiversityGroup; // Fish, blooming flowers, blue butterfly

        [Header("UI / HUD Readouts")]
        [SerializeField] private TMP_Text stageText;
        [SerializeField] private TMP_Text metricsText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Slider hydrationSlider;

        [Header("Transition Timing")]
        [SerializeField] private float stepDuration = 2.0f;

        private EcosystemSnapshot currentState;
        private readonly float[] hydrationMilestones = { 40f, 55f, 95f, 100f };

        // [Step 1 - Initialization & Degraded Start]
        // What we did: Initialized cracked earth, sparse shrubs, and baseline metrics on Start.
        // Challenge: Ensuring a clean reset before starting the demo sequence.
        // Solution: Created ApplyInitialState() to reset objects, coroutines, and UI cleanly.
        // Current & Future Role: Currently sets up a repeatable demo start; in the future, it helps
        // learners replay restoration steps in classrooms or workshops.
        private void Start()
        {
            ApplyInitialState();
        }

        public void ApplyInitialState()
        {
            StopAllCoroutines();
            currentState = EcosystemPresets.InitialDegradedState();

            if (crackedEarthObject != null) crackedEarthObject.SetActive(true);
            if (sparseShrubsObject != null) sparseShrubsObject.SetActive(true);
            if (forestCanopyGroup != null) forestCanopyGroup.localScale = Vector3.one * 0.1f;
            if (clearStreamObject != null) clearStreamObject.SetActive(false);
            if (rainfallParticles != null) rainfallParticles.Stop();
            if (biodiversityGroup != null) biodiversityGroup.SetActive(false);

            UpdateUI(currentState);
        }

        public void StartRestorationSequence()
        {
            StopAllCoroutines();
            StartCoroutine(RunRestorationRoutine());
        }

        // [Step 2 - Reforestation, Rainfall, Clear Stream & Hydration 40% -> 55% -> 95% -> 100%]
        // What we did: Progressed canopy growth, rainfall, clear stream, and hydration milestones
        // (40% -> 55% -> 95% -> 100%), ending with fish, blooming flowers, and a blue butterfly.
        // Challenge: Synchronizing visual environment updates with live metric changes smoothly.
        // Solution: Used coroutines and AnimateHydrationTo() with step-by-step milestone values.
        // Current & Future Role: Currently demonstrates visual cause-and-effect clearly; in the future,
        // it can support interactive restoration choices and expanded scenarios.
        private IEnumerator RunRestorationRoutine()
        {
            // Sub-step A: Reforestation, rainfall & clear stream -> 40% hydration
            currentState.stageLabel = "Reforestation & Water Return";
            if (rainfallParticles != null && !rainfallParticles.isPlaying) rainfallParticles.Play();
            if (clearStreamObject != null) clearStreamObject.SetActive(true);
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[0], 12f, 35f, 0.35f));

            // Sub-step B: Canopy & moisture growth -> 55% hydration
            currentState.soilMoisturePercent = 45f;
            currentState.greenCoveragePercent = 45f;
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[1], 30f, 55f, 0.60f));

            // Sub-step C: Dense forest & recovery metrics -> 95% hydration
            currentState.soilMoisturePercent = 75f;
            currentState.greenCoveragePercent = 82f;
            currentState.waterQualityPercent = 88f;
            currentState.airPurityPercent = 86f;
            currentState.habitatBalanceStatus = "Optimal / Stable";
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[2], 65f, 90f, 0.90f));

            // Sub-step D: Full hydration (100%) -> Habitat Balance: Optimal & Ecosystem Status: Thriving
            currentState = EcosystemPresets.ThrivingState();
            if (hydrationSlider != null) hydrationSlider.value = 100f;
            if (forestCanopyGroup != null) forestCanopyGroup.localScale = Vector3.one;
            if (biodiversityGroup != null) biodiversityGroup.SetActive(true);

            UpdateUI(currentState);
        }

        // [Step 3 - Smooth Interpolation & Live HUD Updates]
        // What we did: Interpolated hydration, water quality, air purity, and canopy scale over time.
        // Challenge: Preventing abrupt jumps or frame-rate dependent speed differences.
        // Solution: Used Mathf.Lerp with Time.deltaTime and Mathf.Clamp01.
        // Current & Future Role: Currently keeps demo transitions smooth and readable; in the future,
        // it supports adjustable pacing for educational sessions.
        private IEnumerator AnimateHydrationTo(float targetHydration, float targetWaterQ, float targetAirP, float canopyScale)
        {
            float elapsed = 0f;
            float startHydration = currentState.hydrationPercent;
            float startWaterQ = currentState.waterQualityPercent;
            float startAirP = currentState.airPurityPercent;
            float startScale = forestCanopyGroup != null ? forestCanopyGroup.localScale.x : 0.1f;

            while (elapsed < stepDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / stepDuration);

                currentState.hydrationPercent = Mathf.Lerp(startHydration, targetHydration, t);
                currentState.waterQualityPercent = Mathf.Lerp(startWaterQ, targetWaterQ, t);
                currentState.airPurityPercent = Mathf.Lerp(startAirP, targetAirP, t);

                if (forestCanopyGroup != null)
                {
                    float s = Mathf.Lerp(startScale, canopyScale, t);
                    forestCanopyGroup.localScale = new Vector3(s, s, s);
                }

                if (hydrationSlider != null)
                {
                    hydrationSlider.value = currentState.hydrationPercent;
                }

                UpdateUI(currentState);
                yield return null;
            }

            currentState.hydrationPercent = targetHydration;
            currentState.waterQualityPercent = targetWaterQ;
            currentState.airPurityPercent = targetAirP;
            UpdateUI(currentState);
        }

        private void UpdateUI(EcosystemSnapshot state)
        {
            if (stageText != null)
            {
                stageText.text = state.stageLabel;
            }

            if (metricsText != null)
            {
                metricsText.text =
                    $"Temp: {state.temperatureC:0.#}°C | Soil Moisture: {state.soilMoisturePercent:0.#}% | Green Coverage: {state.greenCoveragePercent:0.#}%\n" +
                    $"Hydration: {state.hydrationPercent:0.#}% | Water Quality: {state.waterQualityPercent:0.#}% | Air Purity: {state.airPurityPercent:0.#}% | Biodiversity: {state.biodiversityPercent:0.#}%";
            }

            if (statusText != null)
            {
                statusText.text = $"Habitat Balance: {state.habitatBalanceStatus} | Ecosystem Status: {state.ecosystemStatus}";
            }
        }
    }
}
