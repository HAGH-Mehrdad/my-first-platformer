using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{

    private Animator anim;

    [SerializeField] private float pushPower = 25f;
    [SerializeField] private float duration = 0.5f;


    private void Awake()
    {
        // This anim will be used to trigger the trampoline animation a couple of times. so we should store it 
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            // If we use vector2 instead of transform, the direction of force never change.
            player.Push(transform.up * pushPower , duration);
            anim.SetTrigger("activates");
        }
    }
}
