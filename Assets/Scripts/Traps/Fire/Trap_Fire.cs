using System.Collections;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{

    [SerializeField] private float offDuration;//For how much should it be diactive
    [SerializeField] private Trap_FireButton fireButton;//Reference to the fire button script, so we can call the coroutine from it

    private Animator anim;
    private BoxCollider2D fireCollider;
    private bool isActive = false;




    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<BoxCollider2D>();
    }


    private void Start()
    {
        if(fireButton == null)
        {
            Debug.LogError("Fire Button is not assigned in the Trap_Fire script on " + gameObject.name);
            return;
        }

        SetFire(true);
    }

    public void SwithcOffFire()// This method is called from the fire button script when the player enters the button collider
    {
        if (isActive == false)
            return; // If the fire is already off, we don't need to do anything

        StartCoroutine(FireCoroutine());
    }


    private IEnumerator FireCoroutine()// we need to call this co-routine out side of the script (we want to call it form when the player enter the button collider)
    {
        SetFire(false);

        yield return new WaitForSeconds(offDuration);

        SetFire(true);
    }
    
    private void SetFire(bool active)
    {
        anim.SetBool("activates", active);
        fireCollider.enabled = active;
        isActive = active;
    }
}
