using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpMenu : MonoBehaviour
{
    public GameObject HelpMenuScreen;

    public void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
