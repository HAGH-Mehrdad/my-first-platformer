using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{

    [Header("Level Info")]
    [SerializeField] private TextMeshProUGUI level_txt;

    [Header("Time & Fruits")]
    [SerializeField] private TextMeshProUGUI txtBestTime;
    [SerializeField] private TextMeshProUGUI txtFruit;

    private int levelIndex;
    private string sceneName;


    public void SetupButton(int newLevelindex)
    {
        levelIndex = newLevelindex;

        level_txt.text = "Level " + levelIndex; // The text of the button
        sceneName = "Level_" + levelIndex; // the format of levels starts with "Level_" and then adds the index

        txtBestTime.text = TimerInfotxt(); // The best time of the level
        txtFruit.text = FruitsInfotxt(); // The fruits collected and total fruits of the level
    }
    public void LoadLevel()//this will be assigned to the button to load the level that is created by the UI_LevelCreation.cs
    {
        int difficultyIndex = ((int)DifficultyManager.instance.difficulty);
        PlayerPrefs.SetInt("GameDifficulty", difficultyIndex);
        SceneManager.LoadScene(sceneName);
    }

    private string TimerInfotxt()
    {

        float timerValue = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime");

        string txtTimer = timerValue == 0 ? "--" : timerValue.ToString("00");

        return "Best Time : " + txtTimer;
    }

    private string FruitsInfotxt()
    {
        int totalFruitsValue = PlayerPrefs.GetInt("Level" + levelIndex + "totalFruits", 0);

        string txtTotalFruits = totalFruitsValue == 0 ? "?" : totalFruitsValue.ToString(); //if it is zero then ? will be shown in total txtTotalFruits

        int fruitsCollectedValue = PlayerPrefs.GetInt("Level" + levelIndex + "FruitsCollected", 0);

        string txtFruitsCollected = fruitsCollectedValue == 0 ? "--" : fruitsCollectedValue.ToString(); //if no fruits is collected then show -- in txtCollectedFruits

        return "Fruits : " + $"{txtFruitsCollected} / {txtTotalFruits}";
    }
}
