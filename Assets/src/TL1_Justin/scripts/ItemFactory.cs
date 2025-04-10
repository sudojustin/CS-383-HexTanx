using UnityEngine;

// Defines the different types of items that can be created by the ItemFactory.
// This enum is used to specify which item to instantiate.
public enum ItemType
{
    HealthPack,
    Flag,
    Armor
    // Add other item types here later
}

// Factory class responsible for creating different types of game items.
// This follows the Factory design pattern to centralize item creation logic.
public class ItemFactory : MonoBehaviour
{
    // These prefabs must be assigned in the Unity Inspector
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
            // Add cases for other item types here
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
        // These tags must be defined in Unity's Tag Manager
        switch (type)
        {
            case ItemType.HealthPack: newItem.tag = "HealthPack"; break;
            case ItemType.Flag:       newItem.tag = "Flag"; break;
            case ItemType.Armor:      newItem.tag = "Armor"; break;
        }

        return newItem;
    }
} 