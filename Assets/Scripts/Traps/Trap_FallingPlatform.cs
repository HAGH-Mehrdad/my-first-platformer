using System.Collections;
using UnityEngine;

public class Trap_FallingPlatform : MonoBehaviour
{
    //We don't use inheritance here, but we can use it if we want to (There is no reason to use for one method and one variable)

    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;


    [SerializeField] private float speed = 1f;
    [SerializeField] private float travelDistance;


    [Header("Platform Fall Detail")]
    [SerializeField] private float impactSpeed = 3; // For how much does the platform is moved down by the player
    [SerializeField] private float impactDuration = .1f; // For how long the platform is impacted by the player
    [SerializeField] private float powerImpact = 10f;
    private float impactTimer;
    private bool impactHapped; // To prevent the player pushing the platform down all the time (OR it shows the player that it will fall soon)

    [Space]
    [SerializeField] private float fallDelay = 1f;

    private  Vector3[] waypoints;
    private int wayPointIndex;
    private bool canMove =  false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private IEnumerator Start()
    {
        StartWayPoint();// For the platform moving up and down, feeling of floating in the air


        float startDelay = Random.Range(0, 0.6f);//actually form 0 to 0.59
        
        yield return new WaitForSeconds(startDelay);

        canMove = true;
    }

    private void Update()
    {
        HandleImpact();
        HandleMovement();
    }

    private void StartWayPoint()
    {
        waypoints = new Vector3[2];
        
        float yOffset = travelDistance / 2f;

        waypoints[0] = transform.position + new Vector3(0 , yOffset , 0);
        waypoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }

    private void HandleMovement()
    {
        if (!canMove)// one logic here is when this becomes true, the deactivated plaftform won't be handled by the following commands.
            return;

        //I had a problem here! I didn't assing this to the postion of the transform, so it didn't move
        transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex], speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, waypoints[wayPointIndex]) < 0.01f)
        {
            wayPointIndex++;

            if(wayPointIndex >= waypoints.Length)
                wayPointIndex = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactHapped)
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            Invoke(nameof(SwithOffpltform), fallDelay);

            impactTimer = impactDuration;

            impactHapped = true;
            //Debug.Log("Player is on falling platform");
        }
    }

    //We move the platform down by a timer while the player jumps on the platform
    private void HandleImpact()
    {
        if (impactTimer < 0)
            return;

        impactTimer -= Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, (Vector3.down * powerImpact), impactSpeed * Time.deltaTime);
    }


    //this method should be called after a delay. so it won't fall when the player stands on it immediately
    private void SwithOffpltform()
    {
        canMove = false; //To prevent jitter effect of the deactivated platform

        anim.SetTrigger("deactive");

        //rb.isKinematic = false;   THIS IS OBSOLETE IN UNITY 6
        rb.bodyType = RigidbodyType2D.Dynamic; // The new version

        rb.gravityScale = 2.5f;
        
        //rb.drag = .3f; THIS ONE IS OBSOLETE TOO
        rb.linearDamping = .5f; //The new version


        boxCollider.enabled = false;
        capsuleCollider.enabled = false;
    }
}
