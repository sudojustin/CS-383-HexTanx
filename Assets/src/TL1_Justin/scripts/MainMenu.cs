using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RawImage webcamDisplay; // Reference to UI RawImage to display webcam output
    private WebCamTexture webcamTexture; // Reference to webcam texture
    private Texture2D capturedImage; // To store the captured image
    public int preferredWidth = 640;
    public int preferredHeight = 480;
    public int preferredFPS = 30;
    public GameObject MainMenuScreen;

    public void Start()
    {
        string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LostFromLevel", currentLevel);
        PlayerPrefs.Save();
        InitializeWebcam();
    }

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

    public void InitializeWebcam()
    {
        Debug.Log("Initializing webcam. Available devices: " + WebCamTexture.devices.Length);

        if (WebCamTexture.devices.Length > 0)
        {
            // Log all available devices
            foreach (WebCamDevice device in WebCamTexture.devices)
            {
                Debug.Log($"Found webcam: {device.name}, IsFrontFacing: {device.isFrontFacing}");
            }

            // Find the first front-facing camera
            WebCamDevice? selectedDevice = null;
            foreach (WebCamDevice device in WebCamTexture.devices)
            {
                if (device.isFrontFacing)
                {
                    selectedDevice = device;
                    break;
                }
            }

            // If no front-facing camera is found, fall back to the default (first) camera
            if (selectedDevice.HasValue)
            {
                webcamTexture = new WebCamTexture(selectedDevice.Value.name, preferredWidth, preferredHeight, preferredFPS);
                Debug.Log($"Selected front-facing webcam: {selectedDevice.Value.name}");
            }
            else
            {
                webcamTexture = new WebCamTexture(preferredWidth, preferredHeight, preferredFPS);
                Debug.Log("No front-facing webcam found, using default webcam.");
            }

            webcamDisplay.texture = webcamTexture;
            Debug.Log("Assigned webcam texture to RawImage");


            webcamTexture.Play();
            Debug.Log("Started webcam. Is playing: " + webcamTexture.isPlaying);

            Invoke("CaptureImage", 1.0f);
        }
        else
        {
            Debug.LogWarning("No webcam found on this device!");
        }
    }
}
