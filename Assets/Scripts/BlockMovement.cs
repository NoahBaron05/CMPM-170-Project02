using UnityEngine;
using UnityEngine.InputSystem;

public class BlockMovement : MonoBehaviour
{
    //Input system
    InputAction clickAction;
    InputAction rotateAction;

    //Picking up blocks variables
    private bool isDragging = false;
    Vector3 offset;

    //Rotating blocks variables
    [SerializeField] private float rotateSpeed;
    private bool isRotating;

    Rigidbody2D rb;

    private void Start()
    {
        clickAction = InputSystem.actions.FindAction("ClickBlock");
        rotateAction = InputSystem.actions.FindAction("Rotate");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveBlock();
    }

    private void MoveBlock()
    {
        if (clickAction == null) return;

        //Grabs mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mousePosition.z = transform.position.z;

        if (clickAction.WasPressedThisFrame())
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (hit != null && hit.gameObject == gameObject && gameObject.tag == "Block")
            {
                isDragging = true;
                offset = transform.position - mousePosition;

                //Sets all values to 0 so the block doesn't have previous velocity
                rb.gravityScale = 0f;
                rb.angularVelocity = 0f;
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (clickAction.WasReleasedThisFrame())
        {
            isDragging = false;
            rb.gravityScale = 1f;
        }

        if (isDragging)
        {
            rb.position = mousePosition + offset;
            RotateBlock();
        }
    }

    private void RotateBlock()
    {
        if (rotateAction == null) return;

        if (rotateAction.WasPressedThisFrame() && isDragging)
        {
            isRotating = true;
        }

        if (rotateAction.WasReleasedThisFrame())
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float rotateValue = rotateAction.ReadValue<float>();
            rb.rotation += -rotateValue * rotateSpeed;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
