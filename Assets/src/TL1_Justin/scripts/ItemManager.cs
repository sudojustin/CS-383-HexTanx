using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlaceTile placeTileScript;
    // private ItemManager itemManager;
    private static ItemManager instance;
    public GameObject healthPack;
    public GameObject flag;
    public GameObject armor;
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
            return;
        }
        instance = this;

        Debug.Log("Awake() called. Instance ID: " + gameObject.GetInstanceID());
        placeTileScript = FindFirstObjectByType<PlaceTile>();

        if (placeTileScript != null)
        {
            Debug.Log("PlaceTile script found in awake");
            StartCoroutine(WaitForGridAndSpawnItem(healthPack));
            StartCoroutine(WaitForGridAndSpawnItem(flag));
            StartCoroutine(WaitForGridAndSpawnItem(armor));
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
                else if (itemType.Contains("Flag"))
                {
                    Debug.Log("Player picked up the flag!");
                    
                    // Find the BattleSystem and call GameWon
                    BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
                    if (battleSystem != null)
                    {
                        Debug.Log("BattleSystem found, calling GameWon");
                        battleSystem.SendMessage("GameWon");
                    }
                    else
                    {
                        Debug.LogError("BattleSystem not found, cannot trigger game win condition");
                    }
                }
                else if (itemType.Contains("Armor"))
                {
                    Debug.Log("Player picked up armor!");
                    // Armor effect will be implemented later
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

    public IEnumerator WaitForGridAndSpawnItem(GameObject itemPrefab)
    {
        // Wait for any other item spawning to complete
        while (isSpawningItem)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isSpawningItem = true;           // Mark as running

        while (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0)
        {
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }

        int width = placeTileScript.width;
        int height = placeTileScript.height;

        int randX = Random.Range(0, width);
        int randY = Random.Range(0, height);
        Vector3 itemPos = placeTileScript.Grid[randX, randY];
        itemPos.z = -1f; // Ensure item appears above the tiles

        GameObject spawnedItem = Instantiate(itemPrefab, itemPos, Quaternion.identity);
        spawnedItems.Add(spawnedItem);

        isSpawningItem = false; // Reset flag after completion
    }
}
