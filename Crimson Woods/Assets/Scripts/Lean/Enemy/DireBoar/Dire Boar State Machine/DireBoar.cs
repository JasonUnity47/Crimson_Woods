using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DireBoar : MonoBehaviour
{
    // Declaration

    // Variable
    //Movement
    [Header("Movement")]
    public bool facingRight = true;

    // Health
    [Header("Health")]
    public float health;
    public float maxHealth;
    public float hurtTime;
    public bool isHurt = false;
    public bool isDead = false;

    //Shock State
    [Header("Shock State")]
    [SerializeField] private Transform shockArea;
    [SerializeField] private float shockRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;
    public bool isShocked = false;
    public bool hasObstacle = false;

    // Charge State
    [Header("Charge State")]
    public float chargeCD;
    public float enterChargeTime;
    public float chargeDistance;
    public Vector2 lastTargetPosForCharge;
    [SerializeField] private float chargeSpeed;
    public bool isCharging = false;
    public bool hasCharged = false;

    // Check
    [Header("Check")]
    public float checkTime;
    private float timeBtwEachCheck;

    // Loot
    [Header("Loot")]
    public int lootCount;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    public Collider2D[] col;

    // State Machine
    public DireBoarStateMachine direBoarStateMachine { get; private set; }

    public DireBoarIdleState IdleState { get; private set; }

    public DireBoarChaseState ChaseState { get; private set; }

    public DireBoarShockState ShockState { get; private set; }

    public DireBoarChargeState ChargeState { get; private set; }

    public DireBoarDeadState DeadState { get; private set; }

    // Script Reference
    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playerPos { get; private set; }

    // Game Manager
    public BuffContent buffContent { get; private set; }

    private void Awake()
    {
        direBoarStateMachine = new DireBoarStateMachine();

        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();        
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        lootBag = GetComponent<LootBag>();
        aiPath = GetComponent<AIPath>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        IdleState = new DireBoarIdleState(this, direBoarStateMachine,  "IdleBool");
        ChaseState = new DireBoarChaseState(this, direBoarStateMachine, "ChaseBool");
        ShockState = new DireBoarShockState(this, direBoarStateMachine, "ShockBool");
        ChargeState = new DireBoarChargeState(this, direBoarStateMachine, "ChargeBool");
        DeadState = new DireBoarDeadState(this, direBoarStateMachine, "DeadBool");
    }

    private void Start()
    {
        // Initialize the first state.
        direBoarStateMachine.InitializeState(IdleState);

        // Initialize the enemy health.
        health = maxHealth;
    }

    private void Update()
    {
        direBoarStateMachine.CurrentState.LogicalUpdate();

        // Check whether the enemy is dead.
        CheckDead();

        // Check whether the enemy should flip to the correct facing direction.
        FlipDirection();       
    }

    private void FixedUpdate()
    {
        direBoarStateMachine.CurrentState.PhysicsUpdate();
    }

    public void TrackPlayer()
    {
        // Keep checking the player position in the game with a certain amount of time.
        if (timeBtwEachCheck <= 0)
        {
            timeBtwEachCheck = checkTime;

            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        else
        {
            timeBtwEachCheck -= Time.deltaTime;
        }

        return;
    }

    public void CheckDead()
    {
        if (health <= 0 && !isDead)
        {
            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }

            // Change the "Enemy" tag to "Untagged" tag to disable all the scripts that need "Enemy" tag to prevent after-dead issues.
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

            health = 0;            
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            // If the enemy is dead then change to Dead State.
            direBoarStateMachine.ChangeState(DeadState);
        }

        if (isDead && direBoarStateMachine.CurrentState != DeadState)
        {
            // If the enemy is dead then change to Dead State.
            direBoarStateMachine.ChangeState(DeadState);
        }

        return;
    }

    public void DestroyBody()
    {
        Destroy(this.gameObject, 3f);

        return;
    }

    public void TakeDamage(int damageValue)
    {
        if (isDead)
        {
            return;
        }

        if (!isHurt)
        {
            health -= damageValue;

            Anim.SetTrigger("HurtTrigger");

            StartCoroutine("WaitForHurt");
        }

        return;
    }

    public void DetectObstacle()
    {
        // Check whether there is any obstacle around the player
        hasObstacle = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsObstacle);

        return;
    }

    public void DetectPlayer()
    {
        // Check whether player is around the enemy.
        isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);

        return;
    }
    //public void ShockMotion()
    //{
    //    // IF no obstacle THEN check whether player is around the enemy
    //    if (!hasObstacle)
    //    {
    //        isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);
    //    }
    //}


    public void PrepareCharge()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(WaitForCharge());

        return;
    }


    public void FinishCharge()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(ChargeCD());

        return;
    }

    //public void ChargePlayer()
    //{
    //    // Move enemy to the last player position
    //    transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);
    //}



    public void FlipDirection()
    {
        // Check whether the enemy should flip to the correct facing direction.
        if (aiPath.velocity.x >= 0.01 && !facingRight || aiPath.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

        return;
    }

    public void ChargeAttack()
    {
        // Move enemy to the last player position.
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);

        return;
    }

    IEnumerator WaitForCharge()
    {
        // Set a predefined time to allow the enemy change to the next state.
        isCharging = true;

        yield return new WaitForSeconds(enterChargeTime);

        // Change to CHARGE STATE
        direBoarStateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        // Wait X seconds to refresh the charge action.
        yield return new WaitForSeconds(chargeCD);

        // Change charge status to FALSE for next execution
        hasCharged = false;
    }


    IEnumerator WaitForHurt()
    {
        // Given the enemy a short period to prevent continuous damage.
        isHurt = true;

        yield return new WaitForSeconds(hurtTime);

        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shockArea.position, shockRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(2);
        }
    }
}
