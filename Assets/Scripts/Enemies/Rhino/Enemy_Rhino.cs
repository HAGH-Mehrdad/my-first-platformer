using System;
using Unity.Cinemachine;
using UnityEngine;

public class Enemy_Rhino : Enemy
{

    [Header("Rhino details")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speedUpRate = 1.0f;//rate of speed change
    [SerializeField] private Vector2 impactForce = new Vector2(4,2);// I think it is better to assign default values. Because at the editor I forgot to do it at first


    [Header("Effects")]
    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private Vector2 cameraImpactDir;
    private CinemachineImpulseSource impulseSource;


    private float defaultSpeed;


    protected override void Start()
    {
        base.Start();

        canMove = false; // Rhino will not move until it detects the player at the beginning

        defaultSpeed = moveSpeed;//Saving the moving speed

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    protected override void Update()
    {
        base.Update();

        HandleCharge();
    }

    private void HitWallImpact()
    {
        dustFX.Play();
        impulseSource.DefaultVelocity = new Vector2(cameraImpactDir.x * facingDir, cameraImpactDir.y);
        impulseSource.GenerateImpulse();
    }


    private void HandleCharge()//stands still untill it detects the player
    {
        if (canMove == false)
            return;

        HandleSpeedUp();

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);

        if (isWallDetected)
            WallHit();


        if (!isGroundAheadDetected)
            TurnAround();
    }

    private void HandleSpeedUp()
    {
        moveSpeed = moveSpeed + (speedUpRate * Time.deltaTime);

        if (moveSpeed >= maxSpeed)
            moveSpeed = maxSpeed;
    }

    private void TurnAround()
    {
        SpeedReset();
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        Flip();
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
    }

    private void WallHit()
    {
        canMove = false;// to prevent intervention of velocity in charge method
        HitWallImpact();
        SpeedReset();//reseting the speed
        anim.SetBool("wallHit" , true);
        rb.linearVelocity = new Vector2(impactForce.x * -facingDir, impactForce.y); // A problem I had here was that I declared Vector2 filed and I passed impactforce itself & linearVelocity.y
    }

    private void ChargeIsOver()// this method will be called when the hit animation is over
    {
        anim.SetBool("wallHit" , false);
        Invoke(nameof(Flip), 1); // after he hit the wall, flips 
    }

    protected override void HandleCollision() //this method is no longer in update but it will be executed due to its prescese in parent class
    {
        base.HandleCollision();

        if(isPlayerDetected && isGrounded)// to prevent rhino to flip if it detects the player just after it hits a wall (isGrounded)
            canMove = true;
    }
}
