using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement Settings")]
    
    [SerializeField] private float walkSpeed = 1;
    private Rigidbody2D rb;
    private float xAxis, yAxis;
    Animator anim;


    //JumpBuffer
    private int jumpBufferCounter;
    [SerializeField] private int jumpBufferFrames; //60

    //CoyoteTime
    private float coyoteTimeCounter = 0;

    [SerializeField] private float coyoteTime; //0.1 secound

    //Double Jump
    private int airJumpCounter = 0;
    [SerializeField] private int maxMaxAirJumps;


    [Header("Ground Check Settings")]
    
    [SerializeField] private float jumpForce = 45;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;

    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash Settings")]
    //Dashing
    [SerializeField] private float dashSpeed; //speed of dash
    [SerializeField] private float dashTime; //how long dash lasts
    [SerializeField] private float dashCooldown; //cooldown between dashes
    
    private float gravity;
    public static Player_Controller Instance;

    PlayerStateList pstate;
    private bool canDash;
    private bool dashed;

    //attack
    [Header("Attack Settings")]
    bool attack = false;
    float timeBetweenAttack, timeSinceAttack;
    [SerializeField] Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    [SerializeField] Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask attackableLayer;

    [SerializeField] float damage;
    [SerializeField] GameObject slashEffect;

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }
    void Start()
    {
        pstate = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        canDash = true;
        timeBetweenAttack = 0.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (SideAttackTransform != null)
            Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);

        if (UpAttackTransform != null)
            Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);

        if (DownAttackTransform != null)
            Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if(pstate.dashing) return; //makes sure the player cant move jump or switch sides when dashing


        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
        
    }
    
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    //flips animation for walking
    void Flip()
    {
        if(xAxis < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if(xAxis > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

    }

    private void Move(){
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
        if (rb.linearVelocity.x != 0 && Grounded())
            anim.SetBool("Walking", true);
        else
            anim.SetBool("Walking", false);
    }

    void StartDash(){
        //makes sure player can only dash once in the air

        //left shift == dash button
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if(Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash(){
        canDash = false;
        pstate.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pstate.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack(){
        timeSinceAttack += Time.deltaTime;
        if(attack && timeSinceAttack>= timeBetweenAttack)
        {
            
            Debug.Log("ATTACK triggered");
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");


            //if on ground and attack they hit the side
            if(yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(SideAttackTransform, SideAttackArea);
                Instantiate(slashEffect, SideAttackTransform);
            }

            else if(yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea);
                SlashEffectAtAngle(slashEffect, 80, UpAttackTransform);
            }

            else if(yAxis < 0 && !Grounded())
            {
                Hit(DownAttackTransform, DownAttackArea);
                SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);
            }
        }
    }

    private void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if(objectsToHit.Length > 0 )
        {
            Debug.Log("HIT");
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            Enemy enemy = objectsToHit[i].GetComponent<Enemy>();
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage);
            }
        }
    }

    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect,_attackTransform );
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    public bool Grounded()
    {
        //Checks if raycast hits an edge of the player or the ground check point
        if(Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)){
            return true;
        }
        else{
            return false;
        }
    }

    void Jump()
    {
        //when player lets go of jump
        if(Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            //variable jump height
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            pstate.jumping = false;
        }



        //when player jumps
        if(!pstate.jumping)
        {
            if(jumpBufferCounter > 0 && coyoteTimeCounter > 0){
            //
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);

                pstate.jumping = true;
            }
            else if(!Grounded() && airJumpCounter < maxMaxAirJumps && Input.GetButtonDown("Jump")){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                pstate.jumping = true;
                airJumpCounter++;
            }
        }
        

        anim.SetBool("Jumping", !Grounded());

    }

    void UpdateJumpVariables(){
        if(Grounded()){
            pstate.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBufferFrames;
        }
        else{
            jumpBufferCounter--;
        }
    }
}
