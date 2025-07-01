using UnityEngine;

public class Enemy_SnailBody : MonoBehaviour
{
    private Rigidbody2D rb;
    private float zRotation;


    public void SetupBody(float yVelocity, float zRotaion)// Method to set up the snail body with initial y velocity and z rotation
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, yVelocity);

        this.zRotation = zRotaion;
    }


    private void Update()// similar to HandleDeathRotation() in Enemy.cs
    {
        transform.Rotate(0, 0, zRotation * Time.deltaTime);
    }
}
