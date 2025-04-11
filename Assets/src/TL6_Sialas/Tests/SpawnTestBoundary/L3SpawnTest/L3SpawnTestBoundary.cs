using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class playerSpawnTest
{
    private PlayerTankSpawner playerTankSpawner;
    private PlaceTile placeTile;
    private PlayerTank playerTank;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Level3");
    }

    [UnityTest]
    public IEnumerator SpawnTanksInsideBoundaryL3()
    {
        playerTankSpawner = GameObject.FindObjectOfType<PlayerTankSpawner>();
        Assert.NotNull(playerTankSpawner, "playerTankSpawner not found in scene!");

        placeTile = GameObject.FindObjectOfType<PlaceTile>();
        Assert.NotNull(placeTile, "PlaceTile not found in scene!");

        playerTankSpawner.SpawnPlayerTankWithRandomPosition();

        playerTank = GameObject.FindObjectOfType<PlayerTank>();
        Assert.NotNull(placeTile, "PlaceTile not found in scene!");

        yield return new WaitForSeconds(2.0f); // Allow FPS to update

        Vector3 spawnPosition = playerTank.transform.position;
        Debug.Log($"Tank spawned at: {spawnPosition}");

        bool isOnTile = false;
        for (int x = 0; x < placeTile.width; x++)
        {
            for (int y = 0; y < placeTile.height; y++)
            {
                Vector3 gridPos = placeTile.Grid[x, y];
                gridPos.z = -1f;
                if (spawnPosition == gridPos)
                {
                    isOnTile = true;
                    break;
                }
            }
            if (isOnTile) break;
        }
        Debug.Log($"Tank check: Is on tile: {isOnTile}");
        Assert.IsTrue(isOnTile, $"Tank spawned outside tile map at {spawnPosition}! " + "Position does not match any tile.");
        if(isOnTile)
        {
            Assert.Pass("Player Tank Spawned in Bounds");
        }
    }
}
