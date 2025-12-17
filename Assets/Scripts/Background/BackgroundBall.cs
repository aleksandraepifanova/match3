using UnityEngine;

public class BackgroundBall : MonoBehaviour
{
    private float speed;
    private float amplitude;
    private float frequency;
    private Vector3 direction;
    private float startY;
    private float minX, maxX, minY, maxY;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;

        speed = Random.Range(0.2f, 0.6f);
        amplitude = Random.Range(0.3f, 0.8f);
        frequency = Random.Range(0.5f, 1.5f);

        direction = Random.insideUnitCircle.normalized;
        direction.z = 0;

        startY = transform.position.y;
        CalculateBounds();
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(
            transform.position.x,
            startY + yOffset,
            transform.position.z
        );
        Vector3 pos = transform.position;

        if (pos.x <= minX || pos.x >= maxX)
        {
            direction.x *= -1f;
        }

        if (pos.y <= minY || pos.y >= maxY)
        {
            direction.y *= -1f;
        }

    }
    private void CalculateBounds()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        float radius = spriteRenderer.bounds.extents.x;

        minX = -width / 2f + radius;
        maxX = width / 2f - radius;
        minY = -height / 2f + radius;
        maxY = height / 2f - radius;
    }

}
