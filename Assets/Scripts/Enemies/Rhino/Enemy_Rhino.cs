using System;
using UnityEngine;

public class Enemy_Rhino : Enemy
{

    [Header("Rhino details")]
    [SerializeField] private Vector2 impactForce;
    [SerializeField] private float detectionRange;
    
    private bool playerDetected;


    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocityX);

        HandleCollision();
        HandleCharge();
    }


    private void HandleCharge()//stands still untill it detects the player
    {
        if (canMove == false)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);

        if (isWallDetected)
        {
            WallHit();
        }


        if (!isGroundAheadDetected)
        {
            canMove = false;
            rb.linearVelocity = Vector2.zero;
            Flip();
        }
    }

    private void WallHit()
    {
        anim.SetBool("wallHit" , true);
        rb.linearVelocity = new Vector2(impactForce.x * -facingDir, impactForce.y); // A problem I had here was that I declared Vector2 filed and I passed impactforce itself & linearVelocity.y
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, whatIsPlayer);

        if(playerDetected)
            canMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
