using TMPro;
using Unity.Collections;
using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{

    private UI_LevelCreation levelCreation;
    private UI_MainMenu mainMenu; // to access SwithUI method

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

        mainMenu = GetComponentInParent<UI_MainMenu>(); // Get the main menu component to access SwitchUI method (it is in the Canvas game object)

        levelCreation = mainMenu.GetComponentInChildren<UI_LevelCreation>(true); // Get the level creation component to access SwitchUI method (it is in the level selection game object)
    }

    private void LoadSkinUnlocks()// to load the skins that are unlocked at the beginning of the game.
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
        {
            SkinManager.instance.SetSkinId(skinIndex);
            mainMenu.SwitchUI(levelCreation.gameObject); // Switch to the level creation UI when a skin is selected
        }

        UpdateSkinDisplay();
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
        if (HaveEnoughFruits(skinList[index].skinPrice) == false)
        {
            Debug.Log("Not enough fruits!");
            return;
        }

        string skinName = skinList[skinIndex].skinName;
        skinList[skinIndex].unlocked = true; // unlock the skin

        PlayerPrefs.SetInt(skinName + "Unlocked", 1); // save the skin as unlocked in PlayerPrefs
    }


    private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

    private bool HaveEnoughFruits(int price)
    {
        if (FruitsInBank() >= price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price); // deduct the price from the bank
            return true;
        }
        else
            return false;
    }
}
