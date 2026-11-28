using System;
using UnityEngine;

/*
 * ============================================================================
 * MODULE: EcosystemState.cs
 * STEP WORK: Defines the initial degraded baseline (25°C, 15% soil moisture,
 *            1% green coverage) and target thriving ecosystem metrics
 *            (40% -> 55% -> 95% -> 100% hydration, Habitat Balance: Optimal,
 *            Ecosystem Status: Thriving).
 * CHALLENGE: Keeping numerical indicators and ecological status labels
 *            consistent across simulation stages without hardcoded magic strings.
 * SOLUTION:  Used a structured EcosystemSnapshot struct and static preset
 *            methods (InitialDegradedState and ThrivingState).
 * CURRENT & FUTURE ROLE: Currently provides clean, reliable data states for the
 *            VR demo; in the future, it can expand to support multi-biome and
 *            localized environmental datasets cleanly.
 * ============================================================================
 */

namespace MetaverseGreenSpacesVR
{
    [Serializable]
    public struct EcosystemSnapshot
    {
        public string stageLabel;
        public float temperatureC;
        public float soilMoisturePercent;
        public float greenCoveragePercent;
        public float hydrationPercent;
        public float waterQualityPercent;
        public float airPurityPercent;
        public float biodiversityPercent;
        public string habitatBalanceStatus;
        public string ecosystemStatus;
        public bool enableRainfall;
        public bool enableClearStream;
        public bool enableWildlifeAndFlora;
    }

    public static class EcosystemPresets
    {
        // Step 0: Degraded starting state (25°C, 15% soil moisture, 1% green coverage)
        public static EcosystemSnapshot InitialDegradedState()
        {
            return new EcosystemSnapshot
            {
                stageLabel = "Stage 0: Degraded Baseline",
                temperatureC = 25f,
                soilMoisturePercent = 15f,
                greenCoveragePercent = 1f,
                hydrationPercent = 15f,
                waterQualityPercent = 20f,
                airPurityPercent = 30f,
                biodiversityPercent = 10f,
                habitatBalanceStatus = "Degraded / Stressed",
                ecosystemStatus = "Critical Stress",
                enableRainfall = false,
                enableClearStream = false,
                enableWildlifeAndFlora = false
            };
        }

        // Final thriving state (100% hydration, Habitat Balance: Optimal, Ecosystem Status: Thriving)
        public static EcosystemSnapshot ThrivingState()
        {
            return new EcosystemSnapshot
            {
                stageLabel = "Stage 4: Optimal & Thriving",
                temperatureC = 22f,
                soilMoisturePercent = 85f,
                greenCoveragePercent = 92f,
                hydrationPercent = 100f,
                waterQualityPercent = 98f,
                airPurityPercent = 97f,
                biodiversityPercent = 96f,
                habitatBalanceStatus = "Optimal / Stable",
                ecosystemStatus = "Thriving",
                enableRainfall = false,
                enableClearStream = true,
                enableWildlifeAndFlora = true
            };
        }
    }
}
