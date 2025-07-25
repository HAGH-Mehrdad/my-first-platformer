using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.75f; // Duration for the fade effect

    public static UI_InGame instance; // this component will be available from anywhere
    public UI_FadeEffect fadeEffect { get; private set; }//read-only property to access the fade effect component


    [SerializeField] private TextMeshProUGUI timerTxt;// Text component to display the timer
    [SerializeField] private TextMeshProUGUI fruitTxt;// Text component to display the fruit count

    [Header("Pause menu reference")]
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu UI

    private bool isPaused = false; // Flag to check if the game is paused

    private void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, fadeDuration);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonPressed();
        }
    }

    public void PauseButtonPressed()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
            pauseMenu.SetActive(false); // Hide the pause menu
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f; // Pause the game
            pauseMenu.SetActive(true); // Show the pause menu
        }
    }

    public void ResumeButton()
    {
        isPaused = false; // Set the pause flag to false
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
    }

    public void MainMenuButton()
    {
        isPaused = false; // Set the pause flag to false
        pauseMenu.SetActive(false); // Hide the pause menu
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        Time.timeScale = 1f; // Ensure the game is resumed when returning to the main menu
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        fruitTxt.text = $"{collectedFruits} / {totalFruits}";
    }

    public void UpdateTimerUI(float time)
    {
        timerTxt.text = time.ToString("00" + " s");//format the time to always show two digits
    }
}
