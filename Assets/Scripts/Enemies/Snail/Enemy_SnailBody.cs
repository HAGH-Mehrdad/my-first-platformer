using UnityEngine;

public class Enemy_SnailBody : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float zRotation;


    public void SetupBody(float yVelocity, float zRotaion, int facingDir)// Method to set up the snail body with initial y velocity and z rotation
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, yVelocity);

        this.zRotation = zRotaion;

        if(facingDir == 1)
            sr.flipX = true;

    }


    private void Update()// similar to HandleDeathRotation() in Enemy.cs
    {
        transform.Rotate(0, 0, zRotation * Time.deltaTime);
    }
}
