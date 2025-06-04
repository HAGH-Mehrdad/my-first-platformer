using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //We add this script to the animator game object of the player to add the event => when the animation is finished get controll of the player


    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void FinishRespawn() => player.RespawnFinished(true); // this method will be called at the end of the animation
}
