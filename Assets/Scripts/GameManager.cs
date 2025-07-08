using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("Player")]
    [SerializeField] private GameObject playerPrefab; // Different from the Player's class
    [SerializeField] private Transform respawnPoint; //The place that player knows where to respawn
    [SerializeField] private float respawnDelay;
    public Player player;// we need to refill this parameter when the player respawns

    [Header("Fruits Managment")]
    public bool fruitsAreRandom;
    public int fruitCollected;
    public int totalFruits;

    [Header("Checkpoint")]
    public bool canReactivated; // If true, the checkpoint can be activated again and again (we can reactivate it in the inspector)

    [Header("Arrow")]
    public GameObject arrowPrefab;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        CollectFruitsInfo();

    }

    private void CollectFruitsInfo()
    {
        //totalFruits = FindObjectsOfType<Fruit>().Length; this is deprecated


        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    public void RespawnPlayer()
    {
        //Becasue we want to refill the player parameter and line below is game object we do it in a better way
        //player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity).GetComponent<Player>();


        //For bettter undestanding and reliability I choose to refill player object in 2 steps (respawn coroutine body)

        StartCoroutine(RespawnCoroutine());

    }


    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);

        player = newPlayer.GetComponent<Player>();
    }


    public void AddFruit() => fruitCollected++;
    public bool FruitsHaveRandomLook() => fruitsAreRandom;


    //This method is versitile and if I want to pull another object from the game manager, I can
    public void CreateGameObject(GameObject prefab, Transform target, float delay = 0)
    {
        StartCoroutine(CreateGameObjectCoroutine(prefab, target, delay));
    }

    private IEnumerator CreateGameObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 position = target.position;//We store the target trasform first becuase later we might loose the trasform of the game object we want to instantiate.
        
        yield return new WaitForSeconds(delay);


        GameObject newGameObject = Instantiate(prefab, position, Quaternion.identity);
    }

    public void LoadEndScene() => SceneManager.LoadScene("TheEnd");
}
