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
            //Invoke("SpawnPlayerTankWithPosition", 0.0f); 
        }
        else
        {
            Debug.LogError("PlayerTankSpawner: PlaceTile script not found in the scene!");
        }
    }

    public void SpawnPlayerTankWithPosition()
    {
        Debug.Log("SpawnPlayerTankWithPosition() called!");
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

        if (IsEarthTerrain(playerPos))
        {
            Debug.LogWarning($"Fixed spawn position ({spawnX}, {spawnY}) is an EarthTerrain tile!.");
            // Spawn at random if earth tile
            SpawnPlayerTankWithRandomPosition();
            return;
        }

    
        SpawnPlayerTank(playerPos);
    }

    public void SpawnPlayerTankWithRandomPosition()
    {
        if (placeTileScript == null) placeTileScript = FindObjectOfType<PlaceTile>();

        if (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0 || placeTileScript.Grid.GetLength(1) == 0)
        {
            Debug.LogError("Grid is not populated properly.");
            return;
        }

        int width = placeTileScript.width;
        int height = placeTileScript.height;

        Vector3 playerPos;
        bool validPosition = false;

        while (!validPosition)
        {
           
            int randX = Random.Range(0, width);
            int randY = Random.Range(0, (height / 3));
            playerPos = placeTileScript.Grid[randX, randY];
            playerPos.z = -1f;

            // Check if the tile is not a mountain 
            if (!IsEarthTerrain(playerPos))
            {
                validPosition = true;
                Debug.Log($"Found valid non-mountain tile at ({randX}, {randY})");
                SpawnPlayerTank(playerPos);
            }
            
        }
    }



    public void SpawnPlayerTank(Vector3 spawnLocation)
    {
        //Debug.Log("SpawnPlayerTank() called!");
        if (playerTankPrefab == null)
        {
            Debug.LogError("PlayerTankSpawner: No player tank prefab assigned!");
            return;
        }

        //Debug.Log("Spawning player tank at: " + spawnLocation); 

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

    private bool IsEarthTerrain(Vector3 position)
    {
        Collider2D tileCollider = Physics2D.OverlapPoint(position);
        if (tileCollider != null)
        {
            EarthTerrain terrain = tileCollider.GetComponent<EarthTerrain>();
            if (terrain != null)
            {
                Debug.Log("PlayerTankSpawner: Avoiding Earth Terrain at: " + position);
                return true;
            }
        }
        return false;
    }
}