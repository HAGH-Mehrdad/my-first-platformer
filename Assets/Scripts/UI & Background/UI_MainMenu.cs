using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private string sceneName;// The name of the scene to load for a new game


    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private GameObject[] uiElements; // Array to hold different UI elements that can be switched


    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0,fadeDuration);
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

    private void LoadLevelScene() => SceneManager.LoadScene(sceneName);
}
