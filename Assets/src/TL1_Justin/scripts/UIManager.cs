using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject UICanvas;
    private PlayerTank player;
    private TankType enemyTank;
    private int lastActionPoints = -1;
    private int lastEnemyHealth = -1;

    void Start()
    {
        // Instantiate the UI Canvas from the prefab
        GameObject UICanvasInstance = Instantiate(UICanvas, transform);
        
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
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleLeft;
        style.fontStyle = FontStyle.Bold;
        
        // Background color for UI elements
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        // Player health in top left (above action points)
        if (player != null)
        {
            int playerHealth = player.GetHealth();
            // Draw background box for player health
            GUI.Box(new Rect(10, 10, 240, 40), "");
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
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Box(new Rect(10, 60, 240, 40), "");
            GUI.Label(new Rect(20, 60, 230, 40), "Action Points: " + actionPoints, style);
        }
        
        // Enemy health in top right
        if (enemyTank != null)
        {
            int health = enemyTank.health;
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Box(new Rect(Screen.width - 220, 10, 210, 40), "");
            GUI.Label(new Rect(Screen.width - 210, 10, 200, 40), "Enemy HP: " + health, style);
        }
    }
}
