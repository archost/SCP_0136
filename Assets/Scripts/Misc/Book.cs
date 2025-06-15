using UnityEngine;
using UnityEngine.InputSystem;

public class Book : Item
{
    [SerializeField] private int totalPages = 10;
    private int currentPage = 0;

    public InputActionReference flipPageAction;

    private float nextFlipTime;
    private float flipDelay = 0.5f;

    protected override void Use()
    {
        currentPage++;
        Debug.Log($"����������� �� �������� {currentPage}");
    }

    protected override void EnableActions()
    {
        flipPageAction.action.Enable();
    }

    protected override void DisableActions()
    {
        flipPageAction.action.Disable();
    }

    private void Update()
    {
        if (IsPickedUp)
        {
            float flipDirection = flipPageAction.action.ReadValue<float>();

            if (Time.time >= nextFlipTime && Mathf.Abs(flipDirection) > 0.5f)
            {
                TurnPage((int)Mathf.Sign(flipDirection));
                nextFlipTime = Time.time + flipDelay;
            }
        }
    }

    private void TurnPage(int direction)
    {
        int newPage = Mathf.Clamp(currentPage + direction, 0, totalPages - 1);

        if (newPage != currentPage)
        {
            currentPage = newPage;
            Debug.Log($"Page: {currentPage}");
        }
    }

    public override string GetPromptText()
    {
        return "������� �, ����� ����� �����";
    }
}