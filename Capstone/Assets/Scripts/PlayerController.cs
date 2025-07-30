using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 20f;
    [Space(5)]

    [Header("Vertical Movement Settings:")]
    [SerializeField] private float jumpForce = 45f;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0f;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;
    [Space(5)]

    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings:")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [SerializeField] private GameObject dashEffect;
    private bool canDash = true;
    private bool dashed;
    [Space(5)]

    [Header("Attack Settings:")]
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private float timeSinceAttack;
    [SerializeField] private Transform SideAttackTransform;
    [SerializeField] private Transform UpAttackTransform;
    [SerializeField] private Transform DownAttackTransform;
    [SerializeField] private Vector2 SideAttackArea;
    [SerializeField] private Vector2 UpAttackArea;
    [SerializeField] private Vector2 DownAttackArea;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damage;
    private bool attack;
    [Space(5)]

    private PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis;
    private float yAxis;
    private float gravity;
    private Animator anim;

    public static PlayerController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing)
        {
            return; // Skip the rest of the update if dashing
        }
        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(xAxis * walkSpeed, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack()
    {
        timeSinceAttack += Time.deltaTime;

        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0f;
            anim.SetTrigger("Attacking");

            if (yAxis <= 0 && Grounded())
            {
                Hit(SideAttackTransform, SideAttackArea);
            }
            else if (yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(DownAttackTransform, DownAttackArea);
            }
        }
    }

    void Hit(Transform attackTransform, Vector2 attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0f, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            foreach (Collider2D collider in objectsToHit)
            {
                //if (collider.GetComponent<Enemy>() != null)
                //{
                //    collider.GetComponent<Enemy>().EnemyHit(damage);
                //}
            }
        }
    }

    public bool Grounded()
    {
        return (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                pState.jumping = true;
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            }
        }

        anim.SetBool("Jumping", !Grounded());
    }

    private void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter--;
            }
        }
    }
}
