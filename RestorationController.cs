using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * ============================================================================
 * MODULE: RestorationController.cs
 * STEP WORK: Progresses the simulation from cracked earth and sparse shrubs
 *            through sapling/canopy growth, rainfall, and a clear stream as
 *            hydration rises from 40% -> 55% -> 95% -> 100%, culminating in
 *            swimming fish, blooming flowers, a blue butterfly, Habitat Balance:
 *            Optimal, and Ecosystem Status: Thriving.
 * CHALLENGE: Synchronizing smooth visual environment changes (canopy growth,
 *            rain, stream, wildlife/flora) with live UI/HUD metric updates
 *            without freezing the frame rate.
 * SOLUTION:  Implemented coroutine-based interpolation (AnimateHydrationTo)
 *            with Time.deltaTime and modular GameObject/ParticleSystem toggles.
 * CURRENT & FUTURE ROLE: Currently powers the interactive cause-and-effect
 *            walkthrough; in the future, it can support guided learning modes
 *            and additional restoration stages for classrooms and workshops.
 * ============================================================================
 */

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
        [SerializeField] private GameObject biodiversityGroup; // Swimming fish, blooming flowers, blue butterfly

        [Header("UI / HUD Readouts")]
        [SerializeField] private TMP_Text stageText;
        [SerializeField] private TMP_Text metricsText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Slider hydrationSlider;

        [Header("Transition Timing")]
        [SerializeField] private float stepDuration = 2.0f;

        private EcosystemSnapshot currentState;
        private readonly float[] hydrationMilestones = { 40f, 55f, 95f, 100f };

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

        private IEnumerator RunRestorationRoutine()
        {
            // Step 1: Reforestation, rainfall, and clear stream onset -> 40% hydration
            currentState.stageLabel = "Reforestation & Water Return";
            if (rainfallParticles != null && !rainfallParticles.isPlaying) rainfallParticles.Play();
            if (clearStreamObject != null) clearStreamObject.SetActive(true);
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[0], 12f, 35f, 0.35f));

            // Step 2: Growth & moisture progression -> 55% hydration
            currentState.soilMoisturePercent = 45f;
            currentState.greenCoveragePercent = 45f;
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[1], 30f, 55f, 0.60f));

            // Step 3: Dense canopy & recovery indicators -> 95% hydration
            currentState.soilMoisturePercent = 75f;
            currentState.greenCoveragePercent = 82f;
            currentState.waterQualityPercent = 88f;
            currentState.airPurityPercent = 86f;
            currentState.habitatBalanceStatus = "Optimal / Stable";
            yield return StartCoroutine(AnimateHydrationTo(hydrationMilestones[2], 65f, 90f, 0.90f));

            // Step 4: Full hydration (100%) -> Habitat Balance: Optimal & Ecosystem Status: Thriving
            currentState = EcosystemPresets.ThrivingState();
            if (hydrationSlider != null) hydrationSlider.value = 100f;
            if (forestCanopyGroup != null) forestCanopyGroup.localScale = Vector3.one;
            if (biodiversityGroup != null) biodiversityGroup.SetActive(true);

            UpdateUI(currentState);
        }

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
