using UnityEngine;

public class Trap_FallingPlatform : MonoBehaviour
{
    //We don't use inheritance here, but we can use it if we want to (There is no reason to use for one method and one variable)

    [SerializeField] private float speed = 1f;
    [SerializeField] private float travelDistance;

    private  Vector3[] waypoints;
    private int wayPointIndex;
    public bool canMove;


    private void Start()
    {
        StartWayPoint();
    }

    private void Update()
    {
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
        if (!canMove)
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
}
