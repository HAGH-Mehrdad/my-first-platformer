using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float idleDuration;//Wait duration before moving again
    [SerializeField] protected float idleTimer;


    [Header ("Basic Collision")]
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;// we cannot check ground under the mushroom, we need to check in front of him


    protected int facingDir = -1; // 1 for right, -1 for left
    protected bool facinRight = false; // true if facing right, false if facing left (by default it is faced left because the sprite is)
    protected bool isGrounded; // for when the player can fall on ground and won't keep fliping on the air
    protected bool isGroundForwardDetected = false; // true if the enemy is on the ground / false if it will fall down
    protected bool isWallDetected = false;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;
    }

    protected virtual void HandleFlip(float xValue)
    {
        if ( xValue < 0 && facinRight || xValue > 0 && !facinRight)
            Flip();
    }

    protected virtual void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facinRight = !facinRight;
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down , groundCheckDistance , whatIsGround);//The ground check is cast from enemy's body rather than a child object
        isGroundForwardDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance , whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance , whatIsGround);//The wall check is cast from enemy's face or body rather than a child object
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x , groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));//groundCheck.position or transform.position ? transform because the it is cast from enemy's face or body
    }

}
