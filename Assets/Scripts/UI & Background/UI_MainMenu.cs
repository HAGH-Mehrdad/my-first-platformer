using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;// The name of the scene to load for a new game




    public void NewGame() 
    {
        SceneManager.LoadScene(sceneName);
    }
}
