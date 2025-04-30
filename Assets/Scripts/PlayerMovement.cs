using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpecialMovement sm;

    public float speed;
    public float jumpHeight;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sm = GetComponent<SpecialMovement>();
    }

    private void Update()
    {
        float inputHoriz = Input.GetAxis("Horizontal");

        if (sm.isDashing())
        {
            rb.linearVelocity = new Vector2(sm.dashDir * speed * 2, rb.linearVelocityY);
        }
        else 
            rb.linearVelocity = new Vector2(inputHoriz * speed, rb.linearVelocityY);

        bool grounded = isGrounded();
        if (sm.doubleJump && grounded)
            sm.hasJump = true;

        if (Input.GetKeyDown(KeyCode.Space) && (grounded || sm.hasJump))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            sm.hasJump = sm.doubleJump && grounded;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && sm.dashReady && inputHoriz != 0)
            sm.useDash(inputHoriz);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
