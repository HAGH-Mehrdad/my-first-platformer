using System.Collections;
using UnityEngine;

public class FruitDroppedWhenDamaged : Fruit
{

    [SerializeField] private float[] waitTime;
    [SerializeField] private Color transparentColor;
    [SerializeField] private Vector2 velocity;

    bool canBePickedUp;


    protected override void Start()
    {
        base.Start();
        StartCoroutine(BlinkCoRoutine());
    }

    private void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }

    private IEnumerator BlinkCoRoutine()
    {
        anim.speed = 0;// Stop the animation while blinking

        foreach(float seconds in waitTime)
        {
            ToggleColorAndSpeed(transparentColor);

            yield return new WaitForSeconds(seconds);

            ToggleColorAndSpeed(Color.white);

            yield return new WaitForSeconds(seconds);
        }

        velocity.x = 0; // Stop horizontal movement
        anim.speed = 1;
        canBePickedUp = true;
    }


    private void ToggleColorAndSpeed(Color color)
    {
        sr.color = color;
        velocity.x = velocity.x * -1; // Change horizontal direction left and right
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBePickedUp == false) return;

        base.OnTriggerEnter2D(collision);
    }
}
