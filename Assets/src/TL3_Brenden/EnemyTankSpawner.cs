using UnityEngine;

public class EnemyTankSpawner : MonoBehaviour
{

    public GameObject enemyTankPrefab;  // Assign your enemy tank prefab in the Inspector
    private PlaceTile placeTileScript;
    private GameObject enemyTank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeTileScript = FindObjectOfType<PlaceTile>();  // Get reference to PlaceTile script

        if (placeTileScript != null)
        {
            Invoke("SpawnEnemyTank", 0.1f);
        }
        else
        {
            Debug.LogError("PlaceTile script not found in the scene!");
        }

    }
    void SpawnEnemyTank()
    {
        //Vector3 playerPos = placeTileScript.playertank.transform.position;
        Vector3 enemyPos;

        int width = placeTileScript.width;
        int height = placeTileScript.height;
       /* do
        {*/
            int randX = Random.Range(0, width);
            int randY = Random.Range(0, height);
            enemyPos = placeTileScript.Grid[randX, randY];
        enemyPos.z = -1f;
        /*}
        while (enemyPos == playerPos);  // Ensure the enemy does not spawn on the player's tile
       */
        enemyTank = Instantiate(enemyTankPrefab, enemyPos, Quaternion.identity);
        enemyTank.name = "EnemyTank";
    }
}

