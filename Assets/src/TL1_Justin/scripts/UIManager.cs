using UnityEngine;
using UnityEngine.UI;
// TankType is defined in Assets/src/TL3_Brenden/Scripts (Future)/TankType.cs without a namespace

public class UIManager : MonoBehaviour
{
    public GameObject UICanvas;
    public Texture2D logoTexture; // Add public reference for the logo
    private PlayerTank player;
    private TankType enemyTank;
    private int lastActionPoints = -1;
    private int lastEnemyHealth = -1;
    private Texture2D uiBackgroundTexture;
    private Texture2D healthBarTexture;
    private Texture2D healthBarBackgroundTexture;
    private Texture2D tankIconTexture;

    // Health bar visual properties
    private float healthBarWidth = 0.5f;    // World space width
    private float healthBarHeight = 0.1f;   // World space height
    private float healthBarYOffset = 0.5f;  // How high above the tank

    void Start()
    {
        // Instantiate the UI Canvas from the prefab
        GameObject UICanvasInstance = Instantiate(UICanvas, transform);
        
        // Create solid background texture
        uiBackgroundTexture = new Texture2D(1, 1);
        uiBackgroundTexture.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.27f, 1f)); // Darker gunmetal color
        uiBackgroundTexture.Apply();
        
        // Create health bar textures
        healthBarTexture = new Texture2D(1, 1);
        healthBarTexture.SetPixel(0, 0, Color.red); // Red health bar
        healthBarTexture.Apply();
        
        healthBarBackgroundTexture = new Texture2D(1, 1);
        healthBarBackgroundTexture.SetPixel(0, 0, Color.black); // Black background
        healthBarBackgroundTexture.Apply();
        
        // Create tank icon texture
        tankIconTexture = new Texture2D(1, 1);
        tankIconTexture.SetPixel(0, 0, new Color(0.5f, 0.5f, 0.5f)); // Gray tank icon
        tankIconTexture.Apply();
        
        // Try to find player and enemy
        FindTanks();
    }
    
    void FindTanks()
    {
        // Find player tank
        GameObject playerObject = GameObject.FindWithTag("Player");
        
        if (playerObject == null)
        {
            playerObject = GameObject.Find("PlayerTank");
        }
        
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerTank>();
        }
        
        // Find enemy tank
        GameObject enemyObject = GameObject.FindWithTag("EnemyTank");
        
        if (enemyObject != null)
        {
            enemyTank = enemyObject.GetComponent<TankType>();
        }
    }

    void Update()
    {
        // If we don't have references, try to find them again
        if (player == null || enemyTank == null)
        {
            FindTanks();
            return;
        }

        // Get the current action points for player
        int playerActionPoints = player.GetActionPoints();
        
        // Track when action points change
        if (playerActionPoints != lastActionPoints)
        {
            lastActionPoints = playerActionPoints;
        }
        
        // Track enemy health changes
        if (enemyTank != null && enemyTank.health != lastEnemyHealth)
        {
            lastEnemyHealth = enemyTank.health;
        }
    }

    // UI display for both player action points and enemy health
    void OnGUI()
    {
        // Create style for UI elements
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = new Color(0.8f, 0.4f, 0.2f); // Rusty orange color
        style.alignment = TextAnchor.MiddleLeft;
        style.fontStyle = FontStyle.Bold;
        
        // Create box style with solid background
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = uiBackgroundTexture;
        
        // Player health in top left (above action points)
        if (player != null)
        {
            int playerHealth = player.GetHealth();
            // Draw solid background box for player health
            GUI.Box(new Rect(10, 10, 240, 40), "", boxStyle);
            // Draw health text
            GUI.Label(new Rect(20, 10, 230, 40), "Health: " + playerHealth, style);
            
            // Draw health bar
            float healthPercent = (float)playerHealth / 100f;
            
            // Save original GUI color to restore after drawing health bar
            Color originalColor = GUI.color;
            
            // Draw background (gray)
            GUI.color = Color.gray;
            GUI.DrawTexture(new Rect(110, 20, 130, 20), Texture2D.whiteTexture);
            
            // Calculate health color - green when full, yellow at half, red when empty
            if (healthPercent > 0.6f)
                GUI.color = Color.green;  // High health (above 60%)
            else if (healthPercent > 0.3f)
                GUI.color = Color.yellow; // Medium health (30-60%)
            else
                GUI.color = Color.red;    // Low health (below 30%)
                
            // Draw foreground (colored by health)
            GUI.DrawTexture(new Rect(110, 20, 130 * healthPercent, 20), Texture2D.whiteTexture);
            
            // Restore original color
            GUI.color = originalColor;
        }
        
        // Player action points below player health
        if (player != null)
        {
            int actionPoints = player.GetActionPoints();
            GUI.Box(new Rect(10, 60, 240, 40), "", boxStyle);
            GUI.Label(new Rect(20, 60, 230, 40), "Action Points: " + actionPoints, style);
        }
        
        // Draw logo in the bottom left corner
        if (logoTexture != null)
        {
            float logoWidth = 180f;
            float logoHeight = 120f;
            float padding = 15f;
            GUI.DrawTexture(
                new Rect(padding, Screen.height - logoHeight - padding, logoWidth, logoHeight),
                logoTexture,
                ScaleMode.ScaleToFit
            );
        }
        
        // Draw enemy health bar above the enemy tank
        DrawEnemyHealthBar();
    }
    
    // Draw health bar directly above enemy tank in world space
    void DrawEnemyHealthBar()
    {
        if (enemyTank == null) return;
        
        // Get the enemy's position in world space
        Vector3 enemyPosition = enemyTank.transform.position;
        
        // Convert enemy position to screen space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemyPosition);
        
        // Determine max health based on enemy tank's type name
        float maxHealth = 100f;
        string tankTypeName = enemyTank.GetType().Name;
        
        // TankType hierarchy from codebase search results
        if (tankTypeName == "Level1Tank") maxHealth = 50f;
        else if (tankTypeName == "Level2Tank") maxHealth = 75f;
        else if (tankTypeName == "Level3Tank") maxHealth = 100f; 
        else if (tankTypeName == "Level4Tank") maxHealth = 125f;
        
        // Calculate health percentage based on actual max health
        float healthPercent = Mathf.Clamp01((float)enemyTank.health / maxHealth);
        
        // Health bar position (above the tank)
        float barWidth = 60f; // Screen pixels
        float barHeight = 8f; // Screen pixels
        float borderSize = 1f; // Border size in pixels
        float padding = 4f;  // Space between health bar and icon
        
        // Ensure the health bar is visible when the enemy is on screen
        if (screenPos.z > 0 && 
            screenPos.x > 0 && screenPos.x < Screen.width &&
            screenPos.y > 0 && screenPos.y < Screen.height)
        {
            // Determine if we should flash the health bar (when health is low)
            bool shouldFlash = healthPercent <= 0.3f;
            bool flashOn = !shouldFlash || (shouldFlash && Mathf.PingPong(Time.time * 2.5f, 1f) > 0.5f);
            
            // Apply flashing effect - hide the bar entirely when flashing off
            if (flashOn)
            {
                // Draw black border
                GUI.color = Color.black;
                GUI.DrawTexture(
                    new Rect(screenPos.x - barWidth/2 - borderSize, 
                            Screen.height - screenPos.y - barHeight - 25 - borderSize, 
                            barWidth + borderSize*2, 
                            barHeight + borderSize*2),
                    healthBarBackgroundTexture);
                
                // Draw health bar background (dark gray)
                GUI.color = new Color(0.2f, 0.2f, 0.2f);
                GUI.DrawTexture(
                    new Rect(screenPos.x - barWidth/2, 
                            Screen.height - screenPos.y - barHeight - 25, 
                            barWidth, 
                            barHeight),
                    healthBarBackgroundTexture);
                
                // Calculate health color - rusty orange-red theme
                Color healthColor;
                if (healthPercent > 0.6f)
                    healthColor = new Color(0.8f, 0.4f, 0.2f);  // Rusty orange (high health)
                else if (healthPercent > 0.3f)
                    healthColor = new Color(0.7f, 0.3f, 0.1f);  // Darker rust (medium health)
                else
                    healthColor = new Color(0.6f, 0.1f, 0.1f);  // Dark red rust (low health)
                
                // Draw health bar foreground (colored by health)
                GUI.color = healthColor;
                GUI.DrawTexture(
                    new Rect(screenPos.x - barWidth/2, 
                            Screen.height - screenPos.y - barHeight - 25,
                            barWidth * healthPercent, 
                            barHeight),
                    healthBarTexture);
            }
        }
    }
}
