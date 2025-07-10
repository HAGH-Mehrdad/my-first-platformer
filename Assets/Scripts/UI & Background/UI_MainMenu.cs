using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private string sceneName;// The name of the scene to load for a new game


    [SerializeField] private float fadeDuration = 1.5f;


    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0,fadeDuration);
    }


    public void NewGame() 
    {
        fadeEffect.ScreenFade(1, fadeDuration , LoadLevelScene);// For seeing the fade out effect clikcing the new game button
    }

    private void LoadLevelScene() => SceneManager.LoadScene(sceneName);
}
