using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float checkDistance = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float stompBounceVelocity = 9f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool movingRight = false;
    private bool dead;

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

    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (ShouldTurn(direction))
        {
            TurnAround();
        }
    }

    private bool ShouldTurn(float direction)
    {
        if (wallCheck != null)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, checkDistance, groundLayer);
            if (wallHit.collider != null)
            {
                return true;
            }
        }

        if (edgeCheck != null)
        {
            Vector2 edgeOrigin = edgeCheck.position;
            RaycastHit2D groundHit = Physics2D.Raycast(edgeOrigin, Vector2.down, checkDistance * 4f, groundLayer);
            if (groundHit.collider == null)
            {
                return true;
            }
        }

        return false;
    }

    private void TurnAround()
    {
        movingRight = !movingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead)
        {
            return;
        }

        PlayerController2D player = collision.collider.GetComponent<PlayerController2D>();
        if (player == null)
        {
            return;
        }

        ContactPoint2D contact = collision.GetContact(0);
        bool stomped = contact.normal.y < -0.5f || player.transform.position.y > transform.position.y + 0.2f;

        if (stomped)
        {
            Die();
            player.BounceFromStomp(stompBounceVelocity);
        }
        else
        {
            player.TryTakeHit(transform.position);
        }
    }

    private void Die()
    {
        dead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (wallCheck != null)
        {
            float direction = movingRight ? 1f : -1f;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * direction * checkDistance);
        }

        if (edgeCheck != null)
        {
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * checkDistance * 4f);
        }
    }

    public void ConfigureChecks(Transform newWallCheck, Transform newEdgeCheck, LayerMask mask)
    {
        wallCheck = newWallCheck;
        edgeCheck = newEdgeCheck;
        groundLayer = mask;
    }
}
