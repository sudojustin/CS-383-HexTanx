using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlaceTile placeTileScript;
    // private ItemManager itemManager;
    private static ItemManager instance;
    public GameObject healthPack;
    public List<GameObject> spawnedItems = new List<GameObject>();

    private bool isSpawningItem = false; // Prevent multiple coroutines
 
    // playerTank.gameObject.tag = "PlayerTank";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckItemPickup();
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Duplicate ItemManager found. Destroying extra instance");
            // Destroy(gameObject);
            return;
        }
        instance = this;

        Debug.Log("Awake() called. Instance ID: " + gameObject.GetInstanceID());
        placeTileScript = FindFirstObjectByType<PlaceTile>();

        if (placeTileScript != null)
        {
            Debug.Log("PlaceTile script found in awake");
            StartCoroutine(WaitForGridAndSpawnHealthPack());
        }
    }

    private void CheckItemPickup()
    {
        PlayerTank playerTank = FindObjectOfType<PlayerTank>();
        if (playerTank == null) return;
        
        Vector3 playerPosition = playerTank.transform.position;
        
        // Check each item
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            if (spawnedItems[i] == null) 
            {
                spawnedItems.RemoveAt(i);
                continue;
            }
            
            Vector3 itemPosition = spawnedItems[i].transform.position;
            
            // Check if they're on the same tile using a small threshold
            // Only compare X and Y coordinates, ignoring Z
            float distanceXY = Vector2.Distance(
                new Vector2(playerPosition.x, playerPosition.y),
                new Vector2(itemPosition.x, itemPosition.y)
            );
            
            if (distanceXY < 0.5f) // Adjust threshold as needed
            {
                GameObject currentItem = spawnedItems[i];
                string itemType = currentItem.name;
                
                Debug.Log($"Player picked up item: {itemType}");
                
                // Apply effects based on item type
                if (itemType.Contains("HealthPack"))
                {
                    Debug.Log("Player health before health pack: " + playerTank.GetHealth());
                    playerTank.SetHealth(100);
                    Debug.Log("Player health after health pack: " + playerTank.GetHealth());
                }
                // Add more item types here as needed
                // else if (itemType.Contains("PowerUp")) { ... }
                // else if (itemType.Contains("Ammo")) { ... }
                
                // Destroy and remove from list
                Destroy(currentItem);
                spawnedItems.RemoveAt(i);
            }
        }
    }

    public IEnumerator WaitForGridAndSpawnHealthPack()
    {
        if (isSpawningItem) yield break; // Stop if already running
        isSpawningItem = true;           // Mark as running

        while (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0)
        {
            Debug.Log("Waiting for grid to be populated...");
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }

        Debug.Log("Grid is finally populated");

        int width = placeTileScript.width;
        int height = placeTileScript.height;

        Debug.Log("Grid width is: " + width);
        Debug.Log("Grid height is: " + height);

        int randX = Random.Range(0, width);
        int randY = Random.Range(0, height);
        Debug.Log($"Random health pack locations are {randX} and {randY}");
        Vector3 healthPackPos = placeTileScript.Grid[randX, randY];
        healthPackPos.z = -1f; // Ensure health pack appears above the tiles

        Debug.Log("Spawning health pack at: " + healthPackPos);

        Debug.Log("HealthPack reference: " + (healthPack != null ? "Assigned" : "NULL"));

        GameObject spawnedHealthPack = Instantiate(healthPack, healthPackPos, Quaternion.identity);
        Debug.Log("Health pack instantiated at: " + spawnedHealthPack.transform.position);

        spawnedItems.Add(spawnedHealthPack);

        isSpawningItem = false; // Reset flag after completion
    }
}
