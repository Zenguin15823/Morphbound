using UnityEngine;

public class Breaker : MonoBehaviour
{
    private SpecialMovement sm;

    void Start()
    {
        sm = GetComponent<SpecialMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (sm.isDashing() && collision.gameObject.layer == 8) Destroy(collision.gameObject);
    }
}
