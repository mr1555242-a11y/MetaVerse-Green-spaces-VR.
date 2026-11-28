using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * ============================================================================
 * MODULE: VRRestorationTrigger.cs
 * STEP WORK: Connects XR interaction events to trigger or reset the
 *            ecological restoration simulation in VR.
 * CHALLENGE: Avoiding duplicate event subscriptions or null reference errors
 *            when XR interactables enable/disable during scene transitions.
 * SOLUTION:  Attached [RequireComponent(typeof(XRBaseInteractable))] and
 *            registered/unregistered listeners cleanly in OnEnable/OnDisable
 *            with null checks.
 * CURRENT & FUTURE ROLE: Currently provides clean VR trigger control during
 *            demos; in the future, it can support multi-step interactive
 *            choices across diverse VR training scenarios.
 * ============================================================================
 */

namespace MetaverseGreenSpacesVR
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class VRRestorationTrigger : MonoBehaviour
    {
        [SerializeField] private RestorationController restorationController;
        [SerializeField] private bool resetOnSelectExit = false;

        private XRBaseInteractable interactable;

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        private void OnEnable()
        {
            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnTriggered);
                if (resetOnSelectExit)
                {
                    interactable.selectExited.AddListener(OnReset);
                }
            }
        }

        private void OnDisable()
        {
            if (interactable != null)
            {
                interactable.selectEntered.RemoveListener(OnTriggered);
                if (resetOnSelectExit)
                {
                    interactable.selectExited.RemoveListener(OnReset);
                }
            }
        }

        private void OnTriggered(SelectEnterEventArgs args)
        {
            if (restorationController != null)
            {
                restorationController.StartRestorationSequence();
            }
        }

        private void OnReset(SelectExitEventArgs args)
        {
            if (restorationController != null)
            {
                restorationController.ApplyInitialState();
            }
        }
    }
}
