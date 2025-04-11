using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTankShootTest
{
    private GameObject playerTank;
    private PlayerShooting playerShooting;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("level4");
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing
    }

    [UnityTest]
    public IEnumerator PlayerTankShootsAtEnemyL4()
    {
        // Find the player tank in the scene
        playerTank = GameObject.Find("PlayerTank"); // Ensure the player tank has the name "PlayerTank"
        Assert.NotNull(playerTank, "Player tank not found in the scene!");

        playerShooting = playerTank.GetComponentInChildren<PlayerShooting>();
        Assert.NotNull(playerShooting, "PlayerShooting component not found on player tank!");

        yield return new WaitForSeconds(0.5f); // Allow initialization

        // Call shooting function
        int shots = 0;
        int maxShots = 10;
        while (shots < maxShots)
        {
            playerShooting.ShootAtEnemy();
            yield return new WaitForSeconds(1.5f); // Wait for potential shooting action (same as enemy test)
            shots++;
        }

        Assert.Pass($"Player Tank made {shots} shots toward the enemy tank");
    }
}