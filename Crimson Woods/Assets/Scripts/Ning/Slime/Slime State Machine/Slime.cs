using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // Declaration
    private UnityEngine.Object explosionRef;
    private Material matWhite;
    private Material matDefault;
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
    public SlimeStateMachine slimeStateMachine { get; private set; }

    public SlimeIdleState IdleState { get; private set; }

    public SlimeChaseState ChaseState { get; private set; }

    public SlimeDeadState DeadState { get; private set; }

    // Script Reference
    private SlimeStats slimeStats;

    public SlimeMovement slimeMovement { get; private set; }

    public LootBag lootBag { get; private set; }

    private void Awake()
    {
        slimeStateMachine = new SlimeStateMachine();

        lootBag = GetComponent<LootBag>();

        explosionRef = Resources.Load("Prefab/Explode/SlimeExplode");

        slimeStats = GetComponent<SlimeStats>(); // Get reference before other states

        IdleState = new SlimeIdleState(this, slimeStateMachine, slimeStats, "SlimeIdle");
        ChaseState = new SlimeChaseState(this, slimeStateMachine, slimeStats, "SlimeChase");
        DeadState = new SlimeDeadState(this, slimeStateMachine, slimeStats, "SlimeDead");
    }

    private void Start()
    {
        slimeMovement = GetComponent<SlimeMovement>();

        SR = GetComponent<SpriteRenderer>();

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
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
            isDead = true;
            isHurt = true;
            Rb.velocity = Vector2.zero;
            slimeStats.health = 0;

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
            Invoke("ResetMaterial", 0.05f);
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

        yield return new WaitForSeconds(0.05f);

        isHurt = false;
    }

    void ResetMaterial()
    {
        SR.material = matDefault;
    }
}
