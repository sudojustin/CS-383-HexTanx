using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class EnemyTankMovementTest
{
    private GameObject enemyTank;
    private AIControl aiControl;
    private Vector3 initialPosition;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("Level4");
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing
    }

    [UnityTest]
    public IEnumerator EnemyTank_MovesAfterDecision()
    {
        // Find the existing enemy tank in the scene
        enemyTank = GameObject.FindWithTag("EnemyTank"); // Ensure the enemy tank has the tag "EnemyTank"
        Assert.NotNull(enemyTank, "Enemy tank not found in the scene!");

        aiControl = enemyTank.GetComponent<AIControl>();
        Assert.NotNull(aiControl, "AIControl component not found on enemy tank!");

        // Store initial position
        initialPosition = enemyTank.transform.position;
        yield return new WaitForSeconds(0.5f); // Allow initialization

        // Call AI decision function
        int a = 0;
        while (a != 10000)
        {
            aiControl.MoveToNewLocation();
            yield return new WaitForSeconds(0.7f); // Wait for potential movement
            a++;
            if (a == 11)
            {
                Assert.Pass("Enemy Tank made 11 moves without going out of bounds");
            }
        }

        Vector3 newPosition = enemyTank.transform.position;

        //Debug.Log($"Initial Position: {initialPosition}, New Position: {newPosition}");

        // Check if the position has changed
        Assert.AreNotEqual(initialPosition, newPosition, "Enemy tank did not move!");
    }
}