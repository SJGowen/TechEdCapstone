using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 20f;

    [Header("Ground Check Settings")]
    [SerializeField] private float jumpForce = 45f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    private float xAxis;
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
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInputs();
        Move();
        Jump();
        Flip();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
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
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && IsGrounded());
    }

    public bool IsGrounded()
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
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        anim.SetBool("Jumping", !IsGrounded());
    }
}
