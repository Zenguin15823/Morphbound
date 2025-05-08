using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMovement pm;
    private Death death;

    //attack
    float timeBetweenAttack, timeSinceAttack;
    [SerializeField] Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    [SerializeField] Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask attackableLayer;

    [SerializeField] float damage;
    [SerializeField] GameObject slashEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        death = GetComponent<Death>();
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
        if (death.dead) return;
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        bool attack = Input.GetMouseButtonDown(0);
        Attack(xAxis, yAxis, attack);
    }

    void Attack(float xAxis, float yAxis, bool attack){
        timeSinceAttack += Time.deltaTime;
        if(attack && timeSinceAttack>= timeBetweenAttack)
        {
            
            Debug.Log("ATTACK triggered");
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");


            //if on ground and attack they hit the side
            if(yAxis == 0 || yAxis < 0 && pm.isGrounded())
            {
                Hit(SideAttackTransform, SideAttackArea);
                Instantiate(slashEffect, SideAttackTransform);
            }

            else if(yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea);
                SlashEffectAtAngle(slashEffect, 80, UpAttackTransform);
            }

            else if(yAxis < 0 && !pm.isGrounded())
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
}
