using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boar : MonoBehaviour
{
    // Declaration
    SpriteRenderer SR;

    // Damage
    [Header("Damage")]
    public float damage;

    // Variable
    [Header("Movement")]
    public bool facingRight = true;
    public bool isHurt = false;
    public bool isDead = false;

    // Loot
    [Header("Loot")]
    public int lootCount;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    public Collider2D[] col;

    private SpriteRenderer spriteRenderer;

    // State Machine
    public BoarStateMachine boarStateMachine { get; private set; }

    public BoarIdleState IdleState { get; private set; }

    public BoarChaseState ChaseState { get; private set; }

    public BoarDeadState DeadState { get; private set; }

    // Script Reference
    private BoarStats boarStats;


    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playPos { get; private set; }

    public BuffContent buffContent { get; private set; }

    private void Awake()
    {
        boarStateMachine = new BoarStateMachine();

        lootBag = GetComponent<LootBag>();

        aiPath = GetComponent<AIPath>();

        boarStats = GetComponent<BoarStats>(); // Get reference before other states

        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new BoarIdleState(this, boarStateMachine, boarStats, "BoarIdle");
        ChaseState = new BoarChaseState(this, boarStateMachine, boarStats, "BoarChase");
        DeadState = new BoarDeadState(this, boarStateMachine, boarStats, "BoarDead");
    }

    private void Start()
    {        

        SR = GetComponent<SpriteRenderer>();

        Rb = GetComponent<Rigidbody2D>();

        Anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        boarStateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        boarStateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        boarStateMachine.CurrentState.PhysicsUpdate();
    }


    public void CheckDead()
    {
        if (boarStats.health <= 0)
        {
            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }

            tag = "Untagged";
            //Physics2D.IgnoreLayerCollision(6, 7);
            
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].enabled == true)
                {
                    col[i].enabled = false;
                }
            }

            isDead = true;
            isHurt = true;

            boarStats.health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            spriteRenderer.sortingOrder = 9;

            boarStateMachine.ChangeState(DeadState);
        }
    }

    public void DestroyBody()
    {
        Destroy(this.gameObject, 3f);
    }

    public void TakeDamage(int damageValue)
    {
        if (isDead)
        {
            return;
        }

        if (!isHurt)
        {
            boarStats.health -= damageValue;

            Anim.SetTrigger("HurtTrigger");

            StartCoroutine("WaitForHurt");
        }
    }


    public void FlipDirection()
    {
        if (aiPath.velocity.x >= 0.01 && !facingRight || aiPath.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    //Attack Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    IEnumerator WaitForHurt()
    {
        isHurt = true;

        yield return new WaitForSeconds(0.1f);

        isHurt = false;
    }


}