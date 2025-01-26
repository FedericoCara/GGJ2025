using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string gameplayScene = "PlayerTest";
    
    public void Play()
    {
        SceneManager.LoadScene(gameplayScene);
    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
