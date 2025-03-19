using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on MusicManager!");
            return;
        }

        // Ensure music starts and loops
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}