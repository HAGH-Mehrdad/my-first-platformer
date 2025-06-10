using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        HandleAnimation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveSpeed * facingDirection, rb.linearVelocityY);
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }
}
