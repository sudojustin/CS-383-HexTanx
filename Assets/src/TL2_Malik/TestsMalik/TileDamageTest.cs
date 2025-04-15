using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class TerrainDamageTests
{
    private GameObject player;
    private PlayerTank playerTank;
    private GameObject terrainObj;
    private Terrains terrainScript;

    [SetUp]
    public void Setup()
    {
        // Create mock player
        player = new GameObject("PlayerTank");
        playerTank = player.AddComponent<PlayerTank>();
        playerTank.SetHealth(100);

        // Create terrain
        terrainObj = new GameObject("FireTile");
        terrainObj.transform.position = Vector3.zero;
        terrainScript = terrainObj.AddComponent<Terrains>();

        // Place player on the same position
        player.transform.position = Vector3.zero;
    }

    [UnityTest]
    public IEnumerator TerrainDealsCorrectDamage_WhenBCModeOff()
    {
        PlayerPrefs.SetInt("BCMode", 0); // BC Mode OFF → should deal damage

        terrainScript.Invoke("Start", 0);
        yield return null;

        yield return new WaitForSeconds(0.1f); // Give time for Update to run
        terrainScript.Invoke("Update", 0);

        yield return null;

        Assert.AreEqual(80, playerTank.GetHealth(), "Player should take 10 damage when BCMode is OFF.");
    }

    [UnityTest]
    public IEnumerator TerrainDealsNoDamage_WhenBCModeOn()
    {
        PlayerPrefs.SetInt("BCMode", 1); // BC Mode ON → should deal 0 damage

        playerTank.SetHealth(100);
        terrainScript.Invoke("Start", 0);
        yield return null;

        yield return new WaitForSeconds(0.1f); // Let Update logic kick in
        terrainScript.Invoke("Update", 0);

        yield return null;

        Assert.AreEqual(100, playerTank.GetHealth(), "Player should take 0 damage when BCMode is ON.");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(terrainObj);
        PlayerPrefs.DeleteKey("BCMode");
    }
}
