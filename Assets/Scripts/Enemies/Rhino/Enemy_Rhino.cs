using System;
using UnityEngine;

public class Enemy_Rhino : Enemy
{

    [Header("Rhino details")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speedUpRate = 1.0f;//rate of speed change
    [SerializeField] private Vector2 impactForce = new Vector2(4,2);// I think it is better to assign default values. Because at the editor I forgot to do it at first
    [SerializeField] private float detectionRange;
    private float defaultSpeed;
    
    private bool playerDetected;


    protected override void Start()
    {
        base.Start();

        defaultSpeed = moveSpeed;//Saving the moving speed
    }


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

        moveSpeed = moveSpeed + (speedUpRate * Time.deltaTime);

        if (moveSpeed >= maxSpeed)
            moveSpeed = maxSpeed;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);

        if (isWallDetected)
            WallHit();


        if (!isGroundAheadDetected)
            TurnAround();
    }

    private void TurnAround()
    {
        moveSpeed = defaultSpeed;
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        Flip();
    }

    private void WallHit()
    {
        canMove = false;// to prevent intervention of velocity in charge method
        moveSpeed = defaultSpeed;//reseting the speed
        anim.SetBool("wallHit" , true);
        rb.linearVelocity = new Vector2(impactForce.x * -facingDir, impactForce.y); // A problem I had here was that I declared Vector2 filed and I passed impactforce itself & linearVelocity.y
    }

    private void ChargeIsOver()// this method will be called when the hit animation is over
    {
        anim.SetBool("wallHit" , false);
        Invoke(nameof(Flip), 1); // after he hit the wall, flips 
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, whatIsPlayer);

        if(playerDetected && isGrounded)// to prevent rhino to flip if it detects the player just after it hits a wall (isGrounded)
            canMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
