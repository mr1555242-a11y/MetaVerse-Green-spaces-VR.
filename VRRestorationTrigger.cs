using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MetaverseGreenSpacesVR
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class VRRestorationTrigger : MonoBehaviour
    {
        [SerializeField] private RestorationController restorationController;
        [SerializeField] private bool resetOnSelectExit = false;

        private XRBaseInteractable interactable;

        // [Step 1 - Interactable Setup]
        // What we did: Retrieved XRBaseInteractable component in Awake().
        // Challenge: Missing XR interactable component could cause null errors on setup.
        // Solution: Added [RequireComponent(typeof(XRBaseInteractable))] and null checks.
        // Current & Future Role: Currently ensures reliable VR trigger setup; in the future, it
        // allows modular interactive triggers across scenes.
        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        // [Step 2 - Event Subscription Lifecycle]
        // What we did: Subscribed to selectEntered/selectExited in OnEnable and unsubscribed in OnDisable.
        // Challenge: Duplicate event firing or lingering listeners when objects toggle active state.
        // Solution: Paired listener registration and removal cleanly with OnEnable and OnDisable.
        // Current & Future Role: Currently keeps VR interaction stable during demos; in the future,
        // it supports safe scene transitions and multi-step interactions.
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

        // [Step 3 - Start & Reset Trigger Handlers]
        // What we did: Connected VR selection events to StartRestorationSequence() and ApplyInitialState().
        // Challenge: Triggering actions without a valid controller reference during testing.
        // Solution: Guarded calls with null checks on restorationController.
        // Current & Future Role: Currently starts or resets the restoration walkthrough on demand;
        // in the future, it can trigger guided environmental tasks for learners.
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
