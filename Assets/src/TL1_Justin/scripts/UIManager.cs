using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject healthBarPrefab;
    private Image healthBar;
    private PlayerTank player;
    private int lastActionPoints = -1;

    void Start()
    {
        // Instantiate the UI Canvas from the prefab
        GameObject UICanvasInstance = Instantiate(UICanvas, transform);
        
        // Instantiate the health bar UI from the prefab
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBar = healthBarInstance.transform.Find("TotalHealth").GetComponent<Image>();

        if (healthBar == null)
        {
            Debug.Log("Health bar Image component not found.");
            
            // Try to find any child with "Health" in the name
            for (int i = 0; i < healthBarInstance.transform.childCount; i++)
            {
                Transform child = healthBarInstance.transform.GetChild(i);
                Image childImage = child.GetComponent<Image>();
                if (childImage != null && child.name.Contains("Health"))
                {
                    healthBar = childImage;
                    Debug.Log("Using " + child.name + " as health bar");
                    break;
                }
            }
        }
        
        // Try to find player
        FindPlayerTank();
    }
    
    void FindPlayerTank()
    {
        // Try to find player by tag first
        GameObject playerObject = GameObject.FindWithTag("Player");
        
        if (playerObject == null)
        {
            // Fall back to finding by name
            playerObject = GameObject.Find("PlayerTank");
        }
        
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerTank>();
        }
    }

    void Update()
    {
        // If we don't have a player reference, try to find it again
        if (player == null)
        {
            FindPlayerTank();
            return;
        }

        // Get the current action points
        int playerActionPoints = player.GetActionPoints();
        
        // Track when action points change
        if (playerActionPoints != lastActionPoints)
        {
            lastActionPoints = playerActionPoints;
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
    }

    // Simple OnGUI method for action points display
    void OnGUI()
    {
        // Only draw if we have a player reference
        if (player != null)
        {
            int actionPoints = player.GetActionPoints();
            
            // Set up style for the label
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 24;
            style.normal.textColor = Color.yellow;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontStyle = FontStyle.Bold;
            
            // Create a background box in the top left corner
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Box(new Rect(10, 10, 240, 40), "");
            
            // Draw the text
            GUI.Label(new Rect(20, 10, 230, 40), "Action Points: " + actionPoints, style);
        }
    }
}
