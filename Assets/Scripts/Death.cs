using UnityEngine;
using UnityEngine.Rendering;

public class Death : MonoBehaviour
{
    public bool dead;

    private Vector3 spawnPoint;
    private SpriteRenderer sr;
    private PlayerMovement pm;
    private float deathScreenTimer;

    void Start()
    {
        spawnPoint = transform.position;
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<PlayerMovement>();
        deathScreenTimer = 0;
        dead = false;
    }

    private void Update()
    {
        if (dead)
        {
            if (deathScreenTimer <= 0)
            {
                deathScreenTimer = 0;
                transform.position = spawnPoint;
                sr.enabled = true;
                pm.enabled = true;
                dead = false;
            }
            else
            {
                deathScreenTimer -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            sr.enabled = false;
            pm.enabled = false;
            deathScreenTimer = 1;
            dead = true;
        }
    }
}
