using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [Header("Movement")]
    public bool facingRight = true;

    // Health
    [Header("Health")]
    public float health;
    public float maxHealth;
    public float hurtTime;
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
    [SerializeField] private GameObject MeleeAreaAttackVFX;
    public bool isMeeleAttack = false;
    public bool hasMeeleAttacked = false;

    // Check
    [Header("Check")]
    public float checkTime;
    private float timeBtwEachCheck;

    // Loot
    [Header("Loot")]
    public int lootCount;

    public Boss1StateMachine StateMachine { get; private set; }

    public Boss1IdleState IdleState { get; private set; }
    public Boss1ChaseState ChaseState { get; private set; }
    public Boss1ShockState ShockState { get; private set; }
    public Boss1ChargeState ChargeState { get; private set; }
    public Boss1MeeleAttackState MeeleState { get; private set; }
    public Boss1DeadState DeadState { get; private set; }

    private Boss1Data boss1Data;

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    // Script Reference
    public LootBag lootBag { get; private set; }
    public AIPath aiPath { get; private set; }
    public Transform playerPos { get; private set; }
    public BuffContent buffContent { get; private set; }

    private void Awake()
    {
        StateMachine = new Boss1StateMachine();

        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        boss1Data = GetComponent<Boss1Data>();

        lootBag = GetComponent<LootBag>();
        aiPath = GetComponent<AIPath>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new Boss1IdleState(this, StateMachine, boss1Data, "idle");
        ChaseState = new Boss1ChaseState(this, StateMachine, boss1Data, "chase");
        ShockState = new Boss1ShockState(this, StateMachine, boss1Data, "shock");
        ChargeState = new Boss1ChargeState(this, StateMachine, boss1Data, "charge");
        MeeleState = new Boss1MeeleAttackState(this, StateMachine, boss1Data, "meele");
        DeadState = new Boss1DeadState(this, StateMachine, boss1Data, "dead");
    }

    private void Start()
    {
        // Initialize the first state.
        StateMachine.Initialize(IdleState);

        // Initialize the enemy health.
        health = maxHealth;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        // Keep checking the player position in the game with a certain amount of time.
        TrackPlayer();

        // Check whether the enemy is dead.
        CheckDead();

        // Check whether the enemy should flip to the correct facing direction.
        FlipDirection();

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
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
            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }

            // Change the "Enemy" tag to "Untagged" tag to disable all the scripts that need "Enemy" tag to prevent after-dead issues.
            tag = "Untagged";
            Physics2D.IgnoreLayerCollision(6, 7);
            isDead = true;
            isHurt = true; // Prevent continuous damage from the player.
            health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            // If the enemy is dead then change to Dead State.
            StateMachine.ChangeState(DeadState);
        }

        if (isDead && StateMachine.CurrentState != DeadState)
        {
            // If the enemy is dead then change to Dead State.
            StateMachine.ChangeState(DeadState);
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
            health -= damageValue;

            Animator.SetTrigger("hurt");
            Debug.Log("hurt");
            StartCoroutine("WaitForHurt");
        }

        return;
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

    }

    public void FlipDirection()
    {
        if (aiPath.velocity.x >= 0.01 && !facingRight || aiPath.velocity.x <= -0.01 && facingRight)
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
        GameObject MeeleAreaAttack = Instantiate(MeleeAreaAttackVFX, transform.position, Quaternion.identity);
        Destroy(MeeleAreaAttack, 5f);

        StateMachine.ChangeState(IdleState);
    }

    IEnumerator MeeleCD()
    {
        yield return new WaitForSeconds(2.5f);

        // Change Meele Attack status to FALSE for next execution
        hasMeeleAttacked = false;
    }

    IEnumerator WaitForHurt()
    {
        // Given the enemy a short period to prevent continuous damage.
        isHurt = true;

        yield return new WaitForSeconds(hurtTime);

        isHurt = false;
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
