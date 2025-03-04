using UnityEngine;

public class MonkeyBananaScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    public Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction.normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
