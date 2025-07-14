using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEditor.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd; // The player has the "Capsule" collider 2D!


    private bool canBeContrelled = false;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private float defaultGravityScale;
    private bool canDoubleJump;


    [Header("Buffer & Coyote Jump")]
    [SerializeField] private float bufferJumpWindow = .25f; // The time that will be stored for jumping input (on ground)
    private float bufferJumpActivated = -1f; //Last time the player hit the Jump Button (on ground)
    [SerializeField] private float coyoteJumpWindow = 1f; // The time that will be stored for jumping input (after leaving the the edge of platform)
    private float coyoteJumpActivated = -1f; //Last time the player hit the Jump Button (before leaving the edge of platform)


    [Header("Wall Interactions")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;


    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1f;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;

    [Header("Player Visuals")]
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private int skinId;
    [SerializeField] private GameObject deathVFX;




    [Header("Collision INFO")]
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float wallCheckDistance = 1f;
    [SerializeField] private LayerMask whatIsGround;
    [Space]
    [SerializeField] private Transform enemyCheck;//the position of the enemy and its other information
    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    private bool isGrounded;
    private bool isAirborne; // Use this flag to indicate if isGrounded or airborne should be checked at the same time
    private bool isWallDetected;


    private float xInput;
    private float yInput;

    private bool facingRight = true;
    private int facingDir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();

    }

    private void Start()
    {
        defaultGravityScale = rb.gravityScale;
        RespawnFinished(false);
        
        ChooseSkin();
    }

    void Update()
    {
        UpdateAirboreStatus();

        if (!canBeContrelled)
        {
            //Because of pushing method from trampoline:
            // These mthodes are to avoid the player to play animations when it is respawning or knocked back
            HandleAnimations();
            HandleCollision();
            return;
        }

        if (isKnocked)
            return;
        HandleEnemyDetection();//Checking player jumping on enemy head in this method instead of HandleCollision method
        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimations();
    }

    public void ChooseSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        anim.runtimeAnimatorController = animators[skinManager.ChosenSkinId]; // Set the default animator controller
    }

    private void HandleEnemyDetection()
    {
        if (rb.linearVelocityY > 0)//only when the player is falling on the enemy
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius , whatIsEnemy);

        foreach (var enemy in colliders)//enemy is the name of each item that is stored in colliders collection
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();

            if (newEnemy != null && rb.linearVelocityY < 0)
            {
                newEnemy.Die();
                Jump();
            }
        }
    }


    // This is for Tampoline push force
    public void Push(Vector2 direction , float duration)
    {
        StartCoroutine(PushCoroutine(direction, duration));
    }

    private IEnumerator PushCoroutine(Vector2 direction, float duration)
    {
        canBeContrelled = false;

        rb.linearVelocity = Vector2.zero; // Resets the velocity

        rb.AddForce(direction, ForceMode2D.Impulse); //Impulse is sudden and is not scaled bt Time.DeltaTime

        yield return new WaitForSeconds(duration);

        canBeContrelled = true;
    }


    //Instead of checking if respawning animation is finished we send the parameter to the method
    public void RespawnFinished(bool finishied)
    {
        //It won't work because it is local and at the first time finished in false and gravity scale will get 0 and when finished is true gravity scale will remain 0
        //float gravityScale = rb.gravityScale;


        if (finishied)
        {
            rb.gravityScale = defaultGravityScale; // rigid body where it gets a global value (see the beginning of this method)
            canBeContrelled = true;// player movement
            cd.enabled = true; // Collider 

        }
        else
        {
            rb.gravityScale = 0; // rigid body
            canBeContrelled = false; // player movement
            cd.enabled = false; // Collider
        }
    }


    public void Knockback(float sourceOfDamage)
    {
        float knockbackDirection = sourceOfDamage > transform.position.x ? -1 : 1; // If the source of damage is positive then knockback direction is negative and vice versa

        if (isKnocked)
            return;


        StartCoroutine(KnockbackRoutine());

        rb.linearVelocity = new Vector2 (knockbackPower.x * knockbackDirection, knockbackPower.y);
    }

    public void Die()
    {
        GameObject newFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;

        anim.SetBool("isKnocked",isKnocked);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        anim.SetBool("isKnocked", isKnocked);
    }

    private void UpdateAirboreStatus()
    {
        if (isGrounded && isAirborne) // if on ground then it isn't airborne
            HandleLanding();

        if (!isGrounded && !isAirborne) // if not on ground then it is airborne
            BecomeAirborne();
    }


    /// <summary>
    /// For the purpose of player is airborne
    /// </summary>
    private void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.linearVelocityY < 0) //This indicates that not every time we jump this method will execute. Only when the player is falling 
            ActivateCoyoteJump();
    }

    /// <summary>
    /// For the purpose of when the player is landed
    /// </summary>
    private void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;
        AttemptBufferJump();
    }

    #region Buffer & Coyote Jump
    private void RequestBufferJump()
    {
        if (isWallDetected) //optional
            return;

        if (isAirborne)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time -1;
            Jump();
        }
    }

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;//Indicates that if this ability is available and using it depends on the timer. condition checks when the player hits the jump key
    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1; // We don't set this to zero to make it more sensible
    #endregion

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
            RequestBufferJump();
        }
    }

    /// <summary>
    /// Instead of calling jump function every time we need, we call JumpButton
    /// <remarks>
    /// Handling these jumps: 
    /// - Jump
    /// - Double Jump
    /// - Wall Jump
    /// </remarks>
    /// </summary>
    private void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow; // checking the timer from the player's hitting jump button with the time availabe for coyote jump


        if (isGrounded || coyoteJumpAvailable) //Simple jump OR coyote jump
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

        CancelCoyoteJump(); // Resets the timer
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        canDoubleJump = true; // added here to manage jump even after coyote jump
    }

    private void DoubleJump()
    {
        isWallJumping = false;
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
    }

    private void WallJump()
    {
        canDoubleJump = true;
        //isWallJumping = true; // will be used in HandleMovement to ignore zero velcity on x causing by the player's movement (only y is changing)

        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y); // We won't just set isWallJumping to be false. We need coroutine, to ignore zero velcity on x causing by the player's movement

        Flip();


        StopCoroutine(WallJumpRoutine()); //Maybe in the future I need to use StopAllCoroutines(); instead
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        // TODO: canFlip = false;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
        // TODO: canFlip = true;
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocityY < 0;
        float yModifier = yInput < 0 ? 1 : .05f; // Pressing key down makes the player slide faster

        if (!canWallSlide)
            return;


        rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * yModifier);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        //anim.SetBool("isRunning", rb.linearVelocityX != 0); // we could make a boolean and assign to rb.velocity but for better coding we omitt that 

        //Instead of using old way of propgramming animations I use blend tree

        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void HandleMovement()
    {
        if(isWallDetected)
            return;

        if (isWallJumping)//this condition means that when doing wall jump ignore velocity on x becuase it is zero and affect wall jump force to be difused
            return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
    }

    private void HandleFlip()
    {
        //if the player is running to left and facing to right OR if it is running to right and facing to left we need to flip the character.
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));//For ground check
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y)); //For wall Check
    }
}
