using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    public GameObject LevelSelectorScreen;

    public void BackButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Level1Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void Level2Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }

    public void Level3Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }

    public void Level4Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level4");
    }

    public void Level5Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level5");
    }

    public void Level6Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level6");
    }

    public void Level7Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level7");
    }

    public void Level8Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level8");
    }

    public void Level9Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level9");
    }

    public void Level10Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level10");
    }

    public void Level666Button()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level666");
    }

    public void LevelEasterButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("EasterLevel");
    }
}