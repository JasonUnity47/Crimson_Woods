using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
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

    [Header("Meele Attack State")]
    [SerializeField] private Transform meeleArea;
    [SerializeField] private float meeleRadius;
    public bool isMeeleAttack = false;
    public bool hasMeeleAttacked = false;

    public Boss1StateMachine StateMachine { get; private set; }

    public Boss1IdleState IdleState { get; private set; }
    public Boss1ChaseState ChaseState { get; private set; }
    public Boss1ShockState ShockState { get; private set; }
    public Boss1ChargeState ChargeState { get; private set; }
    public Boss1MeeleAttackState MeeleState { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playerPos { get; private set; }
    private Boss1Data boss1Data;

    public Boss1Movement boss1Movement { get; private set; }

    private void Awake()
    {
        StateMachine = new Boss1StateMachine();

        boss1Data = GetComponent<Boss1Data>();

        IdleState = new Boss1IdleState(this, StateMachine, boss1Data, "idle");
        ChaseState = new Boss1ChaseState(this, StateMachine, boss1Data, "chase");
        ShockState = new Boss1ShockState(this, StateMachine, boss1Data, "shock");
        ChargeState = new Boss1ChargeState(this, StateMachine, boss1Data, "charge");
        MeeleState = new Boss1MeeleAttackState(this, StateMachine, boss1Data, "meele");
    }

    private void Start()
    {
        boss1Movement = GetComponent<Boss1Movement>();
        aiPath = GetComponent<AIPath>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        FlipDirection();

        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void DetectObstacle()
    {
        // Check whether there is any obstacle around the player
        hasObstacle = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsObstacle);
    }

    public void DetectPlayer()
    {
        // Check whether player is around the enemy.
        isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);

        return;
    }

    public void FlipDirection()
    {
        if (Rb.velocity.x >= 0.01 && !facingRight || Rb.velocity.x <= -0.01 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

        return;
    }


    public void PrepareCharge()
    {
        StartCoroutine(WaitForCharge());

        return;
    }

    public void FinishCharge()
    {
        StartCoroutine(ChargeCD());

        return;
    }

    public void IsMeeleAttacking()
    {
        StartCoroutine(MeeleAttacking());

        return;
    }

    public void FinishMeeleAttacked()
    {
        StartCoroutine(MeeleCD());

        return;
    }

    public void ChargePlayer()
    {
        // Move enemy to the last player position
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);

        return;
    }

    public void MeeleArea()
    {
        // Check if player is in area
        isMeeleAttack = Physics2D.OverlapCircle(shockArea.position, meeleRadius, whatIsPlayer);
    }


    IEnumerator WaitForCharge()
    {
        isCharging = true;

        yield return new WaitForSeconds(enterChargeTime);

        // Change to CHARGE STATE
        StateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(chargeCD);

        // Change charge status to FALSE for next execution
        hasCharged = false;
    }

    IEnumerator MeeleAttacking()
    {
        Animator.SetTrigger("meele");
        yield return new WaitForSeconds(1f);

        StateMachine.ChangeState(IdleState);
    }

    IEnumerator MeeleCD()
    {
        yield return new WaitForSeconds(2.5f);

        // Change Meele Attack status to FALSE for next execution
        hasMeeleAttacked = false;
    }
}
