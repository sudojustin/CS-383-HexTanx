using UnityEngine;

// Define the types of items available
public enum ItemType
{
    HealthPack,
    Flag,
    Armor
    // Add other item types here later
}

public class ItemFactory : MonoBehaviour
{
    // Assign these prefabs in the Unity Inspector
    public GameObject healthPackPrefab;
    public GameObject flagPrefab;
    public GameObject armorPrefab;

    // Factory method to create items
    public GameObject CreateItem(ItemType type, Vector3 position)
    {
        GameObject prefabToSpawn = null;

        // Select the correct prefab based on the type
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

        // Check if the selected prefab is assigned
        if (prefabToSpawn == null)
        {
            Debug.LogError($"Prefab for item type '{type}' is not assigned in the ItemFactory Inspector!");
            return null;
        }

        // Instantiate the chosen prefab at the given position
        GameObject newItem = Instantiate(prefabToSpawn, position, Quaternion.identity);
        
        // Optional: Add a component to the instantiated item to easily identify its type later
        // ItemComponent itemComponent = newItem.AddComponent<ItemComponent>();
        // itemComponent.Type = type; 

        // Set tag based on type for pickup logic compatibility
        // Ensure these tags exist in your Unity project's Tag Manager
        switch (type)
        {
            case ItemType.HealthPack: newItem.tag = "HealthPack"; break;
            case ItemType.Flag:       newItem.tag = "Flag"; break;
            case ItemType.Armor:      newItem.tag = "Armor"; break;
        }

        return newItem;
    }
} 