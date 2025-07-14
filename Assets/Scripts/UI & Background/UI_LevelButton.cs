using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level_txt;

    private int levelIndex;
    private string sceneName;


    public void SetupButton(int newLevelindex)
    {
        levelIndex = newLevelindex;

        level_txt.text = "Level " + levelIndex; // The text of the button
        sceneName = "Level_" + levelIndex; // the format of levels starts with "Level_" and then adds the index
    }
    public void LoadLevel()//this will be assigned to the button to load the level that is created by the UI_LevelCreation.cs
    {
        SceneManager.LoadScene(sceneName);
    }
}
