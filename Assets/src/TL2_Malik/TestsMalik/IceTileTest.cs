using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TerrainIceTests
{
    private GameObject terrainIceObject;
    private TerrainIce terrainIce;

    private GameObject playerTankObject;
    private PlayerTank playerTank;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create PlayerTank with dummy component
        playerTankObject = new GameObject("PlayerTank");
        playerTank = playerTankObject.AddComponent<PlayerTank>();
        playerTank.SetActionPoints(3); // Starting with 3 action points

        // Create TerrainIce GameObject
        terrainIceObject = new GameObject("TerrainIce");
        terrainIceObject.transform.position = Vector3.zero;
        terrainIce = terrainIceObject.AddComponent<TerrainIce>();

        yield return null; // Wait a frame for Start() to run
    }

    [UnityTest]
    public IEnumerator PlayerOnTerrainIce_LosesActionPoint()
    {
        // Put player on top of the terrain
        playerTankObject.transform.position = terrainIceObject.transform.position;

        // Wait a few frames to trigger Update
        yield return new WaitForSeconds(0.5f);

        // Should have 2 action points now
        Assert.AreEqual(2, playerTank.GetActionPoints());
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerTankObject);
        Object.Destroy(terrainIceObject);
    }
}
