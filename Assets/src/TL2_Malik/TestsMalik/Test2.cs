
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlaceTileTests
{
    private GameObject testObject;
    private PlaceTile placeTile;

    private GameObject dummyTile;
    private GameObject terrain1;
    private GameObject terrain2;
    private GameObject terrain3;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Setup main camera
        var cameraGO = new GameObject("MainCamera");
        var camera = cameraGO.AddComponent<Camera>();
        camera.tag = "MainCamera";

        // Create dummy tile prefabs
        dummyTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        terrain1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        terrain2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        terrain3 = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // Create test object
        testObject = new GameObject("PlaceTileObject");
        placeTile = testObject.AddComponent<PlaceTile>();

        // Set required public fields
        placeTile.Tile = dummyTile;
        placeTile.Terrain1 = terrain1;
        placeTile.Terrain2 = terrain2;
        placeTile.Terrain3 = terrain3;
        placeTile.min = 2;
        placeTile.max = 3;
        placeTile.offset = 0.5f;
        placeTile.offset2 = 0.5f;

        yield return null; // Wait a frame for Start to run
    }
    /*
    [UnityTest]
    public IEnumerator MakeMap_InstantiatesCorrectNumberOfTiles()
    {
        int testWidth = 3;
        int testHeight = 3;
        placeTile.runOnStart = false;
        placeTile.MakeMap(testWidth, testHeight);

        // Count how many tile objects were instantiated in total
        GameObject[] allTiles = GameObject.FindObjectsOfType<GameObject>();
        int tileCount = 0;

        foreach (GameObject obj in allTiles)
        {
            if (obj.name.Contains("Cube") || obj.name.Contains("Sphere") || obj.name.Contains("Cylinder") || obj.name.Contains("Capsule"))
            {
                tileCount++;
            }
        }
        //tileCount = 9;
        Assert.AreEqual(testWidth * testHeight, tileCount, "Tile count does not match grid size");

        yield return null;
    }
    */
    [UnityTest]
    public IEnumerator Camera_IsCenteredAfterMapGeneration()
    {
        int width = 4;
        int height = 4;

        placeTile.MakeMap(width, height);

        Vector3 expectedCamPos = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        Vector3 actualCamPos = Camera.main.transform.position;

        Assert.AreEqual(expectedCamPos, actualCamPos, "Camera is not centered correctly over the map");

        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObject);
        Object.DestroyImmediate(dummyTile);
        Object.DestroyImmediate(terrain1);
        Object.DestroyImmediate(terrain2);
        Object.DestroyImmediate(terrain3);

        var cam = GameObject.Find("MainCamera");
        if (cam != null) Object.DestroyImmediate(cam);
    }

/*
     [UnityTest]
    public IEnumerator StressTest_LargeMapInstantiation()
    {
        // Start measuring time
        float startTime = Time.realtimeSinceStartup;

        // Use a very large grid size for the stress test (e.g., 500x500)
        placeTileScript.width = 500;
        placeTileScript.height = 500;
        placeTileScript.Grid = null;

        // Call the method to create the map
        placeTileScript.MakeMap(placeTileScript.width, placeTileScript.height);

        // Wait for the instantiation to complete (allow Unity to process)
        yield return null;  // Yield once to ensure the first frame is processed
        yield return null;  // Yield again to let more objects get instantiated

        // Measure the time after the process is completed
        float endTime = Time.realtimeSinceStartup;

        // Log the time taken to instantiate the objects
        float duration = endTime - startTime;
        Debug.Log($"Stress Test: Time taken to instantiate the map: {duration} seconds.");

        // Assert that the map was created
        Assert.AreEqual(placeTileScript.width, placeTileScript.Grid.GetLength(0), "Grid width is incorrect");
        Assert.AreEqual(placeTileScript.height, placeTileScript.Grid.GetLength(1), "Grid height is incorrect");

        // Ensure the total number of tiles/terrain is as expected (width * height)
        int tileCount = GameObject.FindGameObjectsWithTag("Tile").Length;
        int terrainCount = GameObject.FindGameObjectsWithTag("Terrain").Length;
        //Assert.AreEqual(placeTileScript.width * placeTileScript.height, tileCount + terrainCount, "Total number of tiles and terrain is incorrect");

        // Log the count of instantiated objects
        //Debug.Log($"Number of Tiles instantiated: {tileCount}");
        //Debug.Log($"Number of Terrain instantiated: {terrainCount}");

        // Optional: Check if the frame rate dropped below a threshold (e.g., 30 FPS) during the test
        float frameRate = 1 / Time.unscaledDeltaTime;
        Debug.Log($"Frame rate during stress test: {frameRate} FPS");
        Assert.Greater(frameRate, 30, "Frame rate dropped below 30 FPS during stress test.");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.Destroy(placeTileObject);
        Object.Destroy(tilePrefab);
        Object.Destroy(terrainPrefab);
    }
    */
}



