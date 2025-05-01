using UnityEngine;
using UnityEngine.UI; // Added for UI image support

public class GameOverLose : MonoBehaviour
{
    public GameObject GameOverLoseScreen;
    public RawImage webcamDisplay; // Reference to UI RawImage to display webcam output
    private WebCamTexture webcamTexture; // Reference to webcam texture
    private Texture2D capturedImage; // To store the captured image
    public int preferredWidth = 640;
    public int preferredHeight = 480;
    public int preferredFPS = 30;
    public Vector2 webcamPosition = new Vector2(-700, 100); // Webcam position (X, Y) - moved much further left

    void Start()
    {

        // Check if webcamDisplay is assigned
        if (webcamDisplay == null)
        {
            Debug.LogError("webcamDisplay RawImage is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Set position immediately
        PositionWebcamDisplay();

        // Initialize the webcam immediately
        InitializeWebcam();
    }

    private void PositionWebcamDisplay()
    {
        if (webcamDisplay != null)
        {
            // Make sure it's visible
            webcamDisplay.gameObject.SetActive(true);

            // Set position and size
            RectTransform rectTransform = webcamDisplay.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(500, 375); // Reduced size
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = new Vector2(-550, 100); // Adjusted back to the right slightly
                Debug.Log("Set webcam display position to: " + rectTransform.anchoredPosition);
            }
        }
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

    private void CaptureImage()
    {
        Debug.Log("CaptureImage called");
        // Only capture if webcam is available and running
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            try
            {
                // Create a texture to hold the captured image
                capturedImage = new Texture2D(webcamTexture.width, webcamTexture.height);

                // Copy the current webcam frame to the texture
                capturedImage.SetPixels(webcamTexture.GetPixels());
                capturedImage.Apply();
                Debug.Log("Image captured successfully");

                // Stop the webcam after capturing
                webcamTexture.Stop();

                // Display the captured image instead of the live feed
                webcamDisplay.texture = capturedImage;
                Debug.Log("Displaying captured image");

                // Ensure position is correct after setting new texture
                PositionWebcamDisplay();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error capturing image: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Failed to capture image: webcam is not available or not playing");
        }
    }

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

    public void HellButton()
    {
        SoundManager.GetInstance().buttonSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level666");
    }

    public void QuitButton()
    {
        SoundManager.GetInstance().buttonSound();
        Application.Quit();
    }
}
