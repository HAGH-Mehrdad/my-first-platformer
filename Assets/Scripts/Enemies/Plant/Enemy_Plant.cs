using UnityEngine;

public class Enemy_Plant : Enemy
{

    [Header("Plant details")]
    [SerializeField] private float attackCooldown = 2f; // Time between attacks
    private float lastTimeAttacked;


    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time >= lastTimeAttacked + attackCooldown;// to update the time to decide if the plant can attack or not

        if (isPlayerDetected && canAttack)
            Attack();
    }


    private void Attack()
    {
        lastTimeAttacked = Time.time; //To keep track of the last attack time

        anim.SetTrigger("attack");
    }


    protected override void HandleAnimation()
    {
        //Due to static nature of the plant enemy, we don't need to update the animation based on velocity [By not calling base mothod]
        //keep this empty unless you want to add specific animations for the plant enemy
    }
}