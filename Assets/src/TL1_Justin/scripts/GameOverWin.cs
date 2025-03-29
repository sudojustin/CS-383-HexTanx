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
        // FIXME: load Malik's first scene
        Debug.Log("Restart button pressed");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameLevel1");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/src/TL2_Malik/Assets/Scenes/SampleScene.unity");
    }

    public void MainMenuButton()
    {
        Debug.Log("Main menu button pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/src/TL1_Justin/scenes/MainMenu.unity");
    }

    public void QuitButton()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
