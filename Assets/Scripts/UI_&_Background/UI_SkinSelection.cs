using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex;// depending on how many characters we have minus one
    [SerializeField] private Animator skinDisplay;



    public void SelectSkin()
    {
        SkinManager.instance.SetSkinId(currentIndex);
    }

    public void NextSkin()
    {
        currentIndex++;

        if (currentIndex > maxIndex)
        {
            currentIndex = 0;//scoll between skins endlessly.
        }

        UpdateSkinDisplay();
    }


    public void PreviousSkin()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = maxIndex;//scoll between skins endlessly.
        }

        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);
    }
}
