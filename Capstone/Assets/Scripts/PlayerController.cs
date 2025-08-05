using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 20f;
    [Space(5)]

    [Header("Vertical Movement Settings:")]
    [SerializeField] private float jumpForce = 45f;
    private float jumpBufferCounter = 0f;
    [SerializeField] private int jumpBufferFrames;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;
    private float gravity;
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
    [SerializeField] private Transform SideAttackTransform;
    [SerializeField] private Transform UpAttackTransform;
    [SerializeField] private Transform DownAttackTransform;
    [SerializeField] private Vector2 SideAttackArea;
    [SerializeField] private Vector2 UpAttackArea;
    [SerializeField] private Vector2 DownAttackArea;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttack;
    [SerializeField] private float damage;
    [Space(5)]

    [Header("Attack Settings:")]
    [SerializeField] private int recoilXSteps = 5;
    [SerializeField] private int recoilYSteps = 5;
    [SerializeField] private float recoilXSpeed = 100f;
    [SerializeField] private float recoilYSpeed = 100f;
    private int stepsXRecoiled;
    private int stepsYRecoiled;
    [Space(5)]

    [Header("Health Settings:")]
    public int health;
    public int maxHealth;
    [SerializeField] float hitFlashSpeed;
    [Space(5)]

    [HideInInspector] public PlayerStateList pState;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float xAxis;
    private float yAxis;
    private bool attack = false;
    private bool restoreTime;
    private float restoreTimeSpeed;
    private bool canFlash = true;

    public static PlayerController Instance;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);
            }
        }
    }

    private void Awake()
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
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        Health = maxHealth;
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
        if (pState.cutscene) return;

        GetInputs();
        UpdateJumpVariables();
        RestoreTimeScale();

        if (pState.dashing) return;

        if (pState.alive)
        {
            FlashWhileInvincible();
            Flip();
            Move();
            Jump();
            StartDash();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (pState.dashing || pState.cutscene) return;

        Recoil();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");

        // Simulate taking damage for testing purposes
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(2);
        }
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            pState.lookingRight = true;
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

    public IEnumerator WalkIntoNewScene(Vector2 exitDir, float delay)
    {
        try
        {


            if (exitDir.y > 0)
            {
                rb.linearVelocity = jumpForce * exitDir;
            }

            if (exitDir.x != 0)
            {
                xAxis = exitDir.x > 0 ? 1 : -1;

                Move();
            }

            Flip();
            yield return new WaitForSeconds(delay);
        }
        finally
        {

            pState.cutscene = false;
        }
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
                int recoilDirection = pState.lookingRight ? 1 : -1;
                pState.recoilingX = Hit(SideAttackTransform, SideAttackArea, Vector2.right * recoilDirection, recoilXSpeed);
            }
            else if (yAxis > 0)
            {
                pState.recoilingY = Hit(UpAttackTransform, UpAttackArea, Vector2.up, recoilYSpeed);
            }
            else if (yAxis < 0 && !Grounded())
            {
                pState.recoilingY = Hit(DownAttackTransform, DownAttackArea, Vector2.down, recoilYSpeed);
            }
        }
    }

    bool Hit(Transform attackTransform, Vector2 attackArea, Vector2 recoilDir, float recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0f, attackableLayer);
        List<EnemyAI> hitEnemies = new List<EnemyAI>();

        foreach (Collider2D collider in objectsToHit)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();

            if (enemy && !hitEnemies.Contains(enemy))
            {
                if (collider.GetComponent<EnemyAI>() != null)
                {
                    collider.GetComponent<EnemyAI>().EnemyHit(damage, recoilDir, recoilStrength);
                    hitEnemies.Add(enemy);
                }
            }
        }

        return objectsToHit.Length > 0;
    }

    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }

        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    public void TakeDamage(float damage)
    {
        if (pState.alive)
        {
            Health -= Mathf.RoundToInt(damage);

            if (Health <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(StopTakingDamage());
            }
        }
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }

    IEnumerator Flash()
    {
        sr.enabled = !sr.enabled;
        canFlash = false;
        yield return new WaitForSeconds(0.1f);
        canFlash = true;
    }

    void FlashWhileInvincible()
    {
        if (pState.invincible)
        {
            if (Time.timeScale > 0.2f && canFlash)
            {
                StartCoroutine(Flash());
            }
        }
        else
        {
            sr.enabled = true;
        }
    }

    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.unscaledDeltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float newTimeScale, float restoreSpeed, float delay)
    {
        restoreTimeSpeed = restoreSpeed;
        Time.timeScale = newTimeScale;

        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float delay)
    {
        restoreTime = true;
        yield return new WaitForSecondsRealtime(delay);
    }

    IEnumerator Death()
    {
        pState.alive = false;
        Time.timeScale = 1f;
        anim.SetTrigger("Death");

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game Over");
    }

    public bool Grounded()
    {
        return (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround));
    }

    void Jump()
    {
        if (jumpBufferCounter > 0 && !pState.jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            pState.jumping = true;
        }
        else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
        {
            pState.jumping = true;
            airJumpCounter++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 3)
        {
            pState.jumping = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            airJumpCounter = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime * 20;
        }
    }
}
