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
    public Vector2 lastTargetPosForCharge;
    [SerializeField] private float chargeSpeed;
    public bool isCharging = false;
    public bool hasCharged = false;


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

    public DireBoarMovement direBoarMovement { get; private set; }

    public LootBag lootBag { get; private set; }

    private void Awake()
    {
        direBoarStateMachine = new DireBoarStateMachine();

        lootBag = GetComponent<LootBag>();

        direBoarStats = GetComponent<DireBoarStats>(); // Get reference before other states

        IdleState = new DireBoarIdleState(this, direBoarStateMachine, direBoarStats, "IdleBool");
        ChaseState = new DireBoarChaseState(this, direBoarStateMachine, direBoarStats, "ChaseBool");
        ShockState = new DireBoarShockState(this, direBoarStateMachine, direBoarStats, "ShockBool");
        ChargeState = new DireBoarChargeState(this, direBoarStateMachine, direBoarStats, "ChargeBool");
        DeadState = new DireBoarDeadState(this, direBoarStateMachine, direBoarStats, "DeadBool");
    }

    private void Start()
    {
        direBoarMovement = GetComponent<DireBoarMovement>();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    public void CheckDead()
    {
        if (direBoarStats.health <= 0)
        {
            isDead = true;
            isHurt = true;
            Rb.velocity = Vector2.zero;
            direBoarStats.health = 0;
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
        if (Rb.velocity.x >= 0.01 && !facingRight || Rb.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
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
