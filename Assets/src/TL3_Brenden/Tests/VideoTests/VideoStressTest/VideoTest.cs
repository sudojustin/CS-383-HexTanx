using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

/*public class VideoTest
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    [UnityTest]
    public IEnumerator Shoot_AtEnemyTest()
    {
        var placePlayer = GameObject.Find("PlayerTank");
        var playerShooter = GameObject.FindObjectOfType<PlayerShooting>();

        Assert.NotNull(playerShooter, "PlayerShooting not found in scene");
        Assert.NotNull(placePlayer, "PlayerTank not found in scene");

        // Move player to the correct position
        placePlayer.transform.position = new Vector3(0.5f, 0, -1);

        bool isDone = false;
        float startTime = Time.time;
        float testDuration = 10f; // Timeout limit

        while (!isDone && Time.time - startTime < testDuration)
        {
            playerShooter.Shoot(); // Shoot first

            yield return new WaitForSeconds(0.1f); // Wait for bullet to spawn

            var bulletShooter = GameObject.FindObjectOfType<Projectile>();
            if (bulletShooter != null)
            {
                bulletShooter.speed += 1000;
                float fps = 1.0f / Time.deltaTime;
                Debug.Log($"Speed: {bulletShooter.speed}, FPS: {fps}");
            }
            else
            {
                Debug.LogWarning("Projectile not found after shooting.");
            }

            if(bulletShooter.transform.position.y > 1f || bulletShooter.transform.position.y < -1f)
            {
                Assert.Fail("Bullets went past map");
            }

            yield return null;
        }

        if (!isDone)
        {
            Assert.Pass("Test completed successfully within time limit.");
        }
    }
       
}*/
