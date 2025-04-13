using UnityEngine;

// Defines the different types of items that can be created by the ItemFactory.
// This enum is used to specify which item to instantiate.
public enum ItemType
{
    HealthPack,
    Flag,
    Armor
    // TODO: Add other item types here later
}

/*
 * FACTORY PATTERN EXPLANATION
 * 
 * This is a Concrete Factory implementation of the Factory Method pattern.
 * It's a MonoBehaviour class that creates different types of game objects (items) based on an enum parameter.
 * The factory method is CreateItem(ItemType type, Vector3 position) which returns a GameObject.
 * It centralizes the creation logic for different item types (HealthPack, Flag, Armor).
 * The factory handles the instantiation, tagging, and setup of each item type.
 * 
 * This factory demonstrates key principles of the Factory pattern:
 * - It encapsulates object creation logic
 * - It provides a single point of control for creating objects
 * - It allows for easy extension (adding new item types)
 * - It hides the implementation details of how objects are created
 * 
 * Note for me:
 * In game development, factories are commonly used to:
 * 1. Create game objects with specific behaviors
 * 2. Manage different variations of the same type of object
 * 3. Centralize creation logic to make the code more maintainable
 * 4. Support game modes or settings that require different object behaviors
 */
public class ItemFactory : MonoBehaviour
{
    // Each prefab represents the visual and physical representation of an item type
    public GameObject healthPackPrefab;
    public GameObject flagPrefab;
    public GameObject armorPrefab;

    // Creates and returns a new item of the specified type at the given position.
    // <param name="type">The type of item to create (from ItemType enum)</param>
    // <param name="position">The world position where the item should be spawned</param>
    // <returns>The instantiated GameObject, or null if creation failed</returns>
    public GameObject CreateItem(ItemType type, Vector3 position)
    {
        GameObject prefabToSpawn = null;

        // Select the appropriate prefab based on the requested item type
        switch (type)
        {
            case ItemType.HealthPack:
                prefabToSpawn = healthPackPrefab;
                break;
            case ItemType.Flag:
                prefabToSpawn = flagPrefab;
                break;
            case ItemType.Armor:
                prefabToSpawn = armorPrefab;
                break;
            // TODO: Add cases for other item types here
            default:
                Debug.LogError($"Item type '{type}' not recognized by the factory.");
                return null; // Return null if type is unknown
        }

        // Validate that the prefab was properly assigned in the Inspector
        if (prefabToSpawn == null)
        {
            Debug.LogError($"Prefab for item type '{type}' is not assigned in the ItemFactory Inspector!");
            return null;
        }

        // Create the item in the game world at the specified position
        // Quaternion.identity means no rotation (default orientation)
        GameObject newItem = Instantiate(prefabToSpawn, position, Quaternion.identity);

        // Set the appropriate tag on the item for pickup detection
        switch (type)
        {
            case ItemType.HealthPack: newItem.tag = "HealthPack"; break;
            case ItemType.Flag:       newItem.tag = "Flag"; break;
            case ItemType.Armor:      newItem.tag = "Armor"; break;
            // TODO: Add cases for other item types here
        }

        return newItem;
    }
} 