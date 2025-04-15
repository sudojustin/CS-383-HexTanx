using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class JustinL3HPSpawnFPS
{
    private ItemManager itemManager;
    private const float MIN_FPS_THRESHOLD = 30f; // Minimum acceptable FPS
    private const int NUM_HEALTH_PACKS_TO_SPAWN = 50; // Number of health packs to spawn
    private const float SPAWN_INTERVAL = 0.2f; // Time between spawns in seconds
    
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Level3");
    }
    
    // Test to ensure FPS stays above threshold while spawning health packs
    [UnityTest]
    public IEnumerator HealthPackSpawnFPSThreshold()
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
        
        // List to store FPS measurements
        List<float> fpsMeasurements = new List<float>();
        
        // Spawn multiple health packs and measure FPS
        for (int i = 0; i < NUM_HEALTH_PACKS_TO_SPAWN; i++)
        {
            // Spawn a health pack
            yield return itemManager.StartCoroutine(itemManager.WaitForGridAndSpawnItem(ItemType.HealthPack));
            
            // Wait a frame to let the spawn complete
            yield return null;
            
            // Measure FPS
            float currentFPS = 1.0f / Time.deltaTime;
            fpsMeasurements.Add(currentFPS);
            
            Debug.Log($"Health pack {i+1} spawned. Current FPS: {currentFPS}");
            
            // Wait before spawning the next health pack
            yield return new WaitForSeconds(SPAWN_INTERVAL);
        }
        
        // Calculate average FPS
        float averageFPS = 0f;
        foreach (float fps in fpsMeasurements)
        {
            averageFPS += fps;
        }
        averageFPS /= fpsMeasurements.Count;
        
        Debug.Log($"Average FPS during health pack spawning: {averageFPS}");
        
        // Ensure average FPS is above threshold
        Assert.IsTrue(averageFPS >= MIN_FPS_THRESHOLD, 
            $"Average FPS ({averageFPS}) fell below minimum threshold of {MIN_FPS_THRESHOLD} while spawning health packs");
        
        // Also check if any individual measurement fell below threshold
        float minFPS = Mathf.Min(fpsMeasurements.ToArray());
        Assert.IsTrue(minFPS >= MIN_FPS_THRESHOLD, 
            $"Minimum FPS ({minFPS}) fell below minimum threshold of {MIN_FPS_THRESHOLD} while spawning health packs");
    }
}
