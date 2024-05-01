using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Pathfinding;

public class Slime1 : MonoBehaviour
{
    // Declaration
    private UnityEngine.Object explosionRef;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer SR;

    // Damage
    [Header("Damage")]
    public float damage;

    // Variable
    [Header("Movement")]
    public bool facingRight = true;

    public bool isHurt = false;
    public bool isDead = false;

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
    public SlimeStateMachine1 slimeStateMachine1 { get; private set; }

    public SlimeIdleState1 IdleState { get; private set; }

    public SlimeChaseState1 ChaseState { get; private set; }

    public SlimeDeadState1 DeadState { get; private set; }

    // Script Reference
    private SlimeStats1 slimeStats1;

   

    public LootBag lootBag { get; private set; }

    public AIPath aiPath { get; private set; }

    public Transform playPos { get; private set; }

    public BuffContent buffContent { get; private set; }

    public AudioSource myAudio;
    public AudioClip Slime1HurtSFX;
    public AudioClip Slime1DieSFX;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();

        slimeStateMachine1 = new SlimeStateMachine1();

        lootBag = GetComponent<LootBag>();

        aiPath = GetComponent<AIPath>();

        explosionRef = Resources.Load("Prefab/Explode/SlimeExplode 1");

        slimeStats1 = GetComponent<SlimeStats1>(); // Get reference before other states

        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        IdleState = new SlimeIdleState1(this, slimeStateMachine1, slimeStats1, "SlimeIdle");
        ChaseState = new SlimeChaseState1(this, slimeStateMachine1, slimeStats1, "SlimeChase");
        DeadState = new SlimeDeadState1(this, slimeStateMachine1, slimeStats1, "SlimeDead");
    }

    private void Start()
    {
        

        SR = GetComponent<SpriteRenderer>();

        matWhite = Resources.Load<Material>("Material/WhiteFlash");
        matDefault = SR.material;

        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        slimeStateMachine1.InitializeState(IdleState);
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }

        FlipDirection();

        slimeStateMachine1.CurrentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        slimeStateMachine1.CurrentState.PhysicsUpdate();
    }

    public void CheckDead()
    {
        if (slimeStats1.health <= 0)
        {
            myAudio.PlayOneShot(Slime1DieSFX);

            // Hide the self light if the enemy is dead.
            selfLight.SetActive(false);

            // If the Vampiric Essence buff is activated then player can have a chance to restore health.
            if (buffContent.onVampiricEssence)
            {
                buffContent.DetectDead();
            }
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
            isHurt = true;

            slimeStats1.health = 0;
            aiPath.isStopped = true;
            aiPath.maxSpeed = 0;

            spriteRenderer.sortingOrder = 9;

            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

            slimeStateMachine1.ChangeState(DeadState);
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
            myAudio.PlayOneShot(Slime1HurtSFX);

            slimeStats1.health -= damageValue;

            SR.material = matWhite;
            Invoke("ResetMaterial", 0.1f);
            StartCoroutine("WaitForHurt");
        }
    }

    
    public void FlipDirection()
    {
        if (aiPath.velocity.x >= 0.01 && !facingRight || aiPath.velocity.x <= -0.01 && facingRight)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduce player's health here
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}
