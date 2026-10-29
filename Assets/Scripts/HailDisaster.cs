using System.Collections;
using UnityEngine;

public class HailDisaster : MonoBehaviour
{
    [SerializeField] private float spawnWidth = 10f;
    [SerializeField] private float spawnHeight = 5f;
    [SerializeField] private float spawnInterval = 0.15f;
    [SerializeField] private float fallSpeed = 5f;

    private Coroutine hailRoutine;
    private static Sprite hailSprite;

    public void StartHail()
    {
        if (hailRoutine == null)
        {
            hailRoutine = StartCoroutine(SpawnHail());
        }
    }

    public void StopHail()
    {
        if (hailRoutine != null)
        {
            StopCoroutine(hailRoutine);
            hailRoutine = null;
        }
    }

    private IEnumerator SpawnHail()
    {
        while (true)
        {
            var hailstone = new GameObject("Hailstone");
            hailstone.transform.position = transform.position + new Vector3(
                Random.Range(-spawnWidth / 2f, spawnWidth / 2f),
                spawnHeight,
                0f);
            hailstone.transform.localScale = Vector3.one * 0.16f;

            var renderer = hailstone.AddComponent<SpriteRenderer>();
            renderer.sprite = GetHailSprite();
            renderer.color = new Color(0.8f, 0.9f, 1f);
            renderer.sortingOrder = 5;

            hailstone.AddComponent<CircleCollider2D>();
            var body = hailstone.AddComponent<Rigidbody2D>();
            body.linearVelocity = Vector2.down * fallSpeed;

            Destroy(hailstone, 5f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private static Sprite GetHailSprite()
    {
        if (hailSprite != null) return hailSprite;

        var texture = new Texture2D(16, 16, TextureFormat.RGBA32, false);
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(7.5f, 7.5f));
                texture.SetPixel(x, y, distance < 7f ? Color.white : Color.clear);
            }
        }

        texture.Apply();
        hailSprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), Vector2.one * 0.5f);
        return hailSprite;
    }
}
