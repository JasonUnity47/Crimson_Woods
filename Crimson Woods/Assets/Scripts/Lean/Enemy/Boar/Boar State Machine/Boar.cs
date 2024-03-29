using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boar : MonoBehaviour
{
    // Declaration
    SpriteRenderer SR;

    // Variable
    [Header("Movement")]
    public bool facingRight = true;
    public bool isHurt = false;
    public bool isDead = false;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // State Machine
    public BoarStateMachine boarStateMachine { get; private set; }

    public BoarIdleState IdleState { get; private set; }

    public BoarChaseState ChaseState { get; private set; }

    public BoarDeadState DeadState { get; private set; }

    // Script Reference
    private BoarStats boarStats;

    public BoarMovement boarMovement { get; private set; }

    public LootBag lootBag { get; private set; }

    private void Awake()
    {
        boarStateMachine = new BoarStateMachine();

        lootBag = GetComponent<LootBag>();

        boarStats = GetComponent<BoarStats>(); // Get reference before other states

        IdleState = new BoarIdleState(this, boarStateMachine, boarStats, "BoarIdle");
        ChaseState = new BoarChaseState(this, boarStateMachine, boarStats, "BoarChase");
        DeadState = new BoarDeadState(this, boarStateMachine, boarStats, "BoarDead");
    }

    private void Start()
    {
        boarMovement = GetComponent<BoarMovement>();

        SR = GetComponent<SpriteRenderer>();

        Rb = GetComponent<Rigidbody2D>();

        Anim = GetComponent<Animator>();

        boarStateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        boarStateMachine.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        boarStateMachine.CurrentState.PhysicsUpdate();
    }


    //Attack Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    public void CheckDead()
    {
        if (boarStats.health <= 0)
        {
            isDead = true;
            isHurt = true;
            Rb.velocity = Vector2.zero;
            boarStats.health = 0;

            boarStateMachine.ChangeState(DeadState);
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
            boarStats.health -= damageValue;


            StartCoroutine("WaitForHurt");
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

    IEnumerator WaitForHurt()
    {
        isHurt = true;

        yield return new WaitForSeconds(0.1f);

        isHurt = false;
    }


}