using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>(); //we are not using it in the Awake method and it is not expensive to get the component here

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            anim.SetTrigger("activates");
        }
    }
}
