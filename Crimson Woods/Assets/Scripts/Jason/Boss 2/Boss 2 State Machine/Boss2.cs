using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boss2 : MonoBehaviour
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

    [Header("Prepare State")]
    [SerializeField] private Transform rangeArea;
    [SerializeField] private float rangeRadius;
    [SerializeField] private GameObject prepareSlashVFX;
    public bool isPrepared = false;
    public bool farEnough = false;

    [Header("Range State")]
    public Vector2 lastTargetPosForSlash;
    [SerializeField] GameObject waterSlash;
    public bool isSlashing = false;
    public bool hasSlashed = false;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // State Machine
    public Boss2StateMachine boss2StateMachine { get; private set; }

    public Boss2IdleState IdleState { get; private set; }

    public Boss2ChaseState ChaseState { get; private set; }

    public Boss2ShockState ShockState { get; private set; }

    public Boss2ChargeState ChargeState { get; private set; }

    public Boss2PrepareState PrepareState { get; private set; }

    public Boss2RangeState RangeState { get; private set; }

    public Boss2DeadState DeadState { get; private set; }

    // Script Reference
    private Boss2Stats boss2Stats;

    public Boss2Movement boss2Movement { get; private set; }

    public LootBag lootBag { get; private set; }

    private void Awake()
    {
        boss2StateMachine = new Boss2StateMachine();

        lootBag = GetComponent<LootBag>();

        boss2Stats = GetComponent<Boss2Stats>(); // Get reference before other states

        IdleState = new Boss2IdleState(this, boss2StateMachine, boss2Stats, "IdleBool");
        ChaseState = new Boss2ChaseState(this, boss2StateMachine, boss2Stats, "ChaseBool");
        ShockState = new Boss2ShockState(this, boss2StateMachine, boss2Stats, "ShockBool");
        ChargeState = new Boss2ChargeState(this, boss2StateMachine, boss2Stats, "ChargeBool");
        PrepareState = new Boss2PrepareState(this, boss2StateMachine, boss2Stats, "PrepareBool");
        RangeState = new Boss2RangeState(this, boss2StateMachine, boss2Stats, "SlashTrigger");
        DeadState = new Boss2DeadState(this, boss2StateMachine, boss2Stats, "DeadBool");
    }

    private void Start()
    {
        boss2Movement = GetComponent<Boss2Movement>();

        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        boss2StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        boss2StateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        boss2StateMachine.CurrentState.PhysicsUpdate();
    }

    public void CheckDead()
    {
        if (boss2Stats.health <= 0)
        {
            isDead = true;
            isHurt = true;
            Rb.velocity = Vector2.zero;
            boss2Stats.health = 0;
            boss2StateMachine.ChangeState(DeadState);
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
            boss2Stats.health -= damageValue;

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

    public void PrepareMotion()
    {
        if (!hasSlashed && !isShocked)
        {
            // Check whether player is in the area
            isPrepared = Physics2D.OverlapCircle(rangeArea.position, rangeRadius, whatIsPlayer);
        }

        else
        {
            isPrepared = false;
        }
    }

    public void FarPlayer()
    {
        if (Vector2.Distance(transform.position, boss2Movement.targetPos.position) > boss2Movement.rangeDistance)
        {
            farEnough = true;
        }

        else
        {
            farEnough = false;
        }
    }

    public void PrepareCharge()
    {
        StartCoroutine("WaitForCharge");
    }

    public void PrepareSlash()
    {
        StartCoroutine("WaitForSlash");
    }

    public void BeforeSlash()
    {
        StartCoroutine("SlashAnimation");
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

    public void SlashAttack()
    {
        hasSlashed = true;

        Instantiate(waterSlash, transform.position, Quaternion.identity);
    }

    public void FinishSlash()
    {
        StartCoroutine("SlashCD");
    }

    public void LoseEssence()
    {
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
        boss2StateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(2.5f);

        // Change charge status to FALSE for next execution
        hasCharged = false;
    }

    IEnumerator WaitForSlash()
    {
        Instantiate(prepareSlashVFX, transform.position, Quaternion.identity, this.transform);

        isSlashing = true;

        yield return new WaitForSeconds(1.2f);

        // Change to RANGE STATE
        boss2StateMachine.ChangeState(RangeState);
    }

    IEnumerator SlashAnimation()
    {
        yield return new WaitForSeconds(0.4f);

        SlashAttack();
    }

    IEnumerator SlashCD()
    {
        yield return new WaitForSeconds(10f);

        // Change slash status to FALSE for next execution
        hasSlashed = false;
    }

    IEnumerator WaitForHurt()
    {
        isHurt = true;

        yield return new WaitForSeconds(0.05f);

        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rangeArea.position, rangeRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(shockArea.position, shockRadius);
    }
}
