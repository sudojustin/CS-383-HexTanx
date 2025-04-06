using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void PlayButton()
    {
        // Play button has been pressed, initialize first game level
        // FIXME: load Malik's first scene
        Debug.Log("Play button pressed");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameLevel1");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level1.unity");
    }

    public void QuitButton()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
