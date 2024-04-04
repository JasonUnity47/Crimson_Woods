using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    // Declaration
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer SR;

    // Variable
    [Header("Movement")]
    public bool facingRight = true;

    public bool isHurt = false;
    public bool isDead = false;

   

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // State Machine
    public GoblinStateMachine goblinStateMachine { get; private set; }

    public GoblinIdleState IdleState { get; private set; }

    public GoblinChaseState ChaseState { get; private set; }

    public GoblinDeadState DeadState { get; private set; }

    // Script Reference
    private GoblinStats goblinStats;

    public GoblinMovement goblinMovement { get; private set; }

    public LootBag lootBag { get; private set; }

    private void Awake()
    {
        goblinStateMachine = new GoblinStateMachine();

        lootBag = GetComponent<LootBag>();



        goblinStats = GetComponent<GoblinStats>(); // Get reference before other states

        IdleState = new GoblinIdleState(this, goblinStateMachine, goblinStats, "GoblinIdle");
        ChaseState = new GoblinChaseState(this, goblinStateMachine, goblinStats, "GoblinChase");
        DeadState = new GoblinDeadState(this, goblinStateMachine, goblinStats, "GoblinDead");
    }

    private void Start()
    {
        goblinMovement = GetComponent<GoblinMovement>();
        
        SR = GetComponent<SpriteRenderer>();

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = SR.material;

        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        goblinStateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
            
        }

        FlipDirection();

        goblinStateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        goblinStateMachine.CurrentState.PhysicsUpdate();
    }

    public void CheckDead()
    {
        if (goblinStats.health <= 0)
        {
            isDead = true;
            isHurt = true;
            Rb.velocity = Vector2.zero;
            goblinStats.health = 0;



            goblinStateMachine.ChangeState(DeadState);
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
            goblinStats.health -= damageValue;

            SR.material = matWhite;
            Invoke("ResetMaterial", 0.05f);
            StartCoroutine("WaitForHurt");
        }
    }

    
    public void FlipDirection()
    {
        if (Rb.velocity.x >= 0.01 && !facingRight || Rb.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator WaitForHurt()
    {
        isHurt = true;

        yield return new WaitForSeconds(0.05f);

        isHurt = false;
    }

    void ResetMaterial()
    {
        SR.material = matDefault;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduce player's health here
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // You can adjust the damage value as needed
            }

            
        }
    }
}

