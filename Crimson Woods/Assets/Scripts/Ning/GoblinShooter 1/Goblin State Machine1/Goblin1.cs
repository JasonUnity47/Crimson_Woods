using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Pathfinding;

public class Goblin1 : MonoBehaviour
{
    // Declaration
    private Material matWhite;
    private Material matDefault;
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
    public GoblinStateMachine1 goblinStateMachine1 { get; private set; }

    public GoblinIdleState1 IdleState { get; private set; }

    public GoblinChaseState1 ChaseState { get; private set; }

    public GoblinDeadState1 DeadState { get; private set; }

    // Script Reference
    private GoblinStats1 goblinStats1;

   

    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playPos { get; private set; }

    public BuffContent buffContent { get; private set; }

    private void Awake()
    {
        goblinStateMachine1 = new GoblinStateMachine1();

        lootBag = GetComponent<LootBag>();

        aiPath = GetComponent<AIPath>();

        goblinStats1 = GetComponent<GoblinStats1>(); // Get reference before other states

        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new GoblinIdleState1(this, goblinStateMachine1, goblinStats1, "GoblinIdle");
        ChaseState = new GoblinChaseState1(this, goblinStateMachine1, goblinStats1, "GoblinChase");
        DeadState = new GoblinDeadState1(this, goblinStateMachine1, goblinStats1, "GoblinDead");
    }

    private void Start()
    {
        

        SR = GetComponent<SpriteRenderer>();

        matWhite = Resources.Load<Material>("Material/WhiteFlash");
        matDefault = SR.material;


        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        goblinStateMachine1.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();

        }

        FlipDirection();

        goblinStateMachine1.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        goblinStateMachine1.CurrentState.PhysicsUpdate();
    }

    public void CheckDead()
    {
        if (goblinStats1.health <= 0)
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
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;
            goblinStats1.health = 0;

            spriteRenderer.sortingOrder = 9;

            goblinStateMachine1.ChangeState(DeadState);
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
            goblinStats1.health -= damageValue;

            SR.material = matWhite;
            Invoke("ResetMaterial", 0.1f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduce player's health here
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // You can adjust the damage value as needed
            }


        }
    }
}

