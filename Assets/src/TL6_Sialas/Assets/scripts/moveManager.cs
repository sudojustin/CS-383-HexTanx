using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerModeManager : MonoBehaviour
{
    public enum PlayerMode
    {
        Move,
        Shoot
    }

    private PlayerMode currentMode = PlayerMode.Move; // Default to Move Mode
    private Button toggleButton;
    private Text buttonText; // Optional: To display the current mode on the button
    private Canvas canvas;

    private void Awake()
    {
        //// Ensure the Canvas persists across scenes
        //DontDestroyOnLoad(gameObject);

        // Get the Canvas component
        canvas = GetComponent<Canvas>();

        // Find the toggle button (assumes it's a child of the Canvas)
        toggleButton = GetComponentInChildren<Button>();

        // Find the button's Text component 
        buttonText = toggleButton.GetComponentInChildren<Text>();


        // Set up the button click
        toggleButton.onClick.AddListener(ToggleMode);

        // Update the button text to reflect the initial mode
        UpdateButtonText();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Show the button only in level scenes
        string sceneName = scene.name.ToLower();
        bool isLevelScene = sceneName.StartsWith("level");
        canvas.enabled = isLevelScene;
    }

    private void ToggleMode()
    {
        // Toggle between Move and Shoot modes
        currentMode = (currentMode == PlayerMode.Move) ? PlayerMode.Shoot : PlayerMode.Move;
        UpdateButtonText();
        Debug.Log($"Player mode switched to: {currentMode}");
    }

    private void UpdateButtonText()
    {
        buttonText.text = currentMode == PlayerMode.Move ? "Move Mode" : "Shoot Mode";
    }

    // Public method to get the current mode
    public PlayerMode GetCurrentMode()
    {
        return currentMode;
    }
}