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
    [Header("Movement")]
    public bool facingRight = true;

    public bool isHurt = false;
    public bool isDead = false;

    [Header("Shock State")]
    [SerializeField] private Transform shockArea;
    [SerializeField] private float shockRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;
    public bool isShocked = false;
    public bool hasObstacle = false;

    [Header("Charge State")]
    public float chargeCD;
    public float enterChargeTime;
    public float chargeDistance;
    public Vector2 lastTargetPosForCharge;
    [SerializeField] private float chargeSpeed;
    public bool isCharging = false;
    public bool hasCharged = false;

    // Loot
    [Header("Loot")]
    public int lootCount;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // State Machine
    public DireBoarStateMachine direBoarStateMachine { get; private set; }

    public DireBoarIdleState IdleState { get; private set; }

    public DireBoarChaseState ChaseState { get; private set; }

    public DireBoarShockState ShockState { get; private set; }

    public DireBoarChargeState ChargeState { get; private set; }

    public DireBoarDeadState DeadState { get; private set; }

    // Script Reference
    private DireBoarStats direBoarStats;


    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playPos { get; private set; }

    // Game Manager
    private BuffContent buffContent;

    private void Awake()
    {
        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();

        direBoarStateMachine = new DireBoarStateMachine();

        lootBag = GetComponent<LootBag>();

        aiPath = GetComponent<AIPath>();

        playPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        direBoarStats = GetComponent<DireBoarStats>(); // Get reference before other states

        IdleState = new DireBoarIdleState(this, direBoarStateMachine, direBoarStats, "IdleBool");
        ChaseState = new DireBoarChaseState(this, direBoarStateMachine, direBoarStats, "ChaseBool");
        ShockState = new DireBoarShockState(this, direBoarStateMachine, direBoarStats, "ShockBool");
        ChargeState = new DireBoarChargeState(this, direBoarStateMachine, direBoarStats, "ChargeBool");
        DeadState = new DireBoarDeadState(this, direBoarStateMachine, direBoarStats, "DeadBool");
    }

    private void Start()
    {        
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        direBoarStateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        direBoarStateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        direBoarStateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    public void CheckDead()
    {
        if (direBoarStats.health <= 0 && !isDead)
        {
            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }

            isDead = true;
            isHurt = true;

            direBoarStats.health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;
            direBoarStateMachine.ChangeState(DeadState);
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
            direBoarStats.health -= damageValue;

            Anim.SetTrigger("HurtTrigger");

            StartCoroutine("WaitForHurt");
        }
    }

    public void DetectObstacle()
    {
        // Check whether there is any obstacle around the player
        hasObstacle = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsObstacle);
        //Debug.Log("Check");
    }

    public void DetectPlayer()
    {
        // Check whether player is around the enemy.
        isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);

        return;
    }
    public void ShockMotion()
    {
        // IF no obstacle THEN check whether player is around the enemy
        if (!hasObstacle)
        {
            isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);
        }
    }


    public void PrepareCharge()
    {
        StartCoroutine("WaitForCharge");
    }


    public void FinishCharge()
    {
        StartCoroutine("ChargeCD");
    }

    public void ChargePlayer()
    {
        // Move enemy to the last player position
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);
    }



    public void FlipDirection()
    {
        if (aiPath.velocity.x >= 0.01 && !facingRight || aiPath.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public void ChargeAttack()
    {
        // Move enemy to the last player position.
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);

        return;
    }

    IEnumerator WaitForCharge()
    {
        isCharging = true;

        yield return new WaitForSeconds(1.5f);

        // Change to CHARGE STATE
        direBoarStateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(2.5f);

        // Change charge status to FALSE for next execution
        hasCharged = false;
    }




    IEnumerator WaitForHurt()
    {
        isHurt = true;

        yield return new WaitForSeconds(0.05f);

        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(shockArea.position, shockRadius);
    }
}
