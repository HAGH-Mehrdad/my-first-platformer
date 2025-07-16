using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelCreation : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;// to create the button game object
    [SerializeField] private Transform buttonParent;// to be passed as the second parameter of Instantiate method

    [SerializeField] private bool[] levelsUnlocked;

    private void Start()
    {
        LoadLevelInfo(); // Load the level information to know which levels are unlocked
        CreateLevels();
    }

    public void CreateLevels()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;// minus 1 to exclude the end menu scene from buttons

        for (int i = 1; i < levelAmount; i++) // i starts with 1 to exclude main menu scene from buttons
        {
            if(IsLevelUnlocked(i) == false)
                return; // If the level is not unlocked, we skip creating the button for that level

            UI_LevelButton newButton = Instantiate(buttonPrefab , buttonParent); // the third type of overload method

            newButton.SetupButton(i);
        }
    }

    private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

    private void LoadLevelInfo()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;

        levelsUnlocked = new bool[levelAmount];

        for (int i = 1; i < levelAmount; i++)
        {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;


            if(levelUnlocked)
                levelsUnlocked[i] = true;
        }

        levelsUnlocked[1] = true; // The first level is always unlocked, so we set it to true
    }
}
