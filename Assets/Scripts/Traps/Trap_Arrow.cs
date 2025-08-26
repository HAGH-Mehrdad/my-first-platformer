using Unity.VisualScripting;
using UnityEngine;

public class Trap_Arrow : Trap_Trampoline
{
    [Header("Additional Info")]
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private bool rotatingRight;// Deciding to rotate to left or right
    [SerializeField] private float rotationSpeed = 10f;
    private int direction = -1;

    [Space]
    [SerializeField] private float regrowSpeed = 1f;
    [SerializeField] private Vector3 targetScale;


    private void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // The beginnign scale of the arrow.
    }



    private void Update()
    {
        HandleScaleUp();

        HandleRotation();

    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x) // We can't access the whole scale. se we only check the x value of scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, regrowSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        direction = rotatingRight ? -1 : 1;

        transform.Rotate(0, 0, (rotationSpeed * direction) * Time.deltaTime);
    }

    //Instead of adding destroy method, I added DestroyEvent.cs to the game object and call it from the last animation of the "activates".
    //But I think for instantiating it after cooldown requires a specefic method. :)))

    private void DestroyMe()
    {
        GameObject arrow = ObjectCreator.instance.arrowPrefab;

        ObjectCreator.instance.CreateGameObject(arrow, transform, false,cooldown);

        Destroy(gameObject);
    }

    
}
