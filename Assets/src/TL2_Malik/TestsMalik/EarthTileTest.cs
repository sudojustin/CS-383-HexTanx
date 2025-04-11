using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class EarthTerrainTests
{
    private GameObject terrainGO;
    private EarthTerrain earthTerrain;
    private GameObject bullet1;
    private GameObject bullet2;

    [SetUp]
    public void Setup()
    {
        // Create the EarthTerrain object
        terrainGO = new GameObject("EarthTerrain");
        terrainGO.transform.position = Vector3.zero;
        earthTerrain = terrainGO.AddComponent<EarthTerrain>();

        // Create bullet projectiles
        bullet1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet1.tag = "Projectile";
        bullet1.name = "Bullet1";
        bullet1.transform.position = new Vector3(5f, 0f, 0f); 

        bullet2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet2.tag = "Projectile";
        bullet2.name = "Bullet2";
        bullet2.transform.position = new Vector3(5f, 0f, 0f); // Out of range
    }

    [UnityTest]
    public IEnumerator EarthTerrain_DestroysProjectileWithinRange()
    {
        // Wait one frame for Update to pick up bullets
        yield return null;

        // First frame: should detect and track bullets
        Assert.AreEqual(2, GameObject.FindGameObjectsWithTag("Projectile").Length);
        bullet1.transform.position = new Vector3(0.5f, 0f, 0f);
        // Wait enough time for EarthTerrain Update to destroy the in-range bullet
        yield return new WaitForSeconds(0.2f);

        // Check that only one projectile remains
        GameObject[] remainingProjectiles = GameObject.FindGameObjectsWithTag("Projectile");
        Assert.AreEqual(1, remainingProjectiles.Length);
        Assert.AreEqual("Bullet2", remainingProjectiles[0].name); // The one outside the range

        // Optional: Make sure Bullet1 is destroyed
        Assert.IsTrue(bullet1 == null || !bullet1, "Bullet1 should have been destroyed.");
    }

    [TearDown]
    public void TearDown()
    {
        if (bullet1) Object.DestroyImmediate(bullet1);
        if (bullet2) Object.DestroyImmediate(bullet2);
        if (terrainGO) Object.DestroyImmediate(terrainGO);
    }
}
