using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction;
    public Transform cameraTransform;

    private float moveSpeed = 10f;
    private float movementSmoothing = 0.5f;
    private Vector3 currentVelocity;
    private Vector3 targetDirection;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        CalculateTargetDirection(input);
        SmoothMovement();
        controller.Move(currentVelocity * Time.deltaTime);
    }

    void CalculateTargetDirection(Vector2 input)
    {
        if (input.magnitude < 0.1f)
        {
            targetDirection = Vector3.zero;
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;

        targetDirection = (cameraForward * input.y + cameraRight * input.x).normalized * moveSpeed;
    }

    void SmoothMovement()
    {
        currentVelocity = Vector3.Lerp(currentVelocity, targetDirection, movementSmoothing * Time.deltaTime * 10f);
        if (targetDirection == Vector3.zero)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, movementSmoothing * Time.deltaTime * 5f);
        }
    }
}