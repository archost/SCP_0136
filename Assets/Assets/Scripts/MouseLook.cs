using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public float smoothing = 0.1f;

    private float xRotation = 0f;
    private Vector2 smoothedDelta = Vector2.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        smoothedDelta = Vector2.Lerp(smoothedDelta, mouseDelta, smoothing);

        float mouseX = smoothedDelta.x * mouseSensitivity;
        float mouseY = smoothedDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}