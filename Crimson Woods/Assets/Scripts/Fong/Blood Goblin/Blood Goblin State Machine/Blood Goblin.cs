using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BloodGoblin : MonoBehaviour
{
    // Declaration
    
    // Damage
    [Header("Damage")]
    public float damage;

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
    [SerializeField] private ParticleSystem dust;
    public bool isCharging = false;
    public bool hasCharged = false;

    // Prepare State
    [Header("Prepare State")]
    [SerializeField] private Transform rangeArea;
    [SerializeField] private float rangeRadius;
    [SerializeField] private GameObject prepareThrowVFX;
    public float enterThrowTime;
    public bool isPrepared = false;
    public bool farEnough = false;

    // Range State
    [Header("Range State")]
    public float throwCD;
    public float rangeToThrow;
    public float originalMovespeed;
    public Vector2 lastTargetPosForThrow;
    [SerializeField] GameObject ThrowWeapon;
    public bool isThrowing = false;
    public bool hasThrowed = false;

    [Header("Meele Attack State")]
    [SerializeField] private Transform meeleArea;
    [SerializeField] private float meeleRadius;
    [SerializeField] private GameObject MeleeAreaAttackVFX;
    [SerializeField] private GameObject explosionRef;
    public bool isMeeleAttack = false;
    public bool hasMeeleAttacked = false;

    // Check
    [Header("Check")]
    public float checkTime;
    private float timeBtwEachCheck;

    // Loot
    [Header("Loot")]
    public int lootCount;

    // Light
    [Header("Light")]
    [SerializeField] private GameObject selfLight;

    // Component
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    public Collider2D[] col;

    private SpriteRenderer spriteRenderer;

    // State Machine
    public BloodGoblinStateMachine bloodGoblinStateMachine { get; private set; }

    public BloodGoblinIdleState IdleState { get; private set; }

    public BloodGoblinChaseState ChaseState { get; private set; }

    public BloodGoblinShockState ShockState { get; private set; }

    public BloodGoblinChargeState ChargeState { get; private set; }

    public BloodGoblinMeeleAttackState MeeleState { get; private set; }

    public BloodGoblinPrepareState PrepareState { get; private set; }

    public BloodGoblinRangeState RangeState { get; private set; }

    public BloodGoblinDeadState DeadState { get; private set; }

    // Script Reference
    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playerPos { get; private set; }

    public BuffContent buffContent { get; private set; }

    public AudioSource myAudio;
    public AudioClip BloodBoss1HurtSFX;
    public AudioClip BloodBoss1DieSFX;
    public AudioClip BloodBoss1ChargeSFX;
    public AudioClip BloodBoss1EarthquakeSFX;
    public AudioClip BloodBoss1AxeChargeSFX;
    public AudioClip BloodBoss1AxeThrowSFX;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();

        bloodGoblinStateMachine = new BloodGoblinStateMachine();

        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lootBag = GetComponent<LootBag>();
        aiPath = GetComponent<AIPath>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new BloodGoblinIdleState(this, bloodGoblinStateMachine, "idle");
        ChaseState = new BloodGoblinChaseState(this, bloodGoblinStateMachine, "chase");
        ShockState = new BloodGoblinShockState(this, bloodGoblinStateMachine, "shock");
        ChargeState = new BloodGoblinChargeState(this, bloodGoblinStateMachine, "charge");
        MeeleState = new BloodGoblinMeeleAttackState(this, bloodGoblinStateMachine, "meele");
        PrepareState = new BloodGoblinPrepareState(this, bloodGoblinStateMachine, "prepare");
        RangeState = new BloodGoblinRangeState(this, bloodGoblinStateMachine, "throw");
        DeadState = new BloodGoblinDeadState(this, bloodGoblinStateMachine, "dead");

    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the first state.
        bloodGoblinStateMachine.InitializeState(IdleState);

        // Initialize the enemy health.
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        bloodGoblinStateMachine.CurrentState.LogicalUpdate();

        // Keep checking the player position in the game with a certain amount of time.
        TrackPlayer();

        // Check whether the enemy is dead.
        CheckDead();

        // Check whether the enemy should flip to the correct facing direction.
        FlipDirection();
    }

    private void FixedUpdate()
    {
        bloodGoblinStateMachine.CurrentState.PhysicsUpdate();
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
            myAudio.PlayOneShot(BloodBoss1DieSFX);

            // Hide the self light if the enemy is dead.
            selfLight.SetActive(false);

            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }

            // Change the "Enemy" tag to "Untagged" tag to disable all the scripts that need "Enemy" tag to prevent after-dead issues.
            tag = "Untagged";
            //Physics2D.IgnoreLayerCollision(6, 7);

            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].enabled == true)
                {
                    col[i].enabled = false;
                }
            }

            isDead = true;
            isHurt = true; // Prevent continuous damage from the player.
            health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            spriteRenderer.sortingOrder = 9;

            // If the enemy is dead then change to Dead State.
            bloodGoblinStateMachine.ChangeState(DeadState);
        }

        if (isDead && bloodGoblinStateMachine.CurrentState != DeadState)
        {
            // If the enemy is dead then change to Dead State.
            bloodGoblinStateMachine.ChangeState(DeadState);
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
            myAudio.PlayOneShot(BloodBoss1HurtSFX);

            health -= damageValue;


            Anim.SetTrigger("hurt");

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

    public void MeeleArea()
    {
        // Check if player is in area
        isMeeleAttack = Physics2D.OverlapCircle(meeleArea.position, meeleRadius, whatIsPlayer);
    }

    public void DetectThrow()
    {
        // If the enemy didn't throw the player before and the player is not within the shock area
        // then check whether the player is within the range area.
        if (!hasThrowed && !isShocked)
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
        if (Vector2.Distance(transform.position, playerPos.position) > rangeToThrow)
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
        if ((aiPath.velocity.x >= 0.01 && !facingRight) || (aiPath.velocity.x <= -0.01 && facingRight))
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

    public void ThrowAttack()
    {
        // If the enemy didn't throw weapon to the player before then throw the player with weapon.
        if (!hasThrowed)
        {
            hasThrowed = true;

            // Slash the player.
            Instantiate(ThrowWeapon, transform.position, Quaternion.identity);
        }

        return;
    }

    public void ThrowPlayer()
    {
        StartCoroutine(ThrowTime());
    }

    public void CreateDust()
    {
        dust.Play();
    }

    IEnumerator ThrowTime()
    {       
        // Wait for certain seconds for the slash attack animation.
        yield return new WaitForSeconds(0.4f);

        ThrowAttack();
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
        bloodGoblinStateMachine.ChangeState(ChargeState);
    }

    IEnumerator WaitForThrow()
    {
        // Set a predefined time to allow the enemy change to the next state.

        // VFX to show the enemy is ready to throw the player with weapon.
        GameObject prepapreEffect = Instantiate(prepareThrowVFX, transform.position, Quaternion.identity, this.transform);

        Destroy(prepapreEffect, 1.1f);

        isThrowing = true;

        yield return new WaitForSeconds(enterThrowTime);

        // Change to Range State.
        bloodGoblinStateMachine.ChangeState(RangeState);
    }

    IEnumerator ChargeCD()
    {
        // Wait X seconds to refresh the charge action.
        yield return new WaitForSeconds(chargeCD);

        // Change charge status to false for next execution.
        hasCharged = false;
    }

    IEnumerator ThrowCD()
    {
        // Wait 10 seconds to refresh the throw action.
        yield return new WaitForSeconds(throwCD);

        // Change throw status to false for next execution.
        hasThrowed = false;
    }

    public void PrepareCharge()
    {
        myAudio.PlayOneShot(BloodBoss1ChargeSFX);

        // Allow enemy states can use the functions from the BloodGoblin.
        StartCoroutine(WaitForCharge());

        return;
    }

    public void PrepareThrow()
    {
        myAudio.PlayOneShot(BloodBoss1AxeChargeSFX);
        // Allow enemy states can use the functions from the BloodGoblin.
        StartCoroutine(WaitForThrow());

        return;
    }

    public void FinishCharge()
    {
        // Allow enemy states can use the functions from the BloodGoblin.
        StartCoroutine(ChargeCD());

        return;
    }

    public void FinishThrow()
    {
        myAudio.PlayOneShot(BloodBoss1AxeThrowSFX);
        // Allow enemy states can use the functions from the BloodGoblin.
        StartCoroutine(ThrowCD());

        return;
    }

    public void IsMeeleAttacking()
    {
        StartCoroutine(MeeleAttacking());

        myAudio.PlayOneShot(BloodBoss1EarthquakeSFX);

        return;
    }

    public void FinishMeeleAttacked()
    {
        StartCoroutine(MeeleCD());

        return;
    }

    IEnumerator MeeleAttacking()
    {
        Anim.SetTrigger("meele");
        yield return new WaitForSeconds(0.8f);
        GameObject MeeleAreaAttack = Instantiate(MeleeAreaAttackVFX, transform.position, Quaternion.identity);
        GameObject explosion = Instantiate(explosionRef, transform.position, Quaternion.identity);
        Destroy(MeeleAreaAttack, 5f);

        bloodGoblinStateMachine.ChangeState(IdleState);
    }

    IEnumerator MeeleCD()
    {
        yield return new WaitForSeconds(2.5f);

        // Change Meele Attack status to FALSE for next execution
        hasMeeleAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(damage);
        }
    }
}
