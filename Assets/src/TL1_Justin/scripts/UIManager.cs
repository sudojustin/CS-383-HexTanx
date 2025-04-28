using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Add this for SceneManager

public class UIManager : MonoBehaviour
{
    public GameObject UICanvas;
    public Texture2D logoTexture;
    private PlayerTank player;
    private List<TankType> enemyTanks = new List<TankType>();
    private int lastActionPoints = -1;
    private int lastEnemyHealth = -1;
    private Texture2D uiBackgroundTexture;
    private Texture2D healthBarTexture;
    private Texture2D healthBarBackgroundTexture;
    private Texture2D tankIconTexture;
    
    // Turn notification variables
    private bool isPlayerTurn = true;
    private bool turnJustChanged = false;
    private float turnNotificationDuration = 2.5f;
    private float turnNotificationTimer = 0f;
    private string turnNotificationMessage = "";

    // Health bar visual properties
    private float healthBarWidth = 0.5f;    // World space width
    private float healthBarHeight = 0.1f;   // World space height
    private float healthBarYOffset = 0.5f;  // How high above the tank
    
    // Level indicator
    private string currentLevelName = "Level 1";

    void Start()
    {
        // Instantiate the UI Canvas from the prefab
        GameObject UICanvasInstance = Instantiate(UICanvas, transform);
        
        // Get current level name
        currentLevelName = SceneManager.GetActiveScene().name;
        
        // Format the level name for display
        FormatLevelName();
        
        // Create UI background texture
        uiBackgroundTexture = new Texture2D(1, 1);
        uiBackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f)); // Black color with 100% opacity
        uiBackgroundTexture.Apply();

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
    
    // Format the level name for display
    private void FormatLevelName()
    {
        // Handle special level names
        if (currentLevelName == "Level666")
        {
            currentLevelName = "Level 666";
        }
        else if (currentLevelName == "LevelEaster")
        {
            currentLevelName = "Easter Level";
        }
        else if (currentLevelName.StartsWith("Level"))
        {
            // For regular levels (Level1, Level2, etc.)
            string levelNumber = currentLevelName.Substring(5);
            currentLevelName = "Level " + levelNumber;
        }
        // If it's not a level scene, don't display anything
        else if (currentLevelName == "MainMenu" || 
                 currentLevelName == "LevelSelector" || 
                 currentLevelName == "HelpMenu" || 
                 currentLevelName == "GameOverWin" || 
                 currentLevelName == "GameOverLose")
        {
            currentLevelName = "";
        }
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
        
        // Clear the previous list
        enemyTanks.Clear();
        
        // Find all enemy tanks
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("EnemyTank");
        
        foreach (GameObject enemyObject in enemyObjects)
        {
            TankType enemyTank = enemyObject.GetComponent<TankType>();
            if (enemyTank != null)
            {
                enemyTanks.Add(enemyTank);
            }
        }
    }

    void Update()
    {
        // If we don't have references, try to find them again
        if (player == null || enemyTanks.Count == 0)
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
        
        // No need to track a single enemy health since we have multiple
        
        CheckTurnState();
        
        if (turnJustChanged)
        {
            turnNotificationTimer = turnNotificationDuration;
            turnJustChanged = false;
        }
        
        if (turnNotificationTimer > 0)
        {
            turnNotificationTimer -= Time.deltaTime;
        }
    }
    
    // Check if turn state has changed
    private void CheckTurnState()
    {
        bool currentIsPlayerTurn = player.GetActionPoints() > 0;
        
        if (currentIsPlayerTurn != isPlayerTurn)
        {
            isPlayerTurn = currentIsPlayerTurn;
            turnJustChanged = true;
            
            if (isPlayerTurn)
            {
                turnNotificationMessage = "PLAYER TURN";
            }
            else
            {
                turnNotificationMessage = "ENEMY TURN";
            }
        }
    }

    // UI display for both player action points and enemy health
    void OnGUI()
    {
        // Create style for UI elements
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white; // White text color
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
        
        // Level indicator in top right (only show if we have a level name)
        if (!string.IsNullOrEmpty(currentLevelName))
        {
            GUIStyle levelStyle = new GUIStyle(style);
            levelStyle.alignment = TextAnchor.MiddleRight;
            levelStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width - 250, 10, 230, 40), currentLevelName, levelStyle);
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
        
        DrawEnemyHealthBar();
        DrawTurnNotificationBanner();
    }
    
    void DrawEnemyHealthBar()
    {
        // Draw health bars for all enemy tanks
        foreach (TankType enemyTank in enemyTanks)
        {
            if (enemyTank == null) continue;
            
            Vector3 enemyPosition = enemyTank.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemyPosition);
            
            float maxHealth = 100f;
            string tankTypeName = enemyTank.GetType().Name;
            
            if (tankTypeName == "Level1Tank") maxHealth = 50f;
            else if (tankTypeName == "Level2Tank") maxHealth = 100f;
            else if (tankTypeName == "Level3Tank") maxHealth = 150f; 
            else if (tankTypeName == "Level4Tank") maxHealth = 300f;
            else if (tankTypeName == "Level5Tank") maxHealth = 100f;
            else if (tankTypeName == "Level6Tank") maxHealth = 125f;
            else if (tankTypeName == "Level7Tank") maxHealth = 150f;
            else if (tankTypeName == "Level8Tank") maxHealth = 300f;
            else if (tankTypeName == "Level9Tank1") maxHealth = 225f;
            else if (tankTypeName == "Level10Tank1") maxHealth = 100f;
            else if (tankTypeName == "Level666Tank") maxHealth = 666f;
            else if (tankTypeName == "LevelEasterTank") maxHealth = 50f;
            
            // Calculate health percentage based on actual max health
            float healthPercent = Mathf.Clamp01((float)enemyTank.health / maxHealth);
            
            // Health bar position (above the tank)
            float barWidth = 60f; // Screen pixels
            float barHeight = 8f; // Screen pixels
            float borderSize = 1f; // Border size in pixels
            float padding = 4f;  // Space between health bar and icon
            
            if (screenPos.z > 0 && 
                screenPos.x > 0 && screenPos.x < Screen.width &&
                screenPos.y > 0 && screenPos.y < Screen.height)
            {
                bool shouldFlash = healthPercent <= 0.3f;
                bool flashOn = !shouldFlash || (shouldFlash && Mathf.PingPong(Time.time * 2.5f, 1f) > 0.5f);
                
                if (flashOn)
                {
                    GUI.color = Color.black;
                    GUI.DrawTexture(
                        new Rect(screenPos.x - barWidth/2 - borderSize, 
                                Screen.height - screenPos.y - barHeight - 25 - borderSize, 
                                barWidth + borderSize*2, 
                                barHeight + borderSize*2),
                        healthBarBackgroundTexture);
                    
                    GUI.color = new Color(0.2f, 0.2f, 0.2f);
                    GUI.DrawTexture(
                        new Rect(screenPos.x - barWidth/2, 
                                Screen.height - screenPos.y - barHeight - 25, 
                                barWidth, 
                                barHeight),
                        healthBarBackgroundTexture);

                    Color healthColor;
                    if (healthPercent > 0.6f)
                        healthColor = new Color(0.8f, 0.4f, 0.2f);  // Rusty orange (high health)
                    else if (healthPercent > 0.3f)
                        healthColor = new Color(0.7f, 0.3f, 0.1f);  // Darker rust (medium health)
                    else
                        healthColor = new Color(0.6f, 0.1f, 0.1f);  // Dark red rust (low health)
                    
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
    
    void DrawTurnNotificationBanner()
    {
        if (turnNotificationTimer <= 0) return;
        
        float bannerHeight = 60f;
        float bannerAlpha = Mathf.Min(1f, turnNotificationTimer / 0.5f);
        if (turnNotificationTimer < 0.5f)
        {
            bannerAlpha = turnNotificationTimer / 0.5f;
        }
        
        float bannerY = 0f;
        if (turnNotificationTimer > (turnNotificationDuration - 0.5f))
        {
            float slideProgress = (turnNotificationDuration - turnNotificationTimer) / 0.5f;
            bannerY = Mathf.Lerp(-bannerHeight, 0f, slideProgress);
        }
        else if (turnNotificationTimer < 0.5f)
        {
            float slideProgress = turnNotificationTimer / 0.5f;
            bannerY = Mathf.Lerp(0f, -bannerHeight, 1f - slideProgress);
        }
        
        GUIStyle bannerBoxStyle = new GUIStyle(GUI.skin.box);
        bannerBoxStyle.normal.background = uiBackgroundTexture;
        
        GUIStyle bannerTextStyle = new GUIStyle(GUI.skin.label);
        bannerTextStyle.fontSize = 32;
        bannerTextStyle.alignment = TextAnchor.MiddleCenter;
        bannerTextStyle.fontStyle = FontStyle.Bold;
        
        if (isPlayerTurn)
        {
            // Rusty orange for player turn
            bannerTextStyle.normal.textColor = new Color(0.8f, 0.4f, 0.2f, bannerAlpha);
        }
        else
        {
            // Dark red rust for enemy turn
            bannerTextStyle.normal.textColor = new Color(0.6f, 0.1f, 0.1f, bannerAlpha);
        }
        
        Color originalColor = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, bannerAlpha * 0.8f);
        GUI.Box(new Rect(0, bannerY, Screen.width, bannerHeight), "", bannerBoxStyle);
        GUI.color = originalColor;
        GUI.Label(new Rect(0, bannerY, Screen.width, bannerHeight), turnNotificationMessage, bannerTextStyle);
    }
}
