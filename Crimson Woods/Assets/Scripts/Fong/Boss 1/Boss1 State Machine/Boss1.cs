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

    public void ShockMotion()
    {
        // IF no obstacle THEN check whether player is around the enemy
        if (!hasObstacle)
        {
            isShocked = Physics2D.OverlapCircle(meeleArea.position, shockRadius, whatIsPlayer);
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


    public void PrepareCharge()
    {
        StartCoroutine("WaitForCharge");
    }

    public void FinishCharge()
    {
        StartCoroutine("ChargeCD");
    }

    public void IsMeeleAttacking()
    {
        StartCoroutine("MeeleAttacking");
    }

    public void FinishMeeleAttacked()
    {
        StartCoroutine("MeeleCD");
    }

    public void ChargePlayer()
    {
        // Move enemy to the last player position
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPosForCharge, chargeSpeed * Time.deltaTime);
    }

    public void MeeleArea()
    {
        // Check if player is in area
        isMeeleAttack = Physics2D.OverlapCircle(shockArea.position, meeleRadius, whatIsPlayer);
    }


    IEnumerator WaitForCharge()
    {
        isCharging = true;

        yield return new WaitForSeconds(1.5f);

        // Change to CHARGE STATE
        StateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(2.5f);

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
