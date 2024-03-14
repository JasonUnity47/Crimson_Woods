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
    [Header("Shock State")]
    [SerializeField] private Transform shockArea;
    [SerializeField] private float shockRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;
    public bool isShocked = false;
    public bool hasObstacle = false;

    [Header("Charge State")]
    public Vector2 lastTargetPos;
    [SerializeField] private float chargeSpeed;
    public bool isCharging = false;
    public bool hasCharged = false;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // State Machine
    public Boss2StateMachine boss2StateMachine { get; private set; }

    public Boss2IdleState IdleState { get; private set; }

    public Boss2ChaseState ChaseState { get; private set; }

    public Boss2ShockState ShockState { get; private set; }

    public Boss2ChargeState ChargeState { get; private set; }

    // Script Reference
    private Boss2Stats boss2Stats;

    public Boss2Movement boss2Movement { get; private set; }

    private void Awake()
    {
        boss2StateMachine = new Boss2StateMachine();

        boss2Stats = GetComponent<Boss2Stats>(); // Get reference before other states

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
        transform.position = Vector2.Lerp((Vector2)transform.position, lastTargetPos, chargeSpeed * Time.deltaTime);
    }

    IEnumerator WaitForCharge()
    {
        isCharging = true;

        yield return new WaitForSeconds(3f);

        // Change to CHARGE STATE
        boss2StateMachine.ChangeState(ChargeState);
    }

    IEnumerator ChargeCD()
    {
        yield return new WaitForSeconds(6f);

        // Change charge status to FALSE for next execution
        hasCharged = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(shockArea.position, shockRadius);
    }
}
