using UnityEngine;

public class MonkeyBananaScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    public Vector2 direction;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = direction.normalized * force;

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }
}
