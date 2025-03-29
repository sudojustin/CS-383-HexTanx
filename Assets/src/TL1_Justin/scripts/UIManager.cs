using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject healthBarPrefab;
    private Text UIActionPoints;
    // Use Component type instead of direct TMPro reference to avoid compile errors
    private Component tmpTextComponent; 
    private bool usingTMPro = false; // Flag to track which text component we're using
    private Image healthBar; // The part of the health bar we want to adjust
    private PlayerTank player;
    private int lastActionPoints = -1; // Track last action points to detect changes
    
    // New direct canvas reference
    private Canvas directCanvas;
    private Text directActionPointsText;

    private PlayerTankSpawner playerTankSpawner;

    void Start()
    {
        playerTankSpawner = FindObjectOfType<PlayerTankSpawner>();

        if (playerTankSpawner == null)
        {
            Debug.Log("PlayerTankSpawner not found in the scene.");
            return;
        }

        // Instantiate the UI Canvas from the prefab
        GameObject UICanvasInstance = Instantiate(UICanvas, transform);
        
        // Ensure it has a Canvas component
        Canvas canvas = UICanvasInstance.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = UICanvasInstance.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            UICanvasInstance.AddComponent<CanvasScaler>();
            UICanvasInstance.AddComponent<GraphicRaycaster>();
            Debug.Log("Added Canvas components to UICanvas");
        }
        
        Debug.Log("UI Canvas instantiated: " + (UICanvasInstance != null));
        
        // Check if the ActionPointText exists in the hierarchy
        Transform actionPointTextTransform = UICanvasInstance.transform.Find("ActionPointText");
        Debug.Log("ActionPointText found in hierarchy: " + (actionPointTextTransform != null));
        
        if (actionPointTextTransform != null)
        {
            // Try to get Text component first
            UIActionPoints = actionPointTextTransform.GetComponent<Text>();
            Debug.Log("Text component found on ActionPointText: " + (UIActionPoints != null));
            
            // If Text component not found, try TextMeshProUGUI
            if (UIActionPoints == null)
            {
                try
                {
                    // Try to dynamically get TextMeshProUGUI to avoid compile errors if the assembly is missing
                    System.Type tmpType = System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro");
                    if (tmpType != null)
                    {
                        tmpTextComponent = actionPointTextTransform.GetComponent(tmpType);
                        if (tmpTextComponent != null)
                        {
                            Debug.Log("TextMeshProUGUI component found on ActionPointText");
                            usingTMPro = true;
                            
                            // Disable the original TextMeshProUGUI object so it doesn't show
                            actionPointTextTransform.gameObject.SetActive(false);
                            Debug.Log("Disabled original ActionPointText object");
                            
                            // Create a GameObject with our own Text component
                            GameObject textObject = new GameObject("ActionPointTextRegular");
                            textObject.transform.SetParent(UICanvasInstance.transform, false);
                            UIActionPoints = textObject.AddComponent<Text>();
                            
                            // Set up the text component properties
                            UIActionPoints.fontSize = 36; // Larger font size
                            UIActionPoints.color = Color.yellow; // More visible color
                            UIActionPoints.alignment = TextAnchor.UpperCenter;
                            
                            // Make sure we have a font
                            Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                            if (arialFont != null)
                            {
                                UIActionPoints.font = arialFont;
                            }
                            
                            // Position it in the canvas (adjust these values as needed)
                            RectTransform rectTransform = UIActionPoints.GetComponent<RectTransform>();
                            rectTransform.anchorMin = new Vector2(0, 1);
                            rectTransform.anchorMax = new Vector2(1, 1);
                            rectTransform.pivot = new Vector2(0.5f, 1f);
                            rectTransform.anchoredPosition = new Vector2(0, -50);
                            rectTransform.sizeDelta = new Vector2(0, 80);
                            
                            // Set initial text
                            UIActionPoints.text = "Action Points: 0";
                            Debug.Log("Created our own Text component with initial text: " + UIActionPoints.text);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error trying to access TextMeshProUGUI: " + e.Message);
                }
            }
            else
            {
                // Set initial text if we found the Text component
                UIActionPoints.text = "Action Points: 0";
                Debug.Log("Initial text set to: " + UIActionPoints.text);
            }
        }

        if (UIActionPoints == null)
        {
            Debug.Log("ACTION POINTS TEXT COMPONENT NOT FOUND - Creating our own");
            
            // Create our own Text object since we couldn't find or use the existing one
            GameObject textObject = new GameObject("ActionPointText");
            textObject.transform.SetParent(UICanvasInstance.transform, false);
            UIActionPoints = textObject.AddComponent<Text>();
            
            // Set up the text component
            UIActionPoints.fontSize = 36; // Larger font size
            UIActionPoints.color = Color.yellow; // More visible color
            UIActionPoints.alignment = TextAnchor.UpperCenter;
            
            // Make sure we have a font
            Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            if (arialFont != null)
            {
                UIActionPoints.font = arialFont;
            }
            
            // Position it in the canvas - adjust to match your existing text
            RectTransform rectTransform = UIActionPoints.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -50);
            rectTransform.sizeDelta = new Vector2(0, 80);
            
            // Set initial text
            UIActionPoints.text = "Action Points: 0";
            Debug.Log("Created custom Text component with improved visibility");
        }
        else
        {
            Debug.Log("ACTION POINTS TEXT COMPONENT FOUND");
        }

        // Instantiate the health bar UI from the prefab
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBar = healthBarInstance.transform.Find("TotalHealth").GetComponent<Image>();

        if (healthBar == null)
        {
            Debug.Log("Health bar Image component not found.");
            Debug.Log("Looking for TotalHealth in: " + healthBarInstance.name);
            
            // Debug all children of the health bar instance
            for (int i = 0; i < healthBarInstance.transform.childCount; i++)
            {
                Transform child = healthBarInstance.transform.GetChild(i);
                Debug.Log("Health bar child " + i + ": " + child.name);
                
                // Check if this child has an Image component
                Image childImage = child.GetComponent<Image>();
                if (childImage != null)
                {
                    Debug.Log("Found Image component on: " + child.name);
                    if (child.name.Contains("Health"))
                    {
                        healthBar = childImage;
                        Debug.Log("Using " + child.name + " as health bar");
                        break;
                    }
                }
            }
            
            if (healthBar == null)
            {
                Debug.LogError("Could not find any suitable health bar component");
                // Don't return, we still want action points to work
            }
        }
        else
        {
            Debug.Log("Health bar Image component found successfully");
        }
        
        // Try to find player at start
        FindPlayerTank();
    }
    
    void CreateDirectCanvas()
    {
        // Create a completely new GameObject for the canvas
        GameObject canvasObject = new GameObject("DirectCanvas");
        canvasObject.transform.SetParent(transform, false);
        
        // Add canvas components
        directCanvas = canvasObject.AddComponent<Canvas>();
        directCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        directCanvas.sortingOrder = 100; // Make sure it's on top of everything
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        
        // Create a panel background
        GameObject panelObject = new GameObject("ActionPointsPanel");
        panelObject.transform.SetParent(canvasObject.transform, false);
        
        // Add image component as background
        Image panelImage = panelObject.AddComponent<Image>();
        // Use a white sprite for the background (will be tinted by color)
        panelImage.color = new Color(0f, 0f, 0f, 0.7f); // Semi-transparent black
        
        // Create a 1x1 white texture for the background
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        panelImage.sprite = sprite;
        
        // Position the panel
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(400, 100);
        
        // Create text object as a child of the panel
        GameObject textObject = new GameObject("DirectActionPointsText");
        textObject.transform.SetParent(panelObject.transform, false);
        
        // Add and configure text component
        directActionPointsText = textObject.AddComponent<Text>();
        
        // Try to get a system font first
        Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        if (arialFont == null)
        {
            // Try other common fonts if Arial isn't available
            arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "LiberationSans.ttf");
        }
        
        if (arialFont != null)
        {
            directActionPointsText.font = arialFont;
            Debug.Log("Font assigned to text: " + arialFont.name);
        }
        else
        {
            Debug.LogError("Could not find a valid font for UI text!");
            
            // Try to find ANY font in the project
            Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
            if (allFonts.Length > 0)
            {
                directActionPointsText.font = allFonts[0];
                Debug.Log("Using fallback font: " + allFonts[0].name);
            }
        }
        
        directActionPointsText.fontSize = 48; // Extra large
        directActionPointsText.color = Color.red; // Bright red
        directActionPointsText.alignment = TextAnchor.MiddleCenter; // Center of screen
        directActionPointsText.horizontalOverflow = HorizontalWrapMode.Overflow;
        directActionPointsText.verticalOverflow = VerticalWrapMode.Overflow;
        
        // Position text to fill the panel
        RectTransform rectTransform = directActionPointsText.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        
        // Set initial text
        directActionPointsText.text = "ACTION POINTS: 0";
        
        Debug.Log("Created direct Canvas with centered text and background panel");
    }
    
    void FindPlayerTank()
    {
        // Try to find player by tag first (more reliable)
        GameObject playerObject = GameObject.FindWithTag("Player");
        
        if (playerObject == null)
        {
            // Fall back to finding by name
            playerObject = GameObject.Find("PlayerTank");
        }
        
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerTank>();
            if (player != null)
            {
                Debug.Log("Found PlayerTank component successfully");
            }
        }
    }

    void Update()
    {
        // If we don't have a player reference or lost it, try to find it again
        if (player == null)
        {
            FindPlayerTank();
            
            if (player == null)
            {
                Debug.Log("Could not find PlayerTank, will try again next frame");
                return;
            }
        }

        // Get the current action points
        int playerActionPoints = player.GetActionPoints();
        
        // Just log when action points change
        if (playerActionPoints != lastActionPoints)
        {
            lastActionPoints = playerActionPoints;
            Debug.Log("Action points changed to: " + playerActionPoints);
        }
        
        // Update health bar
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (player != null && healthBar != null)
        {
            // Get the player's health as a percentage
            float healthPercent = player.GetHealth() / 100f;

            // Update the health bar's fill amount based on the player's health
            healthBar.fillAmount = healthPercent;
        }
        else if (player != null && healthBar == null)
        {
            Debug.LogWarning("Cannot update health bar - healthBar reference is null");
        }
    }

    // Add a direct GUI method as a fallback
    void OnGUI()
    {
        // Only draw if we have a player reference
        if (player != null)
        {
            int actionPoints = player.GetActionPoints();
            
            // Set up style for the label
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 24; // Smaller font size
            style.normal.textColor = Color.yellow; // Consistent yellow color
            style.alignment = TextAnchor.MiddleLeft; // Left-aligned text
            style.fontStyle = FontStyle.Bold;
            
            // Create a background box in the top left corner - wider to fit "Action Points"
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Box(new Rect(10, 10, 240, 40), ""); // Wider box positioned in the top left
            
            // Draw the text
            GUI.Label(new Rect(20, 10, 230, 40), "Action Points: " + actionPoints, style); // Back to full text
        }
    }
}
