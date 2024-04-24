using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Pathfinding;

public class Slime : MonoBehaviour
{
    // Declaration
    private UnityEngine.Object explosionRef;
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

    // State Machine
    public SlimeStateMachine slimeStateMachine { get; private set; }

    public SlimeIdleState IdleState { get; private set; }

    public SlimeChaseState ChaseState { get; private set; }

    public SlimeDeadState DeadState { get; private set; }

    // Script Reference
    private SlimeStats slimeStats;

   

    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playPos { get; private set; }

    public BuffContent buffContent { get; private set; }

    private void Awake()
    {
        slimeStateMachine = new SlimeStateMachine();

        lootBag = GetComponent<LootBag>();

        aiPath = GetComponent<AIPath>();

        explosionRef = Resources.Load("Prefab/Explode/SlimeExplode");

        slimeStats = GetComponent<SlimeStats>(); // Get reference before other states

        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new SlimeIdleState(this, slimeStateMachine, slimeStats, "SlimeIdle");
        ChaseState = new SlimeChaseState(this, slimeStateMachine, slimeStats, "SlimeChase");
        DeadState = new SlimeDeadState(this, slimeStateMachine, slimeStats, "SlimeDead");
    }

    private void Start()
    {
        

        SR = GetComponent<SpriteRenderer>();

        matWhite = Resources.Load<Material>("Material/WhiteFlash");
        matDefault = SR.material;

        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        slimeStateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        slimeStateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        slimeStateMachine.CurrentState.PhysicsUpdate();
    }

    public void CheckDead()
    {
        if (slimeStats.health <= 0)
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

            slimeStats.health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

            slimeStateMachine.ChangeState(DeadState);
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
            slimeStats.health -= damageValue;

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
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}
