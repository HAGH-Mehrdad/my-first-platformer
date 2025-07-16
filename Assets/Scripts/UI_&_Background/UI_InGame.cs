using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.75f; // Duration for the fade effect

    public static UI_InGame instance; // this component will be available from anywhere
    public UI_FadeEffect fadeEffect { get; private set; }//read-only property to access the fade effect component


    [SerializeField] private TextMeshProUGUI timerTxt;// Text component to display the timer
    [SerializeField] private TextMeshProUGUI fruitTxt;// Text component to display the fruit count

    private void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, fadeDuration);
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        fruitTxt.text = $"{collectedFruits}/{totalFruits}";
    }

    public void UpdateTimerUI(float time)
    {
        timerTxt.text = time.ToString("00" + " s");//format the time to always show two digits
    }
}
