using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    private float gravity = -19.62f;
    private float jumpHeight = 3f;
    private float groundDistance = 0.4f;

    private bool isGrounded;
    private Vector3 velocity;
    private CharacterController controller;

    public InputActionReference jumpAction;
    public Transform groundCheck;
    public LayerMask groundMask;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        jumpAction.action.Enable();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (jumpAction.action.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
