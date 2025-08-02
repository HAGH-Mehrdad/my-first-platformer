using UnityEngine;


public class Fruit : MonoBehaviour
{
    //enum is defined to choose what type of fruit it is if we want to off the random option on GameManager
    //fruit type is stored on FruitType.cs for cleaner code
    [SerializeField] private FruitType fruitType;
    [SerializeField] GameObject pickupVFX;

    private GameManager gameManager;

    private Animator anim;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }


    // We do it in start because if we do it in awake it may happen that awake in this script may be executed first than the game manager instance
    // if it executed before, it may return null when we try to get the instance
    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomLookIfNeeded();
    }


    private void SetRandomLookIfNeeded()
    {
        if (!gameManager.FruitsHaveRandomLook())
        {
            UpdateFruitVisuals();
            return;
        }


        int randomIndex = Random.Range(0 , 8);// max value is exclusive, so it will give number from 0 to 7. In animator blend tree we have 7 animations.
        anim.SetFloat("fruitIndex", randomIndex);
    }

    private void UpdateFruitVisuals() => anim.SetFloat("fruitIndex", (int)fruitType);// Because fruitType is an enum, we can cast it to int to get the index of the fruit type in the animator blend tree.


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            gameManager.AddFruit();

            AudioManager.instance.PlaySfx(8);

            Destroy(gameObject);

            
            // Please Remember that if you want to manipulate the instantiated game object you should declare a local variable and store in it.
            GameObject newFX = Instantiate(pickupVFX, transform.position, Quaternion.identity);
            //Destroy(newFX); // One way of preventing from cluttering VFXs is destroying them here after a time but I decided to use events on the VFX itself
        }
    }
}
