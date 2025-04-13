using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SoundManagerTest
{
    private GameObject soundManager;
    private AudioSource musicSource;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing
        SceneManager.LoadScene("level1"); // Load the main menu scene
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing


    }

    [UnityTest]
    public IEnumerator SoundManager_InitializesAndPlaysL1Music()
    {
        // Find the SoundManager GameObject
        soundManager = GameObject.Find("soundManager");
        Assert.NotNull(soundManager, "SoundManager GameObject not found in the MainMenu scene!");


        // Get the AudioSource components (assumes order: sound effects, music, pickup sounds)
        AudioSource[] audioSources = soundManager.GetComponents<AudioSource>();
        Assert.IsTrue(audioSources.Length >= 3, "SoundManager does not have at least 3 AudioSource components!");

        // Assign the music AudioSource (second AudioSource)
        musicSource = audioSources[1];

        yield return new WaitForSeconds(0.5f); 
        // Test 1: Verify that the SoundManager is initialized
        Assert.IsNotNull(soundManager, "SoundManager script is not initialized!");
        Assert.IsNotNull(musicSource, "Music AudioSource is not initialized!");

        // Test 2: Verify that the main menu music is playing
        Assert.IsTrue(musicSource.isPlaying, "Main menu music is not playing in the MainMenu scene!");
        Assert.IsNotNull(musicSource.clip, "Music AudioSource does not have a clip assigned!");



        Assert.Pass("SoundManager initialized and main menu music is playing correctly");
    }
}