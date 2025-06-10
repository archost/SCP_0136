using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorHinge;
    private bool isOpen = false;

    public void Interact(PlayerInteractor interactor)
    {
        if (!isOpen)
        {
            LeanTween.rotateY(doorHinge, -75, 1f).setEase(LeanTweenType.easeOutQuad);
            isOpen = true;
        }
        else
        {
            LeanTween.rotateY(doorHinge, 0, 1f).setEase(LeanTweenType.easeOutQuad);
            isOpen = false;
        }
    }

    public string GetPromptText()
    {
        if (!isOpen) 
        {
            return "Нажмите E, чтобы открыть дверь";
        }
        return "Нажмите E, чтобы закрыть дверь";
    }
}