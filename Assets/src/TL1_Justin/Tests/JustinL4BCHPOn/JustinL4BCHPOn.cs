using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class JustinL4BCHPOn
{
    private ItemManager itemManager;
    private GameObject playerTankObject;

    [OneTimeSetUp]
    public void LoadScene()
    {
        // Load Level 4 scene
        SceneManager.LoadScene("Level4");
    }

    // Test to ensure health packs heal for 100 when BC mode is enabled
    [Test]
    public void HealthPackHealAmountBCModeEnabled()
    {
        // Enable BC mode
        PlayerPrefs.SetInt("BCMode", 1);
        PlayerPrefs.Save();
        
        // Create a health pack effect using the factory
        HealthPackEffect healthEffect = HealthPackEffectFactory.CreateEffect();
        
        // Verify that the healing amount is 100 (BC mode healing amount)
        int healAmount = healthEffect.GetHealAmount();
        
        Debug.Log($"BC Mode: {PlayerPrefs.GetInt("BCMode", 0) == 1}, Heal Amount: {healAmount}");
        
        Assert.AreEqual(100, healAmount, 
            $"Health pack heal amount ({healAmount}) does not match expected value (100) with BC mode enabled");
    }
}
