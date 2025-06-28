using UnityEngine;

public class Enemy_Plant : Enemy
{
    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
            Attack();
    }


    private void Attack()
    {
        anim.SetTrigger("attack");
    }
}