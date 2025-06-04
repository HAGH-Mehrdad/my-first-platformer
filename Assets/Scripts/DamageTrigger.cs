using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        //Or we can do this: player?.knockback();
        if (player != null)
            player.Knockback(transform.position.x);
    }
}
