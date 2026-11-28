using System;
using UnityEngine;

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
        // [Step 1 - Degraded Baseline]
        // What we did: Defined starting state with 25°C, 15% soil moisture, and 1% green coverage.
        // Challenge: Avoiding inconsistent numbers and scattered status text across scenes.
        // Solution: Used a structured EcosystemSnapshot preset for the initial degraded state.
        // Current & Future Role: Currently standardizes demo start metrics; in the future, it can
        // support multiple ecological starting presets cleanly.
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

        // [Step 2 - Optimal & Thriving Target State]
        // What we did: Defined target thriving state with 100% hydration, Habitat Balance: Optimal,
        // and Ecosystem Status: Thriving.
        // Challenge: Keeping final ecological indicators consistent with the end of the demo.
        // Solution: Centralized final target metrics in ThrivingState().
        // Current & Future Role: Currently marks the thriving completion state; in the future, it can
        // extend to additional restored habitat benchmarks.
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
