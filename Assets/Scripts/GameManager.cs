using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private UI_InGame uiInGame; // This will be used to access UI_InGame component because we use it a couple of times in this script

    [Header("Level Managment")]
    [SerializeField] private float levelTimer; // Timer for the level, can be used to track time taken to complete the level
    [SerializeField] private int currentLevelIndex;
    //nextLevelIndex isn't incremented; it's re-calculated based on the currentLevelIndex of the scene the GameManager is currently active in.
    private int nextLevelIndex; // We are going to use this variable to load the next level in many places


    [Header("Player")]
    [SerializeField] private GameObject playerPrefab; // Different from the Player's class
    [SerializeField] private Transform respawnPoint; //The place that player knows where to respawn
    [SerializeField] private float respawnDelay;
    public Player player;// we need to refill this parameter when the player respawns

    [Header("Fruits Managment")]
    public bool fruitsAreRandom;
    public int fruitsCollected;
    public int totalFruits;
    public int allFruits;

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
        uiInGame = UI_InGame.instance; // We can access the UI_InGame component from the GameManager instance

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;// to have the information about what level we are in.
        nextLevelIndex = currentLevelIndex + 1; // We can use this variable to load the next level in many places

        CollectFruitsInfo();
        ShowAllFruitsInBank(); // to know how many fruits does the player has collected
    }

    private void Update()
    {
        //timer = Time.time; // This line is deprecated because it returns the time since the start of the game, not the time since the level started.
        levelTimer += Time.deltaTime; // We use Time.deltaTime to get the interval in seconds since the last frame, so we can update the timer every frame

        uiInGame.UpdateTimerUI(levelTimer);
    }

    private void CollectFruitsInfo()// to indicate how many fruits we have in the level and how many we have collected
    {
        //totalFruits = FindObjectsOfType<Fruit>().Length; this is deprecated


        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;

        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "totalFruits", totalFruits);
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


    public void AddFruit()//pick up fruit method
    {
        fruitsCollected++;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruitEasyMode()
    {
        fruitsCollected--;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruitNormalMode()
    {
        fruitsCollected -= 2;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public int FruitsCollected() => fruitsCollected;

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


    public void LevelFinished()
    {
        SaveLevelProgression();

        SaveBestTime();

        SaveFruitsInfo();

        LoadNextScene();
    }

    private void SaveFruitsInfo()
    {

        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected", 0);
        
        if( fruitsCollectedBefore < fruitsCollected)// We only save the fruits collected if the current value is greater than the previous one[Best collects]
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");

        PlayerPrefs.SetInt("TotalFruitsAmount" , totalFruitsInBank + fruitsCollected); // We save the total fruits collected in the game so far

    }

    private void ShowAllFruitsInBank()
    {
        allFruits = PlayerPrefs.GetInt("TotalFruitsAmount");
    }

    private void SaveBestTime()// Save the time that the player took to complete the level
    {
        float lastTimeFinished = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime",999);

        if(levelTimer < lastTimeFinished)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime" , levelTimer);
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1); // We save the next level as unlocked in PlayerPrefs so that we can access it later

        if (NoMoreLevels() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex); // We save the next level number to use it for continue button in the main menu
    }

    public void RestartLevel()
    {
        UI_InGame.instance.fadeEffect.ScreenFade(1, 0.5f ,LoadCurrentScene);
    }

    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);

    private void LoadEndScene() => SceneManager.LoadScene("TheEnd");
    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level_" +  nextLevelIndex);
    }

    private void LoadNextScene()
    {
        UI_FadeEffect fadeEffect = UI_InGame.instance.fadeEffect;

        if (NoMoreLevels())
            fadeEffect.ScreenFade(1, 0.5f, LoadEndScene); // created an instance above so we call it with the new variable
        else
            fadeEffect.ScreenFade(1, 0.5f, LoadNextLevel);
    }

    private bool NoMoreLevels()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;// We have main and end scenes that's why we subtract by 2

        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        return noMoreLevels;
    }
}
