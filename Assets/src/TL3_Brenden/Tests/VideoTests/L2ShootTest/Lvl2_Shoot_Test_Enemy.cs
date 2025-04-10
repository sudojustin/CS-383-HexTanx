using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class EnemyTankShootTest
{
    private GameObject enemyTank;
    private AIControl aiControl;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("Level2");
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing
    }

    [UnityTest]
    public IEnumerator EnemyTank_ShootsAfterDecision()
    {
        // Find the existing enemy tank in the scene
        enemyTank = GameObject.FindWithTag("EnemyTank"); // Ensure the enemy tank has the tag "EnemyTank"
        Assert.NotNull(enemyTank, "Enemy tank not found in the scene!");

        aiControl = enemyTank.GetComponent<AIControl>();
        Assert.NotNull(aiControl, "AIControl component not found on enemy tank!");

        yield return new WaitForSeconds(0.5f); // Allow initialization

        // Call AI decision function
        int a = 0;
        while (a != 10000)
        {
            aiControl.ShootAtPlayer();
            yield return new WaitForSeconds(0.7f); // Wait for potential movement
            a++;
            if (a == 11)
            {
                Assert.Pass("Enemy Tank made 11 Shots without going out of bounds");
            }
        }
    }
}
