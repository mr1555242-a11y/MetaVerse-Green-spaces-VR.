

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MetaVerseGreenSpacesVR
{
    [System.Serializable]
    public class FormattedStatusEvent : UnityEvent<string> { }

    public class GreenSpaceUIConnector : MonoBehaviour
    {
        [Header("Source & Optional Legacy UI Text")]
        [SerializeField] private GreenSpaceManager greenSpaceManager;
        [SerializeField] private Text statusText;
        [SerializeField] private Text mentorResponseText;

        [Header("Custom UI / TextMeshPro Hook")]
        public FormattedStatusEvent onStatusTextFormatted;
        public FormattedStatusEvent onMentorTextFormatted;

        private void Awake()
        {
            if (greenSpaceManager == null)
            {
                greenSpaceManager = FindFirstObjectByType<GreenSpaceManager>();
            }
        }

        private void OnEnable()
        {
            if (greenSpaceManager != null)
            {
                greenSpaceManager.onEcologicalReportUpdated.AddListener(HandleReportUpdated);
            }
        }

        private void OnDisable()
        {
            if (greenSpaceManager != null)
            {
                greenSpaceManager.onEcologicalReportUpdated.RemoveListener(HandleReportUpdated);
            }
        }

        public void HandleReportUpdated(EcologicalReport report)
        {
            string formatted =
                $"Biome: {report.biomeName}\n" +
                $"Weather: {report.weather} | Temp: {report.resources.temperatureC:F1}C\n" +
                $"Flora: {report.totalFloraCount} ({report.speciesDiversityCount} species)\n" +
                $"Water: {report.resources.waterLevel:F1}% | Nutrients: {report.resources.soilNutrients:F1}%\n" +
                $"Avg Vitality: {report.averageVitality:F1}%\n" +
                $"Annual Carbon Offset: {report.estimatedAnnualCarbonKg:F1} kg CO2/yr";

            if (statusText != null)
            {
                statusText.text = formatted;
            }

            onStatusTextFormatted?.Invoke(formatted);
        }

        public async void RequestAndDisplayMentorAdvice(string query)
        {
            if (greenSpaceManager == null) return;

            string reply = await greenSpaceManager.AskEcologicalMentor(query);
            if (mentorResponseText != null)
            {
                mentorResponseText.text = reply;
            }

            onMentorTextFormatted?.Invoke(reply);
        }
    }
}

*GreenSpaceUIConnector.cs (UI Connector)
​To display ecological status, feedback, and AI mentor responses in the user interface:
​UI References:
​Assign UI text elements (such as TextMeshProUGUI or Text), input fields, buttons, and panels in the Inspector.
​Scene & Event Setup:
​Include a Canvas and an EventSystem in the scene, and connect UI action/update methods to the corresponding buttons or state callbacks.
​Project Settings:
​Configure the Canvas render mode (such as World Space for VR) and XR UI Input Module so interactions work smoothly.*
