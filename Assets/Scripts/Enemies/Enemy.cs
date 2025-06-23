using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;


    [SerializeField] protected Transform player;
    [SerializeField] private GameObject damageTrigger;

    [Header("General Info")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float idleDuration = 1.5f;//Wait duration before moving again
    protected float idleTimer; // Timer to track the idle duration
    protected bool canMove;


    [Header("Death Details")]
    [SerializeField] private float deathJumpImpact = 5;
    [SerializeField] private float deathRotationSpeed = 150;
    private int deathRotationDirection = 1; //For random rotation when dying
    protected bool isDead;


    [Header ("Basic Collision")]
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected Transform groundCheck;// we cannot check ground under the mushroom, we need to check in front of him


    protected int facingDir = -1; // 1 for right, -1 for left
    protected bool facinRight = false; // true if facing right, false if facing left (by default it is faced left because the sprite is)
    protected bool isGrounded; // for when the player can fall on ground and won't keep fliping on the air
    protected bool isGroundAheadDetected = false; // true if the enemy is on the ground / false if it will fall down
    protected bool isWallDetected = false;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        InvokeRepeating(nameof(UpdatePlayerRef) , 0 , 1);//Reference to player's position [for chicken attack at this point] (Starts immediately and repeats every second)
    }

    private void UpdatePlayerRef()
    {
        if (player == null)
        {
            player = GameManager.instance.player.transform;
        }
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;

        if (isDead)
            HandleDeathRotation();
    }

    public virtual void Die()
    {
        col.enabled = false;
        damageTrigger.SetActive(false);
        anim.SetTrigger("hit");

        rb.linearVelocity = new Vector2(rb.linearVelocityX, deathJumpImpact);

        isDead = true;

        if (Random.Range(0, 100) < 50)
            deathRotationDirection = -deathRotationDirection;//giving random death impact jump

    }

    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, deathRotationSpeed * deathRotationDirection * Time.deltaTime);
    }

    protected virtual void HandleFlip(float xValue)
    {
        if ( xValue < transform.position.x && facinRight || xValue > transform.position.x && !facinRight)
            Flip();
    }

    protected virtual void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facinRight = !facinRight;
    }

    protected virtual void HandleCollision() // TODO: What if I put this method on update and don't override in other enemy scripts
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down , groundCheckDistance , whatIsGround);//The ground check is cast from enemy's body rather than a child object [to prevent enemy from fliping if it is not grounded before playing]
        isGroundAheadDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance , whatIsGround); // The ground check object that the enemy can decide if there is ground ahead or not, so it can flip if there is no ground ahead
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance , whatIsGround);//The wall check is cast from enemy's face or body rather than a child object
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x , groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));//groundCheck.position or transform.position ? transform because the it is cast from enemy's face or body
    }

}
