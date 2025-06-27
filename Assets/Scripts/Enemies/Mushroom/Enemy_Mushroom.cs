using UnityEngine;

public class Enemy_Mushroom : Enemy
{
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

    private void HandleMovement()
    {
        if(idleTimer > 0)
            return; // If the idle [animation] timer is not finished, do not move

        if(isGroundAheadDetected)//this condition removes jittering effect when the player is falling from a platform or is simply not on the ground
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }
}
