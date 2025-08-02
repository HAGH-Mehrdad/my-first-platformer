using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private string firstLevel = "Level_1";// The name of the scene to load for a new game


    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private GameObject[] uiElements; // Array to hold different UI elements that can be switched

    [SerializeField] private GameObject continueButton; // Button to continue the game, if applicable

    [SerializeField] private MenuCharacter menuCharacter; // Reference to the MenuCharacter script for character movement in main menu

    [Header("Interactive Camera")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform mainMenuPoint;
    [SerializeField] private Transform skinSelectionPoint;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        if (HasLevelProgression())
            continueButton.SetActive(true); // If the player has a saved level progression, we enable the continue button
        else
            continueButton.SetActive(false); // Otherwise, we disable it

        fadeEffect.ScreenFade(0, fadeDuration);
    }

    public void SwitchUI(GameObject uiToEnable)// Method to switch between different UI elements & use it in skin ui selection 
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(false);
        }

        uiToEnable.SetActive(true);
        AudioManager.instance.PlaySfx(4);
    }


    public void NewGame() 
    {
        fadeEffect.ScreenFade(1, fadeDuration , LoadLevelScene);// For seeing the fade out effect clikcing the new game button
        AudioManager.instance.PlaySfx(4);
    }

    private void LoadLevelScene() => SceneManager.LoadScene(firstLevel);

    private bool HasLevelProgression()
    {
        bool hasProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0; // Check if the player has a saved level progression that is set in Gamemanager

        return hasProgression;
    }

    public void ContinueGame() // Continue Button
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty" , 1);

        DifficultyManager.instance.LoadDifficulty(difficultyIndex); // Load the saved difficulty level

        int lastSkinUsed = PlayerPrefs.GetInt("LastSkinUsed"); // Get the last skin used by the player
        SkinManager.instance.SetSkinId(lastSkinUsed); // Load the last skin used by the player

        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0); // Get the saved level number to continue
        SceneManager.LoadScene("Level_" + levelToLoad); // Load the saved level

        AudioManager.instance.PlaySfx(4);

    }

    public void MoveCameraToMainMenu()
    {
        menuCharacter.MoveTo(mainMenuPoint);
        cinemachineCamera.Follow = mainMenuPoint;
    }

    public void MoveCameraToSkinSelectionMenu()
    {
        menuCharacter.MoveTo(skinSelectionPoint);
        cinemachineCamera.Follow = skinSelectionPoint;
    }
}
