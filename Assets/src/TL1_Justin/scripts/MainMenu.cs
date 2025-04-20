using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuScreen;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level666.unity");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/EasterLevel.unity");
        }
    }

    public void PlayButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level1.unity");
    }

    public void HelpButton()
    {
        // Help button has been pressed, show help menu
        Debug.Log("Help button pressed");
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("HelpMenu");
    }

    public void QuitButton()
    {
        // Debug.Log("Quit button pressed");
        SoundManager.GetInstance().buttonSound();
        Application.Quit();
    }

    public void LevelSelectorButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelector");
    }
}
