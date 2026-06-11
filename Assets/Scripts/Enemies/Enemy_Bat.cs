using UnityEngine;

public class Enemy_Bat : Enemy
{

    [Header("Bat Detail")]
    [SerializeField] private float agroRadius;
    [SerializeField] private float chaseDuration;
    [SerializeField] private float attackSpeed;

    private float defaultSpeed;
    private float chaseTimer;
    private Vector3 originalPosition;
    private Vector3 destination;

    private bool canDetectPlayer;
    private Collider2D target;

    protected override void Awake()
    {
        base.Awake();
        defaultSpeed = moveSpeed;
        originalPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        base.Update();

        chaseTimer -= Time.deltaTime;

        if (idleTimer < 0)
        {
            canDetectPlayer = true;
        }

        HandleMovement();
        HandlePlayerDetection();
    }

    private void HandleMovement()
    {
        if (canMove == false)
            return;

        if (chaseTimer > 0)
        {
            destination = target.transform.position;
        }
        else
        {
            //Charging the enemy toward the player after chasing is done
            moveSpeed = attackSpeed;
        }

            HandleFlip(destination.x);
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        //check if the enemy reached the player
        if (Vector2.Distance(transform.position, destination) < .1f)
        {
            if (destination == originalPosition)
            {
                idleTimer = idleDuration;
                canDetectPlayer = false;
                canMove = false;
                target = null;
                anim.SetBool("isMoving", false);
                moveSpeed = defaultSpeed;
            }
            else
            {
                destination = originalPosition;
            }
        }
    }

    private void HandlePlayerDetection()
    {
        if (target == null && canDetectPlayer)
        {
            chaseTimer = chaseDuration;
            target = Physics2D.OverlapCircle(transform.position, agroRadius, whatIsPlayer);

            if (target != null)
            {
                canDetectPlayer = false;
                destination = target.transform.position;
                canMove = true;
                anim.SetBool("isMoving", true);
            }
        }
    }

    private void AllowMovement() => canMove = true;

    protected override void HandleAnimation()
    {
        
    }

    public override void Die()
    {
        base.Die();

        canMove = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }
}
