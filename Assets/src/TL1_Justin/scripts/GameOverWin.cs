using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverWin : MonoBehaviour
{
    public GameObject GameOverWinScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void RestartButton()
    {
        // Restart button has been pressed, initialize first game level
        Debug.Log("Restart button pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void MainMenuButton()
    {
        Debug.Log("Main menu button pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
