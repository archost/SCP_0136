using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour 
{
    public Transform holdPoint;

    [SerializeField] private UIPrompt prompt;
    [SerializeField] private Camera playerCamera;

    private Item lastSeenItem;
    private IInteractable lastSeenInteractable;
    private float interactionDistance = 5f;

    [SerializeField] private InputActionReference interactionAction;
    [SerializeField] private InputActionReference dropAction;

    private bool isHolding = false;

    private void Awake()
    {
        interactionAction.action.Enable();
    }

    void Update()
    {
        if (!isHolding)
        {
            CheckInteractable();
            if (interactionAction.action.triggered && lastSeenInteractable != null)
            {
                lastSeenInteractable.Interact(this);
                prompt.HidePrompt();

                if (lastSeenItem != null)
                    isHolding = true;
            }
        }
        else if (dropAction.action.triggered)
        {
            lastSeenInteractable.Interact(this);
            lastSeenInteractable = null;
            lastSeenItem = null;
            isHolding = false;
        }
    }

    private void CheckInteractable()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance);
        if (hasHit && hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            if (lastSeenInteractable != interactable)
            {
                lastSeenInteractable = interactable;
                prompt.ShowPrompt(interactable.GetPromptText());

                hit.collider.TryGetComponent<Item>(out var item);
                if (item != null)
                    lastSeenItem = item;
            }
        }
        else if (lastSeenInteractable != null)
        {
            lastSeenInteractable = null;
            lastSeenItem = null;
            prompt.HidePrompt();
        }
    }
}
