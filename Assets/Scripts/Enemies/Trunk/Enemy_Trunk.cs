using UnityEngine;

public class Enemy_Trunk : Enemy
{

    [Header("Trunk details")]
    [SerializeField] private Enemy_Bullet bulltePrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Vector2 bulletSpeed;
    [SerializeField] private float attackCooldown = 2f; // Time between attacks
    private float lastTimeAttacked;


    protected override void Update()
    {
        base.Update();


        bool canAttack = Time.time >= lastTimeAttacked + attackCooldown;// to update the time to decide if the plant can attack or not

        if (isPlayerDetected && canAttack)
            Attack();

        if (isDead)
            return;

        HandleMovement();
        HandleTurnAround();
    }


    private void Attack()
    {
        idleTimer = idleDuration + attackCooldown; // Reset the idle timer when the enemy attacks
        lastTimeAttacked = Time.time; //To keep track of the last attack time
        anim.SetTrigger("attack");
    }

    private void CreateBullet() // Creating the projectile
    {
        Enemy_Bullet bullet = Instantiate(bulltePrefab, gunPoint.position, Quaternion.identity);
        bullet.SetVelocity(bulletSpeed * facingDir);
    }

    private void HandleTurnAround()
    {
        //we can check this condition on update method but due to Encapsulation, readability and flexibility we check isGrounded here.
        if (!isGrounded)
            return; // If the enemy is not grounded, do not flip [When the designer didn't place the enemy on the ground before play]

        if (!isGroundAheadDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration; // Reset the idle timer when the enemy is not grounded or hits a wall (wants to flip!)
            rb.linearVelocityX = 0; // Stop moving when the enemy is not grounded or hits a wall
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return; // If the idle [animation] timer is not finished, do not move

        if (isGroundAheadDetected)//this condition removes jittering effect when the player is falling from a platform or is simply not on the ground
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }
}
