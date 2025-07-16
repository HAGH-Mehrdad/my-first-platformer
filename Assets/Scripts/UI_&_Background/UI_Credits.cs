using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private RectTransform rectT;
    [SerializeField] private float scrollSpeed = 200f;
    [SerializeField] private float offScreenPosition = 2500f;


    [SerializeField] private string mainMenuSceneName = "MainMenu";


    private bool creditsSkipped;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
        fadeEffect.ScreenFade(0, 1);
    }

    private void Update()
    {
        rectT.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if(rectT.anchoredPosition.y > offScreenPosition)
            GoToMainMenu();
    }

    public void SkipCredits() // We declare it public so it can be called from the UI button (it is part of the canvas component)
    {
        if (creditsSkipped == false)
        {
            scrollSpeed *= 10f;
            creditsSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }


    private void GoToMainMenu() => fadeEffect.ScreenFade(1,1,SwitchToMenuScene);

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
