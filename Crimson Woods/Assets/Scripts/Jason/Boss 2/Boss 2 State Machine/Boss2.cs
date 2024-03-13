using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    public Boss2StateMachine boss2StateMachine { get; private set; }
    private Boss2Stats boss2Stats;

    public Boss2IdleState IdleState { get; private set; }
    public Boss2ChaseState ChaseState { get; private set; }

    public Boss2ShockState ShockState { get; private set; }

    public Boss2ChargeState ChargeState { get; private set; }

    public Boss2Movement boss2Movement { get; private set; }

    public Transform shockArea;
    public float shockRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;

    public bool isShocked = false;
    public bool hasObstacle = false;

    private void Awake()
    {
        boss2StateMachine = new Boss2StateMachine();

        boss2Stats = GetComponent<Boss2Stats>();

        IdleState = new Boss2IdleState(this, boss2StateMachine, boss2Stats, "IdleBool");
        ChaseState = new Boss2ChaseState(this, boss2StateMachine, boss2Stats, "ChaseBool");
        ShockState = new Boss2ShockState(this, boss2StateMachine, boss2Stats, "ShockBool");
        ChargeState = new Boss2ChargeState(this, boss2StateMachine, boss2Stats, "ChargeBool");
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
        boss2StateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        boss2StateMachine.CurrentState.PhysicsUpdate();
    }

    public void ShockMotion()
    {
        if (!hasObstacle)
        {
            isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);
        }
    }

    public void DetectObstacle()
    {
        hasObstacle = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsObstacle);
    }

    public void ChangeToChargeState()
    {
        StartCoroutine("WaitForCharge");
    }

    public void ChangeToIdleState()
    {
        StartCoroutine("ChargeCD");
    }

    public void ChargePlayer()
    {
        if (Vector2.Distance(Rb.position, boss2Movement.targetPos.position) > boss2Movement.stoppingDistance)
        {
            transform.position = Vector2.Lerp(transform.position, boss2Movement.targetPos.position, 10 * Time.deltaTime);
        }

        else
        {
            ChargeState.hasCharged = true;
        }
    }

    IEnumerator WaitForCharge()
    {
        ShockState.isCharging = true;

        yield return new WaitForSeconds(3);

        boss2StateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(6);
        ChargeState.hasCharged = false;
    }
}
