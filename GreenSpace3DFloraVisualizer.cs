

using UnityEngine;

namespace MetaVerseGreenSpacesVR
{
    public class GreenSpace3DFloraVisualizer : MonoBehaviour
    {
        [Header("Visualization Settings")]
        [SerializeField] private MeshFilter targetMeshFilter;
        [SerializeField] private MeshRenderer targetMeshRenderer;
        
        [Header("Growth Scaling Envelope")]
        [SerializeField] private float MinScaleMultiplier = 0.2f;
        [SerializeField] private float MaxScaleMultiplier = 2.5f;

        [Header("Visual Feedback Gradients")]
        [SerializeField] private Gradient healthColorGradient;
        [SerializeField] private Gradient droughtStressGradient;

        private MaterialPropertyBlock propertyBlock;
        private static readonly int BaseColorShaderID = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
            if (targetMeshFilter == null) targetMeshFilter = GetComponent<MeshFilter>();
            if (targetMeshRenderer == null) targetMeshRenderer = GetComponent<MeshRenderer>();
        }

        public void RenderFloraState(float currentGrowth, float healthNormalized, float droughtStressNormalized)
        {
            ApplyGrowthMeshScaling(currentGrowth);
            ApplyDynamicColorModulation(healthNormalized, droughtStressNormalized);
        }

        private void ApplyGrowthMeshScaling(float normalizedGrowth)
        {
            float clampedGrowth = Mathf.Clamp01(normalizedGrowth);
            float targetScale = Mathf.Lerp(MinScaleMultiplier, MaxScaleMultiplier, clampedGrowth);
            transform.localScale = Vector3.one * targetScale;
        }

        private void ApplyDynamicColorModulation(float healthNormalized, float droughtStressNormalized)
        {
            if (targetMeshRenderer == null) return;

            Color healthColor = healthColorGradient.Evaluate(Mathf.Clamp01(healthNormalized));
            Color stressColor = droughtStressGradient.Evaluate(Mathf.Clamp01(droughtStressNormalized));
            Color finalBlendedColor = Color.Lerp(healthColor, stressColor, Mathf.Clamp01(droughtStressNormalized * 0.5f));

            targetMeshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(BaseColorShaderID, finalBlendedColor);
            targetMeshRenderer.SetPropertyBlock(propertyBlock);
        }

        public void ResetVisualizationState()
        {
            transform.localScale = Vector3.one * MinScaleMultiplier;
            ApplyDynamicColorModulation(1.0f, 0.0f);
        }
    }
}


