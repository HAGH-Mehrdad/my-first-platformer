using UnityEngine;
using UnityEngine.EventSystems;

public class UI_JumpButton : MonoBehaviour, IPointerDownHandler
{
    private Player player; // Reference to the Player script

    public void OnPointerDown(PointerEventData eventData)
    {
        player.JumpButton();
    }

    public void UpdatePlayerRef(Player newPlayer) => player = newPlayer;
}
