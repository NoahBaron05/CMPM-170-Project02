using UnityEngine;

public class Earthquake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float distance = 5.0f;

    private Vector2 originalPosition;
    private bool isShaking = false;
    private bool isMovingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = rb.position;
    }

    public void StartEarthquake()
    {
        isShaking = true;
    }

    public void FixedUpdate()
    {
        if (!isShaking) return;

        if(isMovingRight)
        {
            rb.MovePosition(rb.position + Vector2.right * speed * Time.fixedDeltaTime);
            if (rb.position.x >= originalPosition.x + distance)
            {
                isMovingRight = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);
            if (rb.position.x <= originalPosition.x - distance)
            {
                isMovingRight = true;
            }
        }
    }

    public void StopEarthquake()
    {
        isShaking = false;
        rb.position = originalPosition;
    }
}
