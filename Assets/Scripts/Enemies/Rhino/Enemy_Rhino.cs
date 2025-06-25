using UnityEngine;

public class Enemy_Rhino : Enemy
{

    [Header("Rhino details")]
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
