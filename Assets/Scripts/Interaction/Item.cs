using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    protected bool IsPickedUp { get; private set; }

    public virtual void Interact(PlayerInteractor interactor)
    {
        if (!IsPickedUp)
        {
            PickUp(interactor);
        }
        else
        {
            Drop();
        }
    }

    public virtual void PickUp(PlayerInteractor interactor)
    {
        IsPickedUp = true;

        transform.SetParent(interactor.holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = true;
        }

        EnableActions();
    }

    public virtual void Drop() 
    {
        if (!IsPickedUp) return;

        IsPickedUp = false;

        transform.SetParent(null);

        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = false;
        }

        DisableActions();
    }

    public virtual string GetPromptText()
    {
        return "Item interaction";
    }

    protected abstract void Use();
    protected abstract void EnableActions();
    protected abstract void DisableActions();
}
