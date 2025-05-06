using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
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

    // Handle actual collisions (e.g. enemies, spikes)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Lethal object
        {
            KillPlayer("Hit lethal object");
        }
    }

    // Handle trigger deaths (e.g. out-of-bounds or special zones)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.gameObject.layer == 6) // Trigger-based spikes, etc.
        {
            SceneManager.LoadScene(0);
        }
    }

    public void KillPlayer(string reason = "Unknown")
    {
        Debug.Log("Player died: " + reason);
        sr.enabled = false;
        pm.enabled = false;
        deathScreenTimer = 1f;
        dead = true;
    }
}
