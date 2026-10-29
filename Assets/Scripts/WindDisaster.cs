
using UnityEngine;

public class WindDisaster : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] blocks;
    [SerializeField] private float windForce = 5f;

    private bool isWindActive = false;
    private Vector2 windDirection;

    public void StartWind()
    {
        if (blocks == null || blocks.Length == 0)
        {
            GameObject[] blockObjects = GameObject.FindGameObjectsWithTag("Block");
            blocks = new Rigidbody2D[blockObjects.Length];

            for (int i = 0; i < blockObjects.Length; i++)
            {
                blocks[i] = blockObjects[i].GetComponent<Rigidbody2D>();
            }
        }

        isWindActive = true;

        windDirection = Random.value < 0.5f
            ? Vector2.left
            : Vector2.right;

        Debug.Log("Wind started! Direction: " + windDirection);
    }

    private void FixedUpdate()
    {
        if (!isWindActive) return;

        foreach (Rigidbody2D block in blocks)
        {
            if (block != null)
            {
                block.AddForce(
                    windDirection * windForce,
                    ForceMode2D.Force
                );
            }
        }
    }

    public void StopWind()
    {
        isWindActive = false;
        Debug.Log("Wind stopped!");
    }
}
