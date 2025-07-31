using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private Transform menuPoint;
    [SerializeField] private Transform skinSelectionPoint;

    private Vector3 destinationPoint;
    private bool isMoving;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, speed * Time.deltaTime);
            //anim.SetBool("isMoving", true);

            if (Vector2.Distance(transform.position, destinationPoint) < 0.1f)
            {
                isMoving = false;
                //anim.SetBool("isMoving", false);
            }
        }
    }

    public void MoveTo(Transform newDestination)
    {
        destinationPoint = newDestination.position;
        destinationPoint.y = newDestination.position.y;  // to igonre y-axis movement between the player and the game object that is planed for the destination point

        isMoving = true;
    }
}
