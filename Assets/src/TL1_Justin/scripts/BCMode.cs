using UnityEngine;
using UnityEngine.UI;

// This script manages a toggle for "BC Mode"
// It saves the toggle state to PlayerPrefs so it persists between game sessions
public class BCMode : MonoBehaviour
{
    // Reference to the UI Toggle component that controls BC Mode
    // [SerializeField] allows this private field to be visible and editable in the Unity Inspector
    [SerializeField] private Toggle toggle;

    // Called when the script instance is being loaded
    void Start()
    {
        // Check if the toggle reference is not set in the Inspector
        if (toggle == null)
        {
            // Try to get the Toggle component from the same GameObject
            toggle = GetComponent<Toggle>();
            
            // If still null, log an error and exit
            if (toggle == null) 
            {
                Debug.LogError("Toggle component not found on " + gameObject.name);
                return;
            }
        }        

        // Load the saved state from PlayerPrefs
        // "BCMode" is the key used to store the setting
        // 0 is the default value if the key doesn't exist
        // Convert the integer (0 or 1) to a boolean (false or true)
        bool savedState = PlayerPrefs.GetInt("BCMode", 0) == 1;

        // Set the toggle's state to match the saved state
        // This is done without triggering the onValueChanged event
        toggle.isOn = savedState;

        // Add a listener to the toggle's onValueChanged event
        // This will call OnToggleValueChanged whenever the toggle state changes
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // This method is called whenever the toggle value changes
    // It saves the new state to PlayerPrefs so it persists between game sessions
    public void OnToggleValueChanged(bool isOn)
    {
        // Save the toggle state to PlayerPrefs
        // Convert the boolean to an integer (1 for true, 0 for false)
        PlayerPrefs.SetInt("BCMode", isOn ? 1 : 0);
        
        // Explicitly save the PlayerPrefs to ensure the value is written to disk
        PlayerPrefs.Save();
    }
}