using UnityEngine;

public class Enemy_Chicken : Enemy
{

    [Header("Chicken Detail")]
    [SerializeField] private float aggroDuraion;


    private float aggroTimer;
    private bool canFlip = true;


    protected override void Update()
    {
        base.Update();

        // TODO: Somehow we should stop this counter due to memnory save (it keeps decreasing )
        aggroTimer -= Time.deltaTime; // TODO: is it better to add this on parent class? 

        HandleAnimation();

        if (isDead)
            return;

        if (isPlayerDetected)//chase the player if it is on the radar
        {
            canMove = true;
            aggroTimer = aggroDuraion;
        }

        if (aggroTimer < 0)// Do not follow Player if the aggression time is over
        {
            canMove = false;
            aggroTimer = 0; // prevent from decreasing all the time
        }

        HandleCollision();
        HandleMovement();
        HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        //we can check this condition on update method but due to Encapsulation, readability and flexibility we check isGrounded here.
        if (!isGrounded)
            return; // If the enemy is not grounded, do not flip [When the designer didn't place the enemy on the ground before play]

        if (!isGroundAheadDetected || isWallDetected)
        {
            Flip();
            canMove = false;//When it detects the wall or ledge it stops [when the it doesn't detect the player]
            rb.linearVelocityX = 0; // Stop moving when the enemy is not grounded or hits a wall
        }
    }

    private void HandleMovement()
    {
        if (canMove == false)
            return; // If the idle [animation] timer is not finished, do not move

        HandleFlip(player.transform.position.x);

        if (isGroundAheadDetected)//this condition removes jittering effect when the player is falling from a platform or is simply not on the ground
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facinRight || xValue > transform.position.x && !facinRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), 0.3f);
            }
        }
            
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }
}
