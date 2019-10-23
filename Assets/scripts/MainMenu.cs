using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // it's now using LevelChanger function FadeToNextLevel to go to the next level
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
