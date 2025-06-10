using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected float moveSpeed;


    protected int facingDirection = -1; // 1 for right, -1 for left


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

}
