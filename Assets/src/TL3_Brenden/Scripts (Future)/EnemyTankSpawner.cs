using UnityEngine;

public class EnemyTankSpawner : MonoBehaviour
{
    private PlaceTile placeTileScript;
    private TankManager tankManager;
    private GameObject enemyTank;

    void Start()
    {
        placeTileScript = FindObjectOfType<PlaceTile>();
        tankManager = FindObjectOfType<TankManager>();

        if (tankManager == null)
        {
            Debug.LogError("EnemyTankSpawner: No TankManager found in the scene!");
            return;
        }

        if (placeTileScript != null)
        {
            //Invoke("SpawnEnemyTankWithRandomPosition", 0.1f);
        }
        else
        {
            Debug.LogError("PlaceTile script not found in the scene!");
        }
    }

    public void SpawnEnemyTankWithRandomPosition()
    {
        if (placeTileScript.Grid == null || placeTileScript.Grid.GetLength(0) == 0)
        {
            Debug.LogError("Grid is not populated properly.");
            return;
        }

        int width = placeTileScript.width;
        int height = placeTileScript.height;

        Vector3 enemyPos; //= placeTileScript.Grid[randX, randY];
        enemyPos.z = -1f; // Ensure the enemy appears above the tiles
        bool validPosition = false;
        while (!validPosition)
        {
            int randX = Random.Range(0, width);
            int randY = Random.Range((0 + (height / 2) + (height / 3)), height);
            enemyPos = placeTileScript.Grid[randX, randY];

            // Check if the tile at (randX, randY) is not a special terrain
            Collider2D tileCollider = Physics2D.OverlapPoint(enemyPos);

            if (tileCollider != null)
            {
                Tiles tile = tileCollider.GetComponent<Tiles>();
                if (tile != null && !(/*tile is TerrainIce || tile is Terrains ||*/ tile is EarthTerrain))
                {
                    validPosition = true; // Found a valid tile
                    enemyPos.z = -1f; // Ensure the enemy appears above the tiles
                    SpawnEnemyTank(enemyPos);
                }
            }
        }
    }

    public void SpawnEnemyTank(Vector3 spawnLocation)
    {
        if (tankManager == null)
        {
            Debug.LogError("EnemyTankSpawner: No TankManager assigned!");
            return;
        } // Prevent errors if TankManager is missing
        GameObject tankPrefab = tankManager.GetTankForCurrentScene();
        if (tankPrefab == null)
        {
            Debug.LogError("EnemyTankSpawner: No tank prefab returned from TankManager!");
            return;
        }
        //Debug.Log("Spawning an enemy tank at: " + spawnLocation);  //  Confirm method runs

        GameObject spawnedTank = Instantiate(tankPrefab, spawnLocation, Quaternion.identity);
        TankType tank = spawnedTank.GetComponent<TankType>();

        if (tank != null)
        {
            tank.Initialize();// Call Initialize() since we made it parameterless
            tank.UpdateTankLocation(spawnLocation);
            //Debug.Log("Enemy tank initialized successfully!");
        }
        else
        {
            Debug.LogError("Spawned object does not have a TankType component!");
        }

    }
}

