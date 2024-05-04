using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public static PlayerController Instance;

    public float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private ParticleSystem dust;

    public int maxDashes = 3;
    public float dashRestoreTime = 10f;
    public int dashCount;
    public bool isDashing = false;
    public static event Action OnDashCountChanged;
    public float startingMoveSpeed;

    public AudioSource myAudio;   
    public AudioClip DashSFX;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Collider2D myCollider;
    private bool facingLeft = false;

    // Buff
    private BuffContent buffContent;


    // Testing
    //public float startTime;
    //public float timeBtwFrame;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        dashCount = maxDashes;
        myAudio = GetComponent<AudioSource>();

        buffContent = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<BuffContent>();

        //startTime = dashRestoreTime;
        //timeBtwFrame = startTime;
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        StartCoroutine(RestoreDashesRoutine());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();       

        //startTime = dashRestoreTime;

        //if (timeBtwFrame <= 0)
        //{
        //timeBtwFrame = startTime;
        //}

        //else
        //{
        //timeBtwFrame -= Time.deltaTime;
        //Debug.Log(timeBtwFrame);
        //}
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {        
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
       
    }

    private void Move()
    {      
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));       
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (movement != Vector2.zero && dashCount > 0 && !isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            if (myTrailRenderer != null)
            {
                myTrailRenderer.emitting = true;
            }
            CreateDust();

            if (buffContent.onEtherealDash)
            {
                int randomNumber = UnityEngine.Random.Range(0, 101);

                if (randomNumber <= buffContent.dashChance)
                {
                    // Play cost dash sound.
                    FindObjectOfType<AudioManager>().Play("Cost Dash");

                    GameObject costEffect = Instantiate(buffContent.costVFX, transform.position, transform.rotation, transform);

                    Destroy(costEffect, 0.6f);
                }

                else
                {
                    // Decrease the dash count
                    dashCount--;
                }
            }

            else
            {
                // Decrease the dash count
                dashCount--;
            }

            myAudio.PlayOneShot(DashSFX);

            // Invoke the event to notify subscribers (such as the DashUI script) that the dash count has changed
            OnDashCountChanged?.Invoke();

            StartCoroutine(Invulnerability());
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private IEnumerator RestoreDashesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashRestoreTime);
            if (dashCount < maxDashes)
            {
                dashCount++;

                // Invoke the event to notify subscribers (such as the DashUI script) that the dash count has changed
                OnDashCountChanged?.Invoke();
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            mySpriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            mySpriteRender.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    public void CreateDust()
    {
        dust.Play();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}

