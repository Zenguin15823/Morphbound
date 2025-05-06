using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();

        startPos = transform.position.x;

        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
        else if (tilemap != null)
        {
            length = tilemap.localBounds.size.x;
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer or Tilemap component found on: " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
