using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpMenu : MonoBehaviour
{
    public GameObject HelpMenuScreen;

    public void BackButton()
    {
        // Back button has been pressed, return to main menu
        Debug.Log("Back button pressed");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        // Assets/Scenes/MainMenu.unity
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/src/TL2_Malik/Assets/Scenes/SampleScene.unity");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
    }

    public void QuitButton()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
