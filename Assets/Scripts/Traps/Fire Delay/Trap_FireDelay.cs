using UnityEngine;
using System.Collections;

public class Trap_FireDelay : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;
    private DamageTrigger damageTrigger; // Reference to the DamageTrigger component for fire interaction

    [SerializeField] private float activatingDelay; // Delay before the fire activates
    [SerializeField] private float activeDuration; // Duration for which the fire is active

    private bool isActive = false;
    private bool canPushed = true;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        damageTrigger = GetComponent<DamageTrigger>();
    }

    private void Start()
    {
        damageTrigger.CanDamagePlayer(false); // Initially disable the damage trigger
        capsuleCollider.enabled = false; // Initially disable the capsule collider
        boxCollider.enabled = true; // Enable the box collider to allow triggering
        canPushed = true; // Allow the button to be pushed initially
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null && canPushed)
        {
            StartCoroutine(FireDelayRoutine());
        }
    }

    private IEnumerator FireDelayRoutine()
    {
        if( canPushed == false)
            yield break; // If the button is already pushed, exit the coroutine

        canPushed = false; // Prevent further pushes until the fire is activated

        boxCollider.enabled = false; // Disable the box collider to prevent further triggers while the fire is activating
        anim.SetTrigger("pushes");



        yield return new WaitForSeconds(activatingDelay);

        isActive = true;
        damageTrigger.CanDamagePlayer(true); // Enable the damage trigger for fire interaction
        capsuleCollider.enabled = isActive; // Enable the capsule collider to allow interaction with the fire
        anim.SetBool("activates", isActive);



        yield return new WaitForSeconds(activeDuration);

        isActive = false;
        anim.SetBool("activates", isActive); // Deactivate the fire animation


        boxCollider.enabled = true; // Re-enable the box collider for future triggers
        capsuleCollider.enabled = false; // Disable the capsule collider after the fire is inactive

        canPushed = true; // Allow the button to be pushed again
        damageTrigger.CanDamagePlayer(false); // Disable the damage trigger after the fire is inactive
    }
}
