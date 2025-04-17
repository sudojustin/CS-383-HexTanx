using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class JustinL2HPSpawnBounds
{
    private ItemManager itemManager;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Level2");
    }

    // Test to ensure health packs spawn within map boundaries
    [UnityTest]
    public IEnumerator HealthPackSpawnWithinBoundaries()
    {
        // Find the ItemManager in the scene
        itemManager = GameObject.FindObjectOfType<ItemManager>();
        Assert.NotNull(itemManager, "ItemManager not found in the scene");

        // Clear any existing spawned items
        itemManager.spawnedItems.Clear();

        // Find the PlaceTile script to get grid dimensions
        PlaceTile placeTileScript = GameObject.FindObjectOfType<PlaceTile>();
        Assert.NotNull(placeTileScript, "PlaceTile not found in the scene");

        // Wait for grid to be initialized
        yield return new WaitUntil(() => placeTileScript.Grid != null && placeTileScript.Grid.GetLength(0) > 0);

        // Track spawn locations
        Vector3 spawnPos = Vector3.zero;

        // Spawn a health pack
        yield return itemManager.StartCoroutine(itemManager.WaitForGridAndSpawnItem(ItemType.HealthPack));

        // Find the spawned health pack
        GameObject spawnedHealthPack = GameObject.Find("HealthPack(Clone)");
        Assert.NotNull(spawnedHealthPack, "Health pack was not spawned");

        if (spawnedHealthPack != null)
        {
            spawnPos = spawnedHealthPack.transform.position;
            Debug.Log($"Health pack spawned at position: {spawnedHealthPack.transform.position}");

            // Get grid dimensions
            int gridWidth = placeTileScript.width;
            int gridHeight = placeTileScript.height;

            // Ensure the health pack spawn location is within bounds
            Assert.IsTrue(spawnPos.x >= 0 && spawnPos.x < gridWidth, "Health pack spawned outside the X grid boundary.");
            Assert.IsTrue(spawnPos.y >= 0 && spawnPos.y < gridHeight, "Health pack spawned outside the Y grid boundary.");
        }
        else
        {
            Assert.Fail("Health pack was not spawned");
        }
    }
}
