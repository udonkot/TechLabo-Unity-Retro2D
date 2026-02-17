using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 70f;
    [SerializeField] private float deceleration = 80f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Damage")]
    [SerializeField] private float invincibleDuration = 1f;
    [SerializeField] private float knockbackX = 5f;
    [SerializeField] private float knockbackY = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float invincibleTimer;
    private bool jumpPressed;
    private bool jumpHeld;

    public bool IsGrounded { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (groundLayer.value == 0)
        {
            int mask = LayerMask.GetMask("Ground");
            groundLayer = mask == 0 ? Physics2D.DefaultRaycastLayers : mask;
        }
    }

    private void Update()
    {
        if (IsDead)
        {
            return;
        }

        horizontalInput = ReadHorizontalInput();

        if (ReadJumpPressed())
        {
            jumpPressed = true;
            jumpBufferTimer = jumpBufferTime;
        }

        jumpHeld = ReadJumpHeld();

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }

        if (spriteRenderer != null && Mathf.Abs(horizontalInput) > 0.01f)
        {
            spriteRenderer.flipX = horizontalInput < 0f;
        }
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        IsGrounded = groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (IsGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        HandleHorizontalMovement();
        HandleJump();

        if (!jumpHeld && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        jumpPressed = false;
    }

    private void HandleHorizontalMovement()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float rate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movement = rate * speedDiff;

        rb.AddForce(Vector2.right * movement);
    }

    private void HandleJump()
    {
        if (!jumpPressed && jumpBufferTimer <= 0f)
        {
            return;
        }

        if (coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }
    }

    public void BounceFromStomp(float bounceVelocity)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceVelocity);
    }

    public bool TryTakeHit(Vector2 sourcePosition)
    {
        if (invincibleTimer > 0f || IsDead)
        {
            return false;
        }

        invincibleTimer = invincibleDuration;
        float dir = transform.position.x < sourcePosition.x ? -1f : 1f;
        rb.linearVelocity = new Vector2(dir * knockbackX, knockbackY);

        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.OnPlayerHit(this);
        }

        return true;
    }

    public void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        rb.linearVelocity = new Vector2(0f, jumpForce * 0.8f);

        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.OnPlayerDied(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void ConfigureGroundCheck(Transform check, LayerMask mask)
    {
        groundCheck = check;
        groundLayer = mask;
    }

    private float ReadHorizontalInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            float horizontal = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal -= 1f;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal += 1f;
            }
            return horizontal;
        }

        if (Gamepad.current != null)
        {
            return Mathf.Clamp(Gamepad.current.leftStick.ReadValue().x, -1f, 1f);
        }
#endif

        return Input.GetAxisRaw("Horizontal");
    }

    private bool ReadJumpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            return Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        if (Gamepad.current != null)
        {
            return Gamepad.current.buttonSouth.wasPressedThisFrame;
        }
#endif

        return Input.GetButtonDown("Jump");
    }

    private bool ReadJumpHeld()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            return Keyboard.current.spaceKey.isPressed;
        }

        if (Gamepad.current != null)
        {
            return Gamepad.current.buttonSouth.isPressed;
        }
#endif

        return Input.GetButton("Jump");
    }
}
