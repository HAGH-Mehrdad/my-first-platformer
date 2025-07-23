using UnityEngine;

public class UI_Difficulty : MonoBehaviour
{
    private DifficultyManager difficultyManager;


    private void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    public void SetEasyMode() => difficultyManager.SetupDifficulty(DifficultyType.Easy);

    public void SetNormalMode() => difficultyManager.SetupDifficulty(DifficultyType.Normal);

    public void SetHardMode() => difficultyManager.SetupDifficulty(DifficultyType.Hard);
}
