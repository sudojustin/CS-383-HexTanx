using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpMenu : MonoBehaviour
{
    public GameObject HelpMenuScreen;

    public void BackButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        SoundManager.GetInstance().buttonSound();
        Application.Quit();
    }
}
