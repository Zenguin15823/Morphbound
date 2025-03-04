using UnityEngine;

public class Death : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            transform.position = spawnPoint;
    }
}
