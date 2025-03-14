using UnityEngine;

public class PlayerTankSpawner : MonoBehaviour
{
    private PlaceTile placeTileScript;
    public GameObject playerTankPrefab; 

    void Start()
    {
        placeTileScript = FindObjectOfType<PlaceTile>();

        if (placeTileScript != null)
        {
            Invoke("SpawnPlayerTankWithPosition", 0.0f); 
        }
        else
        {
            Debug.LogError("PlayerTankSpawner: PlaceTile script not found in the scene!");
        }
    }

    public void SpawnPlayerTankWithPosition()
    {
        if (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0)
        {
            Debug.LogError("Grid is not populated properly.");
            return;
        }

        int width = placeTileScript.width;
        int height = placeTileScript.height;

        // Spawn at a fixed position 
        int spawnX = width / 2;
        int spawnY = 1;         
        Vector3 playerPos = placeTileScript.Grid[spawnX, spawnY];
        playerPos.z = -1f; 
        SpawnPlayerTank(playerPos);
    }

    public void SpawnPlayerTank(Vector3 spawnLocation)
    {
        if (playerTankPrefab == null)
        {
            Debug.LogError("PlayerTankSpawner: No player tank prefab assigned!");
            return;
        }

        Debug.Log("Spawning player tank at: " + spawnLocation); 

        GameObject spawnedTank = Instantiate(playerTankPrefab, spawnLocation, Quaternion.identity);
        spawnedTank.name = "PlayerTank"; 

        PlayerMovement movement = spawnedTank.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            Debug.Log("Player tank movement component found");
        }
        else
        {
            Debug.LogWarning("Player tank spawned but lacks PlayerMovement component");
        }
    }
}