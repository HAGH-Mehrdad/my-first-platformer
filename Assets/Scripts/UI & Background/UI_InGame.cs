using UnityEditor.PackageManager;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance; // this component will be available from anywhere
    public UI_FadeEffect fadeEffect;

    private void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, 0.5f);
    }
}
