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

    public int maxDashes = 3;
    public float dashRestoreTime = 10f;
    public int dashCount;
    public bool isDashing = false;
    public static event Action OnDashCountChanged;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    public float startingMoveSpeed;
    private bool facingLeft = false;


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
            myTrailRenderer.emitting = true;

            // Decrease the dash count
            dashCount--;

            // Invoke the event to notify subscribers (such as the DashUI script) that the dash count has changed
            OnDashCountChanged?.Invoke();

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

    
}

