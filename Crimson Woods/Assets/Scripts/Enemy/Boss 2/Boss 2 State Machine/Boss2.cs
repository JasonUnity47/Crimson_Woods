using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    // Declaration
    // Movement
    [Header("Movement")]
    public bool facingRight = true;

    // Health
    [Header("Health")]
    public float health;
    public float maxHealth;
    public float hurtTime;
    public bool isHurt = false;
    public bool isDead = false;

    // Shock State
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

    // Prepare State
    [Header("Prepare State")]
    [SerializeField] private Transform rangeArea;
    [SerializeField] private float rangeRadius;
    [SerializeField] private GameObject prepareSlashVFX;
    public float enterSlashTime;
    public bool isPrepared = false;
    public bool farEnough = false;

    // Range State
    [Header("Range State")]
    public float slashCD;
    public float rangeToSlash;
    public float originalMovespeed;
    public Vector2 lastTargetPosForSlash;
    [SerializeField] GameObject waterSlash;
    [SerializeField] GameObject freezeEffect;
    public bool isSlashing = false;
    public bool hasSlashed = false;

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
    public Boss2Stats boss2Stats { get; private set; }

    public LootBag lootBag { get; private set; }

    public AIPath aiPath {  get; private set; }

    public Transform playerPos { get; private set; }

    public PlayerController playerController { get; private set; }

    public SpriteRenderer playerSprite {  get; private set; }

    private void Awake()
    {
        boss2StateMachine = new Boss2StateMachine();

        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        boss2Stats = GetComponent<Boss2Stats>();
        lootBag = GetComponent<LootBag>();
        aiPath = GetComponent<AIPath>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

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
        // Initialize the first state.
        boss2StateMachine.InitializeState(IdleState);

        // Initialize the enemy health.
        health = maxHealth;
    }

    private void Update()
    {
        boss2StateMachine.CurrentState.LogicalUpdate();

        // Keep checking the player position in the game with a certain amount of time.
        TrackPlayer();

        // Check whether the enemy is dead.
        CheckDead();

        // Check whether the enemy should flip to the correct facing direction.
        FlipDirection();
    }

    private void FixedUpdate()
    {
        boss2StateMachine.CurrentState.PhysicsUpdate();
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
            isDead = true;
            isHurt = true; // Prevent continuous damage from the player.
            health = 0;

            // If the enemy is dead then change to Dead State.
            boss2StateMachine.ChangeState(DeadState);
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
        // If the enemy is dead then do nothing.
        if (isDead)
        {
            return;
        }

        // If the enemy has not damaged before then take the damage.
        if (!isHurt)
        {
            boss2Stats.health -= damageValue;

            Anim.SetTrigger("HurtTrigger");

            StartCoroutine("WaitForHurt");
        }

        return;
    }

    public void DetectObstacle()
    {
        // Check whether there is any obstacle around the player.
        hasObstacle = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsObstacle);

        return;
    }

    public void DetectPlayer()
    {
        // Check whether player is around the enemy.
        isShocked = Physics2D.OverlapCircle(shockArea.position, shockRadius, whatIsPlayer);

        return;
    }

    public void DetectSlash()
    {
        // If the enemy didn't slash the player before and the player is not within the shock area
        // then check whether the player is within the range area.
        if (!hasSlashed && !isShocked)
        {
            // Check whether player is in the area.
            isPrepared = Physics2D.OverlapCircle(rangeArea.position, rangeRadius, whatIsPlayer);
        }

        else
        {
            isPrepared = false;
        }

        return;
    }

    public void FarPlayer()
    {
        // Check whether the player is far enough for the enemy to slash the player.
        if (Vector2.Distance(transform.position, playerPos.position) > rangeToSlash)
        {
            farEnough = true;
        }

        else
        {
            farEnough = false;
        }

        return;
    }

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

    public void SlashAttack()
    {
        // If the enemy didn't slash the player before then slash the player with water slash.
        if (!hasSlashed)
        {
            hasSlashed = true;

            // Slash the player.
            Instantiate(waterSlash, transform.position, Quaternion.identity); // Water slash.
        }

        return;
    }

    public void FreezePlayer()
    {
        StartCoroutine(FreezeTime());
    }

    public void SlashPlayer()
    {
        StartCoroutine(SlashTime());
    }

    IEnumerator SlashTime()
    {
        // Wait for certain seconds for the slash attack animation.
        yield return new WaitForSeconds(0.4f);

        SlashAttack();
    }

    IEnumerator FreezeTime()
    {
        GameObject freeze = Instantiate(freezeEffect, playerPos.position, Quaternion.identity, playerPos); // Freeze effect.

        float moveSpeedDecrement = playerController.moveSpeed * (50 / 100f);
        originalMovespeed = playerController.moveSpeed;

        playerController.moveSpeed -= moveSpeedDecrement;
        playerSprite.color = Color.blue;


        if (freeze.GetComponent<Animator>() != null)
        {
            Animator freezeAnim = freeze.GetComponent<Animator>();

            yield return new WaitForSeconds(0.3f);
            freezeAnim.SetBool("Start", true);

            yield return new WaitForSeconds(0.3f);
            freezeAnim.SetBool("Active", true);

            Destroy(freeze, 0.5f);
        }
    }

    IEnumerator WaitForHurt()
    {
        // Given the enemy a short period to prevent continuous damage.
        isHurt = true;

        yield return new WaitForSeconds(hurtTime);

        isHurt = false;
    }

    IEnumerator WaitForCharge()
    {
        // Set a predefined time to allow the enemy change to the next state.
        isCharging = true;

        yield return new WaitForSeconds(enterChargeTime);

        // Change to Chrage State.
        boss2StateMachine.ChangeState(ChargeState);
    }

    IEnumerator WaitForSlash()
    {
        // Set a predefined time to allow the enemy change to the next state.

        // VFX to show the enemy is ready to slash the player with water slash.
        Instantiate(prepareSlashVFX, transform.position, Quaternion.identity, this.transform);

        isSlashing = true;

        yield return new WaitForSeconds(enterSlashTime);

        // Change to Range State.
        boss2StateMachine.ChangeState(RangeState);
    }

    IEnumerator ChargeCD()
    {
        // Wait X seconds to refresh the charge action.
        yield return new WaitForSeconds(chargeCD);

        // Change charge status to false for next execution.
        hasCharged = false;
    }

    IEnumerator SlashCD()
    {
        // Wait 10 seconds to refresh the slash action.
        yield return new WaitForSeconds(slashCD);

        // Change slash status to false for next execution.
        hasSlashed = false;
    }

    public void PrepareCharge()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(WaitForCharge());

        return;
    }

    public void PrepareSlash()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(WaitForSlash());

        return;
    }

    public void FinishCharge()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(ChargeCD());

        return;
    }

    public void FinishSlash()
    {
        // Allow enemy states can use the functions from the boss2.
        StartCoroutine(SlashCD());

        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rangeArea.position, rangeRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(shockArea.position, shockRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(1f);
        }
    }
}
