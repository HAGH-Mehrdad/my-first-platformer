using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;


    [Header("Player")]
    [SerializeField] private GameObject playerPrefab; // Different from the Player's class
    [SerializeField] private Transform respawnPoint; //The place that player knows where to respawn
    [SerializeField] private float respawnDelay;
    public Player player;// we need to refill this parameter when the player respawns

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject); // Ensure only one instance of PlayerManager exists
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>(); // We find the first player in the scene and use its transform as the respawn point
        }

        if (respawnPoint == null)
        {
            respawnPoint = FindFirstObjectByType<StartPoint>().transform; // We find the first player in the scene and use its transform as the respawn point
        }
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
        {
            return; // In hard mode, we don't respawn the player, we just let them die
        }

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
}
