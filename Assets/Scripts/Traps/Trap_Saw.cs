using System.Collections;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{

    private Animator anim;
    private SpriteRenderer sr;


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float cooldown = 1f;

    public int wayPointIndex = 1;
    private bool canMove = true;
    private int moveDirection = 1;

    private Vector3[] wayPointPosition;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateWayPointsInfo();

        transform.position = wayPointPosition[0];
    }

    private void UpdateWayPointsInfo()
    {
        wayPointPosition = new Vector3[wayPoints.Length];// No matter how many waypoints we have, we will create an array of that size.

        // Fill the wayPointPosition array with the positions of the wayPoints so we can use them to make wayPoints the child of Saw Trap
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPointPosition[i] = wayPoints[i].position;
        }
    }

    private void Update()
    {
        anim.SetBool("active", canMove);

        if (!canMove)
            return;

        //From the current position move to the position of the next index
        //wayPoint is Transform so we should access its positon 
        transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayPointIndex], moveSpeed * Time.deltaTime);

        //Checking if the object has reached to the level so we pass the next index position to it.
        if (Vector2.Distance(transform.position, wayPointPosition[wayPointIndex]) <= 0.1f)
        {
            if (wayPointIndex == wayPointPosition.Length - 1 || wayPointIndex == 0)
            {
                moveDirection *= -1; // Reverse direction
                StartCoroutine(StopMovement(cooldown)); // Stop movement for a while
            }


            wayPointIndex += moveDirection;
        }
    }


    private IEnumerator StopMovement(float delay)
    {
        canMove = false;
        
        yield return new WaitForSeconds(delay);

        canMove = true;
        sr.flipX = !sr.flipX; // Flip the sprite
    }
}
