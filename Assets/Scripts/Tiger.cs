using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    private const float lungePower = 5;
    private const float chargeSpeed = 15;
    private const float walkSpeed = 5;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private float decisionTimer = 0;
    private bool lunging;
    private bool charging;
    private float chargeDir;
    private float actionTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // face the player
        if (FindPlayer().transform.position.x - transform.position.x > 0) sr.flipX = true;
        else sr.flipX = false;

        // don't let him fall off that platform!!!
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13.8f, 14.2f), transform.position.y, transform.position.z);

        if (!(lunging || charging))
        {
            rb.linearVelocity = new Vector2(walkSpeed * Mathf.Sign(FindPlayer().transform.position.x - transform.position.x), 0);

            decisionTimer += Time.deltaTime;
            if (decisionTimer > 0.2)
            {
                float rand = Random.value;
                if (rand < 0.1) Lunge();
                else if (rand < 0.15) Charge();
                decisionTimer = 0;
            }
        }
        if (charging)
        {
            // don't charge off the edge of the platform!
            if (transform.position.x < -13.6) chargeDir = 1;
            if (transform.position.x > 14.0) chargeDir = -1;

            rb.linearVelocity = new Vector2(chargeSpeed * chargeDir, 0);

            actionTimer -= Time.deltaTime;
            if (actionTimer < 0) charging = false;

            // face the right direction
            if (rb.linearVelocityX > 0) sr.flipX = true;
            else sr.flipX = false;
        }
        else if (lunging)
        {
            actionTimer -= Time.deltaTime;
            if (actionTimer < 0)
            {
                lunging = false;
                anim.Play("Tiger_Walk");
            }

        }
    }

    private GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private void Lunge()
    {
        // the clamp is to avoid jumping off the edge of the arena (into the spikes)
        float player_dist = Mathf.Clamp(FindPlayer().transform.position.x, -13.8f, 14.2f) - transform.position.x;

        Vector2 lungeVec = new Vector2(player_dist / 5, 2);
        rb.linearVelocity = lungeVec * lungePower;
        lunging = true;
        actionTimer = 1;

        anim.Play("Tiger_Lunge");
    }

    private void Charge()
    {
        chargeDir = Mathf.Sign(FindPlayer().transform.position.x - transform.position.x);
        charging = true;
        actionTimer = Random.Range(1f, 4f);
    }
}
