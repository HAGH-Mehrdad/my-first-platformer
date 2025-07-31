using UnityEngine;
using UnityEngine.Windows;

public class MenuCharacter : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed;

    private Vector3 destinationPoint;


    private bool isMoving;
    private int facingDir = 1;
    private bool facingRight = true;

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

        HandleFlip(destinationPoint.x);
        isMoving = true;
    }

    private void HandleFlip(float xValue)
    {
        //if the player is running to left and facing to right OR if it is running to right and facing to left we need to flip the character.
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}
