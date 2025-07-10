using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    [SerializeField] private Image fadeImage;



    public void ScreenFade(float targetAlpha, float duration, System.Action onComplete = null)
    {
        StartCoroutine(FadeCoroutine(targetAlpha, duration, onComplete));
    }


    private IEnumerator FadeCoroutine(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;

        Color currentColor = fadeImage.color;

        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp (startAlpha, targetAlpha, time/duration);

            fadeImage.color = new Color (currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }

        // after the while this line executes to reach the intended target alpha
        //some times the alpha value might become less or a liitle above the target value
        fadeImage.color = new Color (currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        onComplete?.Invoke();
    }
}
