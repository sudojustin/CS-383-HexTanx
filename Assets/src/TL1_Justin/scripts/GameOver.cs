using UnityEngine;

public class GameOver : MonoBehaviour
{
    public CanvasGroup gameOverCanvas;

    public void ShowGameOverScreen()
    {
        gameOverCanvas.alpha = 1f;
        gameOverCanvas.interactable = true;
        gameOverCanvas.blocksRaycasts = true;
    }

    public void RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
