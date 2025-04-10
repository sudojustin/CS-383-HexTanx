using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverWin : MonoBehaviour
{
    public GameObject GameOverWinScreen;

    public void RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
