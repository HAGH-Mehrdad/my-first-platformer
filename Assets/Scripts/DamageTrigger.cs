using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private bool canDamage = true; // Flag to control damage triggering


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canDamage == false)
            return; // If the damage trigger is not enabled, do nothing (like what we did in the Trap_FireDelay script)


        Player player = collision.gameObject.GetComponent<Player>();

        //Or we can do this: player?.knockback();
        if (player != null)
            player.Knockback(transform.position.x);
    }


    public void CanDamagePlayer(bool canDamage)
    {
        this.canDamage = canDamage;
    }
}
