



using UnityEngine;
using System;

namespace MetaVerseGreenSpacesVR
{
    public class EcologicalReportUploader : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private GreenSpaceManager manager;
        [SerializeField] private bool autoUploadOnUpdate = true;

        private void Awake()
        {
            if (manager == null) manager = FindFirstObjectByType<GreenSpaceManager>();
        }

        private void OnEnable()
        {
            if (manager != null && autoUploadOnUpdate)
            {
                manager.onEcologicalReportUpdated.AddListener(UploadReportData);
            }
        }

        private void OnDisable()
        {
            if (manager != null)
            {
                manager.onEcologicalReportUpdated.RemoveListener(UploadReportData);
            }
        }

        public void UploadReportData(EcologicalReport report)
        {
            try
            {
                string jsonPayload = JsonUtility.ToJson(report, true);
                Debug.Log($"[ReportUploader] Preparing payload for {report.biomeName}: {jsonPayload.Length} bytes.");
                // Add custom web request or cloud sync logic here
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ReportUploader] Failed to serialize report: {ex.Message}");
            }
        }
    }
}


