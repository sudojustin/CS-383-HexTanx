using UnityEngine;
using UnityEngine.UI;

// This attribute allows the script to run in the Unity Editor, not just during gameplay
// This is useful for previewing how the background will look without having to play the game
[ExecuteInEditMode]
public class BackgroundScalar : MonoBehaviour
{
    // Reference to the UI Image component that contains the background sprite
    Image backgroundImage;
    
    // Reference to the RectTransform component that controls the size and position of the UI element
    RectTransform rt;
    
    // Stores the aspect ratio of the background image (width divided by height)
    // This is used to maintain the image's proportions when scaling
    float ratio;

    void Start()
    {
        // Get the Image component attached to the same GameObject
        backgroundImage = GetComponent<Image>();
        
        // Get the RectTransform component from the Image
        rt = backgroundImage.rectTransform;
        
        // Calculate the aspect ratio of the background sprite
        // bounds.size.x is the width, bounds.size.y is the height
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    void Update()
    {
        // Safety check: if RectTransform is not available, exit the method
        if (!rt) 
            return;

        // Scale image proportionally to fit the screen dimensions, while preserving aspect ratio
        // There are two cases to consider:
        
        // Case 1: If the screen height multiplied by the image's aspect ratio is greater than or equal to the screen width
        // This means the image would be wider than the screen if scaled to match the screen height
        if (Screen.height * ratio >= Screen.width)
        {
            // Scale the image to match the screen height, and adjust the width to maintain aspect ratio
            // This ensures the image covers the entire screen height and extends beyond the screen width if needed
            rt.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        }
        // Case 2: If the screen width divided by the image's aspect ratio is greater than the screen height
        // This means the image would be taller than the screen if scaled to match the screen width
        else
        {
            // Scale the image to match the screen width, and adjust the height to maintain aspect ratio
            // This ensures the image covers the entire screen width and extends beyond the screen height if needed
            rt.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
        }
    }
}