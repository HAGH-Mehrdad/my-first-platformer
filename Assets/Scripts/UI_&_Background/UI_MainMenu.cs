using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private string firstLevel = "Level_1";// The name of the scene to load for a new game


    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private GameObject[] uiElements; // Array to hold different UI elements that can be switched

    [SerializeField] private GameObject continueButton; // Button to continue the game, if applicable

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

    public void SwitchUI(GameObject uiToEnable)
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(false);
        }

        uiToEnable.SetActive(true);
    }


    public void NewGame() 
    {
        fadeEffect.ScreenFade(1, fadeDuration , LoadLevelScene);// For seeing the fade out effect clikcing the new game button
    }

    private void LoadLevelScene() => SceneManager.LoadScene(firstLevel);

    private bool HasLevelProgression()
    {
        bool hasProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0; // Check if the player has a saved level progression that is set in Gamemanager

        return hasProgression;
    }

    public void ContinueGame()
    {
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0); // Get the saved level number to continue
        SceneManager.LoadScene("Level_" + levelToLoad); // Load the saved level
    }
}
