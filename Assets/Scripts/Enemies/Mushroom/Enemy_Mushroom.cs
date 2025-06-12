using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        HandleCollision();
        HandleMovement();

        if (!isGroundForwardDetected || isWallDetected)
        {
            if (!isGrounded)
                return;

            Flip();
            idleTimer = idleDuration; // Reset the idle timer when the enemy is not grounded or hits a wall (wants to flip!)
            rb.linearVelocityX = 0; // Stop moving when the enemy is not grounded or hits a wall
        }
    }

    private void HandleMovement()
    {
        if(idleTimer > 0)
            return; // If the idle timer is not finished, do not move

        if(isGroundForwardDetected)
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);//this condition removes jittering effect when the player is falling from a platform
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }
}
