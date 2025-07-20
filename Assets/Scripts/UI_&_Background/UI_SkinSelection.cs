using TMPro;
using Unity.Collections;
using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{

    [SerializeField] private Skin[] skinList;

    [Header("UI details")]
    [SerializeField] private int skinIndex;
    [SerializeField] private int maxIndex;// depending on how many characters we have minus one
    [SerializeField] private Animator skinDisplay;

    [Header("txt Info")]
    [SerializeField] private TextMeshProUGUI txtBuy_Select;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtBank;

    private void Start()
    {
        LoadSkinUnlocks();
        UpdateSkinDisplay();
    }

    private void LoadSkinUnlocks()
    {
        for (int i = 1; i < skinList.Length; i++)//starts at one because no matter what the first skin is unlocked
        {
            string skinName = skinList[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if(skinUnlocked)
                skinList[i].unlocked = true; // if the skin is unlocked, set it to true in the skinList
        }
    }

    public void SelectSkin()
    {
        if (skinList[skinIndex].unlocked == false)
            BuySkin(skinIndex);
        else
            SkinManager.instance.SetSkinId(skinIndex);
    }

    public void NextSkin()
    {
        skinIndex++;

        if (skinIndex > maxIndex)
        {
            skinIndex = 0;//scoll between skins endlessly.
        }

        UpdateSkinDisplay();
    }


    public void PreviousSkin()
    {
        skinIndex--;

        if (skinIndex < 0)
        {
            skinIndex = maxIndex;//scoll between skins endlessly.
        }

        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        txtBank.text = "Bank: " + FruitsInBank(); //display the amount of fruits in the bank


        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(skinIndex, 1);

        if (skinList[skinIndex].unlocked)
        {
            txtPrice.transform.parent.gameObject.SetActive(false); // disable entire the price text object
            txtBuy_Select.text = "Select";
        }
        else
        {
            txtPrice.transform.parent.gameObject.SetActive(true); // enable entire the price text object
            txtPrice.text = "Price: " + skinList[skinIndex].skinPrice; //display the price of the skin
            txtBuy_Select.text = "Buy";
        }
    }

    private void BuySkin(int index)// the functionality is save the information about the skin
    {
        string skinName = skinList[skinIndex].skinName;
        skinList[skinIndex].unlocked = true; // unlock the skin

        PlayerPrefs.SetInt(skinName + "Unlocked", 1); // save the skin as unlocked in PlayerPrefs
    }


    private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");
}
