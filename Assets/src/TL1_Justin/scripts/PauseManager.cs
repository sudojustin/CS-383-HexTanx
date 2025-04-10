using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenuCanvas;
    public static bool isPaused;
    private GameObject pauseMenuInstance;
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;
    private bool rightClickBlocked = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuInstance == null)
        {
            pauseMenuInstance = Instantiate(PauseMenuCanvas);
            pauseMenuInstance.SetActive(false);
        }

        StartCoroutine(FindPlayerTank()); // Use coroutine to wait until PlayerTank is instantiated

        // Find buttons and assign onClick listeners
        Button resumeButton = pauseMenuInstance.transform.Find("PausePanel/ResumeButton").GetComponent<Button>();
        Button mainMenuButton = pauseMenuInstance.transform.Find("PausePanel/MainMenuButton").GetComponent<Button>();
        Button quitButton = pauseMenuInstance.transform.Find("PausePanel/QuitButton").GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        // Block any mouse inputs during pause except for UI clicks
        if (isPaused)
        {
            // Completely disable right-click (shooting) while paused
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Blocking right-click during pause");
                rightClickBlocked = true;
                return;
            }
            
            // Also check for held right-click to prevent shooting
            if (Input.GetMouseButton(1))
            {
                rightClickBlocked = true;
                return;
            }
        }
        else
        {
            // Prevent the queued right-click from firing when unpausing
            if (rightClickBlocked && Input.GetMouseButton(1))
            {
                // Wait until player releases right click before allowing shooting again
                return;
            }
            else
            {
                rightClickBlocked = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Static method to check if game is paused - can be called from any script
    public static bool IsGamePaused()
    {
        return isPaused;
    }

    public void ResumeGame()
    {
        Debug.Log("Resume button pressed");

        // Clean up any projectiles that might have been queued while paused
        CleanupProjectiles();

        pauseMenuInstance.SetActive(false);
        isPaused = false;

        if (playerMovement != null)
        {
            Debug.Log("Player movement is enabled");
            playerMovement.enabled = true;
        }

        if (playerShooting != null)
        {
            Debug.Log("Player shooting is enabled");
            playerShooting.enabled = true;
        }

        // Resume battle music
        if (SoundManager.GetInstance() != null)
        {
            if (SceneManager.GetActiveScene().name == "Level4")
            {
                SoundManager.GetInstance().finalBattleMusic();
            }
            else
            {
                SoundManager.GetInstance().BattleMusic();
            }
        }
        else
        {
            Debug.LogError("SoundManager.Instance is null in ResumeGame!");
        }
        
        // Resume time scale AFTER player components are enabled to prevent queued actions
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        Debug.Log("Pause button pressed");
        // Pause time scale FIRST to prevent any additional shots during pause
        Time.timeScale = 0f;
        pauseMenuInstance.SetActive(true);
        isPaused = true;

        if (playerMovement != null)
        {
            Debug.Log("Player movement is disabled");
            playerMovement.enabled = false;
        }
        
        if (playerShooting != null)
        {
            Debug.Log("Player shooting is disabled");
            playerShooting.enabled = false;
            
            // Completely block shooting while paused
            rightClickBlocked = true;
        }
        
        // Play pause music
        if (SoundManager.GetInstance() != null)
        {
            SoundManager.GetInstance().PauseMusic();
        }
        else
        {
            Debug.LogError("SoundManager.Instance is null in PauseGame!");
        }
    }

    // Method to find and clean up any projectiles that might have been created during pause
    private void CleanupProjectiles()
    {
        // Find all MonoBehaviours in the scene
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        
        // Manually check for projectile scripts by name
        foreach (MonoBehaviour mb in allComponents)
        {
            string scriptName = mb.GetType().Name;
            if (scriptName.Contains("Projectile") || scriptName.Equals("Projectile"))
            {
                Debug.Log("Cleaning up projectile: " + mb.gameObject.name);
                Destroy(mb.gameObject);
            }
        }
        
        // Also check for objects with "Projectile" in their name
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Projectile") || obj.name.Equals("Projectile"))
            {
                Debug.Log("Cleaning up named projectile: " + obj.name);
                Destroy(obj);
            }
        }
        
        // Fallback to explicit GameObject.Find for "Projectile"
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile != null)
        {
            Debug.Log("Cleaning up found projectile: " + projectile.name);
            Destroy(projectile);
        }
        
        // Also look for projectile(Clone)
        GameObject clonedProjectile = GameObject.Find("Projectile(Clone)");
        if (clonedProjectile != null)
        {
            Debug.Log("Cleaning up cloned projectile: " + clonedProjectile.name);
            Destroy(clonedProjectile);
        }
    }

    public void MainMenu()
    {
        Debug.Log("Main menu button pressed");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameLevel1");
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/src/TL1_Justin/scenes/MainMenu.unity");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }

    // Coroutine to wait for PlayerTank to be instantiated
    IEnumerator FindPlayerTank()
    {
        // Wait until the PlayerTank is available
        while (GameObject.Find("PlayerTank") == null)
        {
            yield return null;
        }

        // Once the PlayerTank is found, get the PlayerMovement component
        GameObject playerTank = GameObject.Find("PlayerTank");
        if (playerTank != null)
        {
            playerMovement = playerTank.GetComponent<PlayerMovement>();
            playerShooting = playerTank.GetComponent<PlayerShooting>();
            Debug.Log("PlayerTank found and PlayerMovement initialized.");
        }
        else
        {
            Debug.LogError("PlayerTank not found after waiting.");
        }
    }
}
