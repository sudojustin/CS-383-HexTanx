using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DYNAMIC BINDING EXAMPLE
 * 
 * This code demonstrates dynamic binding (polymorphism) through the HealthPackEffect class hierarchy:
 * 
 * Super Class: HealthPackEffect
 * - Contains a virtual method GetHealAmount() that returns 25
 * 
 * Sub Class: BCHealthPackEffect
 * - Inherits from HealthPackEffect
 * - Overrides GetHealAmount() to return 100
 * 
 * Virtual Function: GetHealAmount()
 * 
 * Dynamic Binding Example:
 * 
 * // Static type is HealthPackEffect (the base class)
 * // Dynamic type is also HealthPackEffect
 * HealthPackEffect effect1 = new HealthPackEffect();
 * int healAmount1 = effect1.GetHealAmount(); // Returns 25
 * 
 * // Static type is HealthPackEffect (the base class)
 * // Dynamic type is BCHealthPackEffect (the derived class)
 * HealthPackEffect effect2 = new BCHealthPackEffect();
 * int healAmount2 = effect2.GetHealAmount(); // Returns 100
 * 
 * What method gets called now?
 * When you have HealthPackEffect effect2 = new BCHealthPackEffect();:
 * - The static type is HealthPackEffect (what the compiler sees)
 * - The dynamic type is BCHealthPackEffect (the actual object type at runtime)
 * - When effect2.GetHealAmount() is called, the BCHealthPackEffect version is called (returns 100)
 * - This is dynamic binding because the method call is resolved at runtime based on the actual object type
 * 
 * Change the dynamic type:
 * If you change the dynamic type to HealthPackEffect:
 * HealthPackEffect effect3 = new HealthPackEffect();
 * int healAmount3 = effect3.GetHealAmount(); // Returns 25
 * Now the HealthPackEffect version of GetHealAmount() is called (returns 25).
 * 
 * This is an example of polymorphism in object-oriented programming, where the same method call 
 * can behave differently based on the actual type of the object at runtime.
 */

// HealthPackEffect class that doesn't inherit from MonoBehaviour
// This class defines the base behavior for health pack healing effects
public class HealthPackEffect
{
    // Virtual method that will be overridden by derived classes
    // Returns the amount of health restored when a health pack is collected
    // This is a dynamically bound method - the actual implementation called
    // depends on the runtime type of the object, not the compile-time type
    public virtual int GetHealAmount()
    {
        return 25; // Default heal amount for normal mode
    }
}

// BC mode version with different healing amount
// This class extends HealthPackEffect to provide a different healing amount for "BC mode"
public class BCHealthPackEffect : HealthPackEffect
{
    // Override to provide different healing amount for BC mode
    // This overrides the virtual method from the parent class
    // When called on a BCHealthPackEffect object, this version will be used
    public override int GetHealAmount()
    {
        return 100; // BC mode heal amount
    }
}

/*
 * FACTORY PATTERN EXPLANATION
 * 
 * This is a Static Factory implementation.
 * It's a static class with a static method CreateEffect() that returns a HealthPackEffect.
 * It creates different types of health pack effects based "BC mode".
 * It follows the Factory pattern by encapsulating the creation logic for different effect types.
 * 
 * The main difference between this and ItemFactory is that this is a static factory
 * while ItemFactory is an instance-based factory (MonoBehaviour).
 */
public static class HealthPackEffectFactory
{
    // Creates and returns the appropriate HealthPackEffect based on game settings
    public static HealthPackEffect CreateEffect()
    {
        // Check if BC mode is enabled in PlayerPrefs
        // PlayerPrefs is Unity's way of storing persistent data between game sessions
        bool bcModeEnabled = PlayerPrefs.GetInt("BCMode", 0) == 1;
        
        // Return the appropriate effect based on BC mode setting
        if (bcModeEnabled)
        {
            // Creating a BCHealthPackEffect but returning it as a HealthPackEffect
            // This demonstrates polymorphism - the static type is HealthPackEffect
            // but the dynamic type is BCHealthPackEffect
            return new BCHealthPackEffect(); // Return BC mode effect with higher healing
        }
        else
        {
            return new HealthPackEffect(); // Return standard effect with normal healing
        }
    }
}

// Main class responsible for managing items in the game
// Handles spawning, tracking, and pickup detection for all game items
public class ItemManager : MonoBehaviour
{
    // Reference to the PlaceTile script that manages the game grid
    private PlaceTile placeTileScript;
    
    // Reference to the ItemFactory that creates item instances
    private ItemFactory itemFactory;
    
    // Singleton instance for global access
    private static ItemManager instance;
    
    // List to track all currently spawned items in the game
    public List<GameObject> spawnedItems = new List<GameObject>();

    // Flag to prevent multiple item spawning coroutines from running simultaneously
    private bool isSpawningItem = false;
 
    // Checks for item pickups by the player
    void Update()
    {
        CheckItemPickup();
    }

    // Awake is called when the script instance is being loaded
    // Sets up the singleton pattern and initializes references
    void Awake()
    {
        // Singleton pattern implementation
        // Ensures only one ItemManager exists in the game
        if (instance != null && instance != this)
        {
            return;
        }
        instance = this;

        // Find the PlaceTile script in the scene
        placeTileScript = FindFirstObjectByType<PlaceTile>();
        
        // Find the ItemFactory in the scene
        itemFactory = FindFirstObjectByType<ItemFactory>();

        // Validate that the ItemFactory was found
        if (itemFactory == null)
        {
            Debug.LogError("ItemFactory not found in the scene!");
            return; // Cannot proceed without the factory
        }

        // Check if PlaceTile script was found
        if (placeTileScript != null)
        {
            Debug.Log("PlaceTile script found in awake");
            // Start spawning different types of items
            StartCoroutine(WaitForGridAndSpawnItem(ItemType.HealthPack));
            StartCoroutine(WaitForGridAndSpawnItem(ItemType.Flag));
            StartCoroutine(WaitForGridAndSpawnItem(ItemType.Armor));
            StartCoroutine(WaitForGridAndSpawnItem(ItemType.Missile));
            StartCoroutine(WaitForGridAndSpawnItem(ItemType.Gasoline));
        }
    }

    // Checks if the player has picked up any items
    // Compares player position with item positions and handles pickup effects
    private void CheckItemPickup()
    {
        // Find the player tank in the scene
        PlayerTank playerTank = FindObjectOfType<PlayerTank>();
        if (playerTank == null) return;
        
        // Get the player's current position
        Vector3 playerPosition = playerTank.transform.position;
        
        // Iterate through all spawned items in reverse order
        // Reverse order to safely remove items during iteration
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            // Skip if item was already destroyed
            if (spawnedItems[i] == null) 
            {
                spawnedItems.RemoveAt(i);
                continue;
            }
            
            // Get the item's position
            Vector3 itemPosition = spawnedItems[i].transform.position;
            
            // Calculate distance between player and item on X-Y plane only
            // This ignores height differences (Z-axis)
            float distanceXY = Vector2.Distance(
                new Vector2(playerPosition.x, playerPosition.y),
                new Vector2(itemPosition.x, itemPosition.y)
            );
            
            // Check if player is close enough to pick up the item
            // 0.5f is the pickup threshold distance
            if (distanceXY < 0.5f)
            {
                // Get reference to the current item
                GameObject currentItem = spawnedItems[i];
                
                // Get the item's tag to determine its type
                string itemTag = currentItem.tag;
                
                // Log the pickup for debugging
                Debug.Log($"Player picked up item with tag: {itemTag}");
                
                // Handle different item types based on their tag
                if (itemTag == "HealthPack")
                {
                    // Log player's health before healing
                    Debug.Log("Player health before health pack: " + playerTank.GetHealth());
                    
                    // Create the appropriate health pack effect based on game mode
                    HealthPackEffect healthEffect = HealthPackEffectFactory.CreateEffect();
                    int healAmount = healthEffect.GetHealAmount();
                    
                    // Apply healing to the player
                    int currentHealth = playerTank.GetHealth();
                    playerTank.SetHealth(currentHealth + healAmount);

                    // Play pickup sound effect if SoundManager is available
                    if (SoundManager.GetInstance() != null)
                    {
                        SoundManager.GetInstance().PickupSound();
                        Debug.Log("Played pickup sound.");
                    }
                    else
                    {
                        Debug.LogError("SoundManager.Instance is null! Cannot play pickup sound.");
                    }

                    // Log healing information for debugging
                    Debug.Log($"BC Mode: {PlayerPrefs.GetInt("BCMode", 0) == 1}, Healed for: {healAmount}");
                    Debug.Log("Player health after health pack: " + playerTank.GetHealth());
                }
                else if (itemTag == "Flag")
                {
                    // Handle flag pickup - likely a game objective
                    Debug.Log("Player picked up the flag!");

                    // Play flag pickup sound if SoundManager is available
                    if (SoundManager.GetInstance() != null)
                    {
                        SoundManager.GetInstance().flagPickupSound();
                        Debug.Log("Played flag pickup sound.");
                    }
                    else
                    {
                        Debug.LogError("SoundManager.Instance is null! Cannot play flag pickup sound.");
                    }

                    // Find the BattleSystem and trigger scene change
                    BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
                    if (battleSystem != null)
                    {
                        Debug.Log("BattleSystem found, calling DecideScene");
                        battleSystem.SendMessage("DecideScene");
                    }
                    else
                    {
                        Debug.LogError("BattleSystem not found, cannot trigger game scene condition");
                    }
                }
                else if (itemTag == "Armor")
                {
                    // Handle armor pickup
                    Debug.Log("Player picked up armor!");

                    playerTank.ActivateArmorShield();

                    // Play armor pickup sound if SoundManager is available
                    if (SoundManager.GetInstance() != null)
                    {
                        SoundManager.GetInstance().healthPickupSound();
                        Debug.Log("Played healthpickup sound.");
                    }
                    else
                    {
                        Debug.LogError("SoundManager.Instance is null! Cannot play healthpickup sound.");
                    }
                }
                else if (itemTag == "Missile")
                {
                    // Handle missile pickup
                    Debug.Log("Player picked up missile!");
                    
                }
                else if (itemTag == "Gasoline")
                {
                    // Handle gasoline pickup
                    Debug.Log("Player picked up gasoline!");
                    
                }
                // Add more item types here as needed
                // else if (itemType.Contains("PowerUp")) { ... }
                // else if (itemType.Contains("Ammo")) { ... }

                // Remove the item from the game and from the tracking list
                Destroy(currentItem);
                spawnedItems.RemoveAt(i);
            }
        }
    }

    // Coroutine that waits for the grid to be ready, then spawns an item
    // This ensures items are only spawned on valid grid positions
    public IEnumerator WaitForGridAndSpawnItem(ItemType itemTypeToSpawn)
    {
        // Wait for any other item spawning to complete
        // This prevents multiple spawns from happening simultaneously
        while (isSpawningItem)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Set flag to indicate spawning is in progress
        isSpawningItem = true;

        // Wait until the grid is initialized and has valid dimensions
        while (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0)
        {
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }

        // Get grid dimensions from PlaceTile script
        int width = placeTileScript.width;
        int height = placeTileScript.height;

        // Initialize position variable
        Vector3 itemPos = Vector3.zero;
        bool validPosition = false;

        // Keep trying until a valid position is found
        // Valid positions are those that don't have EarthTerrain (mountains)
        while (!validPosition)
        {
            int randX;
            int randY;

            // Determine spawn range based on item type
            if (itemTypeToSpawn == ItemType.Flag)
            {
                // Spawn flag in the top 1/6th of the map
                // This ensures flags appear in a specific area
                randX = Random.Range(0, width);
                randY = Random.Range(height * 5 / 6, height);
            }
            else
            {
                // Spawn other items anywhere on the grid
                randX = Random.Range(0, width);
                randY = Random.Range(0, height);
            }

            // Ensure coordinates are within valid grid bounds
            if (randX >= 0 && randX < width && randY >= 0 && randY < height)
            {
                // Get the world position for this grid cell
                itemPos = placeTileScript.Grid[randX, randY];
                itemPos.z = -1f; // Set Z to ensure item appears above the tiles

                // Check if there's a collider at this position
                Collider2D tileCollider = Physics2D.OverlapPoint(itemPos);
                if (tileCollider != null)
                {
                    // Check if the collider has an EarthTerrain component
                    if (tileCollider.GetComponent<EarthTerrain>() == null) 
                    {
                        // Position is valid if it's not EarthTerrain
                        validPosition = true;
                    }
                    // If it is EarthTerrain, continue loop to find a new position
                }
                else
                {
                    // If no collider is found, assume the position is valid
                    validPosition = true;
                }
            }
            else
            {
                // Log error if coordinates are out of bounds (shouldn't happen with Random.Range)
                Debug.LogError($"Generated invalid coordinates ({randX}, {randY}) for grid size ({width}, {height}). Retrying...");
            }

            // If position is still not valid, wait a frame before trying again
            if (!validPosition)
            {
                yield return null;
            }
        }
        // At this point, a valid position has been found

        // Use the ItemFactory to create the item at the valid position
        GameObject spawnedItem = itemFactory.CreateItem(itemTypeToSpawn, itemPos);

        // If item was successfully created, add it to the tracking list
        if (spawnedItem != null)
        {
            if (itemTypeToSpawn == ItemType.Missile)
            {
                spawnedItem.transform.Rotate(0, 0, -45f);
            }
            
            spawnedItems.Add(spawnedItem);
        }
        // If creation failed, error is already logged by the factory

        // Reset the spawning flag
        isSpawningItem = false;
    }
}
