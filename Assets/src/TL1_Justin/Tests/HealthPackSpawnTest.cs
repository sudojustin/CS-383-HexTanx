using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthPackSpawnTest
{
    private const float MinFpsThreshold = 20f;  // Minimum FPS before we stop the test
    private const int MaxSpawnCount = 100;  // Max number of health packs to spawn for the test

    // Test to spawn health packs until FPS drops
    [UnityTest]
    public IEnumerator SpawnHealthPacksUntilFPSDrops()
    {
        // Set up the health pack prefab and ItemManager (assume they're already in the scene)
        GameObject healthPackPrefab = Resources.Load<GameObject>("Assets/src/TL1_Justin/prefabs/HealthPack.prefab"); // Adjust with your path

        // Create a reference to ItemManager
        ItemManager itemManager = GameObject.FindObjectOfType<ItemManager>();

        // Check if health pack prefab and ItemManager are set up
        Assert.IsNotNull(healthPackPrefab, "HealthPackPrefab not found in Resources.");
        Assert.IsNotNull(itemManager, "ItemManager not found in the scene.");

        int spawnCount = 0;

        // Loop to spawn health packs
        while (spawnCount < MaxSpawnCount)
        {
            // Spawn health pack at a random position
            // Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), -1);
            // Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);

            // yield return itemManager.StartCoroutine(itemManager.WaitForGridAnd)

            spawnCount++;

            // Check FPS
            float currentFps = 1.0f / Time.deltaTime;

            // If FPS is too low, break out of the loop
            if (currentFps < MinFpsThreshold)
            {
                Debug.Log($"FPS dropped below threshold: {currentFps}. Stopping spawn.");
                break;
            }

            // Wait a short frame before spawning the next health pack
            yield return null;
        }

        Debug.Log($"Spawned {spawnCount} health packs before FPS dropped.");
    }
}