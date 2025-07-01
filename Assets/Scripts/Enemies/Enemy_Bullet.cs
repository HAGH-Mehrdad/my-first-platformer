using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{


    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";

    private Rigidbody2D rb;
    private SpriteRenderer sr;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite()
    {
        sr.flipX = !sr.flipX; // This method will be used from the enemy to change the projectile sprite direction.
    }


    public void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity; // This method will be used from the enemy to set the projectile velocity.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            collision.GetComponent<Player>().Knockback(transform.position.x);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(gameObject);
        }
    }
}
