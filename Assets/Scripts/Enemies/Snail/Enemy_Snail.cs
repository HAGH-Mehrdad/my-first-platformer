using UnityEngine;

public class Enemy_Snail : Enemy
{
    [Header("Snail Details")]
    [SerializeField] private Enemy_SnailBody bodyPrefab; // Reference to the snail body prefab instead of GameObject to gain easy access to is.
    [SerializeField] private float maxSpeed = 15f;

    private bool hasBody = true;


    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

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
            idleTimer = idleDuration; // Reset the idle timer when the enemy is not grounded or hits a wall (wants to flip!)
            rb.linearVelocityX = 0; // Stop moving when the enemy is not grounded or hits a wall
        }
    }

    public override void Die()
    {
        if (hasBody)
        {
            canMove = false;
            hasBody = false;

            anim.SetTrigger("hit");

            idleDuration = 0; // prevent the shell to wait after hitting a wall.(it will bounce between walls none stop)
            rb.linearVelocity = Vector2.zero; // Stop moving when the snail is hit
        }
        else if (canMove == false && hasBody == false)
        {
            anim.SetTrigger("hit");
            canMove = true;
            moveSpeed = maxSpeed;
        }
        else
        {
            base.Die();
        }

    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return; // If the idle [animation] timer is not finished, do not move

        if(canMove == false)
            return; // If the snail is dead, do not move. THE SHELL STILL THERE.

        if (isGroundAheadDetected)//this condition removes jittering effect when the player is falling from a platform or is simply not on the ground
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }


    private void CreateBody()
    {
        Enemy_SnailBody newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity);

        newBody.SetupBody(deathJumpImpact, deathRotationSpeed * deathRotationDirection , facingDir);

        Destroy(newBody, 15); //destrying the snail body after 15 seconds
    }

    protected override void Flip()
    {
        base.Flip();

        if (hasBody == false)
            anim.SetTrigger("wallHit");
    }
}
