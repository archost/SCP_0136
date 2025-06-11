using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorHinge;

    [SerializeField] private AudioClip _doorOpen;
    [SerializeField] private AudioClip _doorClose;

    private bool isOpen = false;

    public void Interact(PlayerInteractor interactor)
    {
        if (!isOpen)
        {
            AudioManager.Instance.PlaySound(_doorOpen, AudioCategory.Oneshot, transform.position, spatialBlend: 1f);
            LeanTween.rotateY(doorHinge, -75, 1f).setEase(LeanTweenType.easeOutQuad);
            isOpen = true;
        }
        else
        {
            AudioManager.Instance.PlaySound(_doorClose, AudioCategory.Oneshot, transform.position, spatialBlend: 1f);
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