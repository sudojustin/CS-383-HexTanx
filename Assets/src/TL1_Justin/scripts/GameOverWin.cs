using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverWin : MonoBehaviour
{
    public GameObject GameOverWinScreen;

    public void RestartButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void MainMenuButton()
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
