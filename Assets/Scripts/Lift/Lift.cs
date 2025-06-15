using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public Animator animator; 
    public List<Transform> points;
    public int currentPoint = 0;
    public float speed = 5f;
    public float startTime = 1f; //время после нажатия на кнопку после которого лифт поедет
    public float doorCloseTime = 4f;

    private LiftState liftState;
    private PlayerMovement playerMovement;
    private PlayerPhysics playerPhysics;

    private void Start()
    {
        liftState = new LiftAfkState();
    }

    private void Update()
    {
        liftState.StateExecute();
    }

    public void LiftAfkStateApplication() //лифт остонавливаеться и двери лифта открывються
    {
        liftState = new LiftAfkState();
        OpenDoor();
        StartCoroutine(CloseDoorAfterDelay(doorCloseTime)); //через через время двери закроются
    }

    public void LiftMoweStateApplication()
    {
        liftState = new LiftMoweState(this, transform, points[currentPoint].transform, speed);
        OnOrOffPlayerSkripts(false);
    }

    public void OnOrOffPlayerSkripts(bool onOrOff) //классы playerMovement и playerPhysics отключаются так как из за них при движении лифта камера дёргаеться
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = onOrOff;
            playerPhysics.enabled = onOrOff;
        }
    }

    public void OpenDoor()
    {
        animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            playerMovement = other.GetComponent<PlayerMovement>();
            playerPhysics = other.GetComponent<PlayerPhysics>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            playerMovement = null;
            playerPhysics = null;
        }
    }

    public IEnumerator StartMove()
    {
        yield return new WaitForSeconds(startTime);
        LiftMoweStateApplication();
    }

    public IEnumerator CloseDoorAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        CloseDoor();
    }
}
