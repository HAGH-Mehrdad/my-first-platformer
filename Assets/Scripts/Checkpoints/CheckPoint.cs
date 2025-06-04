using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // We let it be seen in inpector if we don't want to set all the checkpoints to canBeReactiavted
    [SerializeField] private bool canBeReactivated; // If true, the checkpoint can be activated again and again (we can reactivate it in the inspector)

    private Animator anim => GetComponent<Animator>(); //we are not using it in the Awake method and it is not expensive to get the component here
    private bool active;



    private void Start()
    {
        canBeReactivated = GameManager.instance.canReactivated; // Get the value from GameManager
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && canBeReactivated == false)
            return;


        Player player = collision.GetComponent<Player>();
        //anim.SetBool("active" , active ); It is better to activate in another method

        if ( player != null)
        {
            ActivateCheckPoint();
        }
    }


    private void ActivateCheckPoint()
    {
        active = true;
        anim.SetTrigger("activates"); // instead of using SetBool we use SetTrigger to activate the animation once
        GameManager.instance.UpdateRespawnPosition(transform);
    }
}
