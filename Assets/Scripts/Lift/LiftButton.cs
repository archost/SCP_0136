using UnityEngine;

public class LiftButton : MonoBehaviour , IInteractable
{
    public int floorNuber; //на какой этаж двигать. Щитаеться с нуля
    [SerializeField] private Lift lift;

    public virtual string GetPromptText()
    {
        return "этаж номер " + floorNuber;
    }

    public virtual void Interact(PlayerInteractor interactor)
    {
        lift.currentPoint = floorNuber; //указываем лифту на какой этаж двигать
        lift.CloseDoor();
        StartCoroutine(lift.StartMove());
    }
}
