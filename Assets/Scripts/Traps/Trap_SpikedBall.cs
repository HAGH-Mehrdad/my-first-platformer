using UnityEngine;

public class Trap_SpikedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spikerb;
    [SerializeField] private float pushForce;


    private void Start()
    {
        Vector2 pushVector = new Vector2 (pushForce , 0f);
        spikerb.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
