using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelCreation : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;// to create the button game object
    [SerializeField] private Transform buttonParent;// to be passed as the second parameter of Instantiate method



    private void Start()
    {
        CreateLevels();
    }

    public void CreateLevels()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;// minus 1 to exclude the end menu scene from buttons

        for (int i = 1; i < levelAmount; i++) // i starts with 1 to exclude main menu scene from buttons
        {
            UI_LevelButton newButton = Instantiate(buttonPrefab , buttonParent); // the third type of overload method

            newButton.SetupButton(i);
        }
    }
}
