using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // Audio players components
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource pickupSource; 

    // Volume control
    [SerializeField] private float effectsVolume = 1f;
    [SerializeField] private float musicVolume = 1f;

    // Sound effect clips
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip missileClip;
    [SerializeField] private AudioClip bossShootClip;
    [SerializeField] private AudioClip tronShootClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip explodeClip;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioClip healthPickupClip;
    [SerializeField] private AudioClip flagPickupClip;
    [SerializeField] private AudioClip oilPickupClip;
    [SerializeField] private AudioClip biblePickupClip;
    [SerializeField] private AudioClip playerMoveClip;
    [SerializeField] private AudioClip enemyMoveClip;
    [SerializeField] private AudioClip bossEnemyMoveClip;
    [SerializeField] private AudioClip animeMoveClip;
    [SerializeField] private AudioClip demonMoveClip;
    [SerializeField] private AudioClip tronMoveClip;
    [SerializeField] private AudioClip buttonClip;

    // Music clips
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip battleMusicClip;
    [SerializeField] private AudioClip finalBattleMusicClip;
    [SerializeField] private AudioClip hellMusicClip;
    [SerializeField] private AudioClip technoMusicClip;
    [SerializeField] private AudioClip animeMusicClip;
    [SerializeField] private AudioClip desertMusicClip;
    [SerializeField] private AudioClip winterMusicClip;
    [SerializeField] private AudioClip pauseMusicClip;
    [SerializeField] private AudioClip winMusicClip;
    [SerializeField] private AudioClip loseMusicClip;

    // Singleton instance (private static)
    private static SoundManager instance;

    // Public static method to access the instance (replaces direct access to Instance)
    public static SoundManager GetInstance()
    {
        if (instance == null)
        {
            // Lazy initialization: Find or create the SoundManager instance
            instance = FindObjectOfType<SoundManager>();
            if (instance == null)
            {
                // Create a new GameObject and attach SoundManager if it doesn't exist
                GameObject soundManagerObj = new GameObject("SoundManager");
                instance = soundManagerObj.AddComponent<SoundManager>();
                Debug.Log("SoundManager instance created via lazy initialization.");
            }

            // Ensure the instance persists across scenes
            DontDestroyOnLoad(instance.gameObject);
        }
        return instance;
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"A duplicate SoundManager instance was created in scene {SceneManager.GetActiveScene().name}. Destroying the duplicate.");
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Ensure AudioSources are assigned
        if (effectsSource == null)
        {
            effectsSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("EffectsSource was null - added new AudioSource.");
        }
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            Debug.Log("MusicSource was null - added new AudioSource with looping enabled.");
        }
        if (pickupSource == null)
        {
            pickupSource = gameObject.AddComponent<AudioSource>();
            pickupSource.playOnAwake = false;
            pickupSource.loop = false;
            pickupSource.volume = effectsVolume;
            Debug.Log("PickupSource was null - added new AudioSource.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("SoundManager subscribed to sceneLoaded event.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("SoundManager unsubscribed from sceneLoaded event.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllSoundEffects();

        string sceneName = scene.name.ToLower();
        Debug.Log($"Scene loaded: {sceneName}. Playing corresponding music.");

        switch (sceneName)
        {
            case "mainmenu":
                Debug.Log("Attempting to play Menu music.");
                MenuMusic();
                break;
            case "level1":
                Debug.Log("Attempting to play Battle music.");
                BattleMusic();
                break;
            case "level2":
                Debug.Log("Attempting to play Battle music.");
                BattleMusic();
                break;
            case "level3":
                Debug.Log("Attempting to play Battle music.");
                BattleMusic();
                break;
            case "level4":
                Debug.Log("Attempting to play final Battle music.");
                finalBattleMusic();
                break;
            case "level5":
                Debug.Log("Attempting to play Anime music.");
                animeMusic();
                break;
            case "level6":
                Debug.Log("Attempting to play desert music.");
                desertMusic();
                break;
            case "level7":
                winterMusic();
                break;
            case "level8":
                winterMusic();
                break;
            case "level9":
                winterMusic();
                break;
            case "level10":
                technoMusic();
                Debug.Log("Attempting to play TronLevel music.");

                break;
            case "level666":
                Debug.Log("Attempting to play Hell music.");
                hellMusic();
                break;
            case "pausescene":
                Debug.Log("Attempting to play Pause music.");
                PauseMusic();
                break;
            case "gameoverwin":
                Debug.Log("Attempting to play Win music.");
                WinMusic();
                break;
            case "gameoverlose":
                Debug.Log("Attempting to play Lose music.");
                LoseMusic();
                break;
            default:
                Debug.LogWarning($"No music defined for scene '{sceneName}'. Defaulting to Menu music.");
                MenuMusic();
                break;
        }
    }

    // Stop all sound effects (but not music) when a scene changes
    private void StopAllSoundEffects()
    {
        if (effectsSource.isPlaying)
        {
            effectsSource.Stop();
            Debug.Log("Stopped effectsSource sounds on scene change.");
        }
    }

    // Volume getters
    public float GetEffectsVolume()
    {
        return effectsVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    // Volume setters
    public void SetEffectsVolume(float volume)
    {
        effectsVolume = Mathf.Clamp01(volume);
        Debug.Log($"Effects volume set to {effectsVolume}");
        effectsSource.volume = effectsVolume;
        pickupSource.volume = effectsVolume; 
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        Debug.Log($"Music volume set to {musicVolume}");
        musicSource.volume = musicVolume;
    }

    // Play a single clip through the sound effects source
    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            //Debug.Log($"Playing sound effect: {clip.name}, volume={effectsVolume}, isPlaying={effectsSource.isPlaying}");
            effectsSource.PlayOneShot(clip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("AudioClip is null in Play!");
        }
    }

    // Play a single clip through the music source
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.Stop(); // Stop any currently playing music
            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.Play();
            //Debug.Log($"Playing music: {clip.name}, isPlaying={musicSource.isPlaying}");
        }
        else
        {
            Debug.LogWarning("AudioClip is null in PlayMusic!");
        }
    }

    // Play a random clip from an array with pitch variation
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = 1.0f;

        effectsSource.pitch = randomPitch;
        Play(clips[randomIndex]);
    }

    // Play a movement sound with the ability to stop it
    public void PlayMovementSound(AudioClip clip)
    {
        if (clip != null)
        {
            effectsSource.clip = clip;
            effectsSource.volume = effectsVolume;
            effectsSource.loop = false; 
            effectsSource.Play();
            //Debug.Log($"Playing movement sound: {clip.name}, isPlaying={effectsSource.isPlaying}");
        }
        else
        {
            Debug.LogWarning("Movement AudioClip is null in PlayMovementSound!");
        }
    }

    public void StopMovementSound()
    {
        if (effectsSource.isPlaying)
        {
            effectsSource.Stop();
            //Debug.Log("Movement sound stopped.");
        }
    }

    // Play the pickup sound 
    public void PickupSound()
    {
        if (pickupClip != null)
        {
            //Debug.Log($"Playing pickup sound: {pickupClip.name}, volume={effectsVolume}, isPlaying={pickupSource.isPlaying}");
            pickupSource.PlayOneShot(pickupClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("PickupClip is null in PickupSound!");
        }
    }

    public void healthPickupSound()
    {
        if (healthPickupClip != null)
        {
            //Debug.Log($"Playing health pickup sound: {healthPickupClip.name}, volume={effectsVolume}, isPlaying={pickupSource.isPlaying}");
            pickupSource.PlayOneShot(healthPickupClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("healthPickupClip is null in PickupSound!");
        }
    }

    public void flagPickupSound()
    {
        if (flagPickupClip != null)
        {
            //Debug.Log($"Playing flag pickup sound: {flagPickupClip.name}, volume={effectsVolume}, isPlaying={pickupSource.isPlaying}");
            pickupSource.PlayOneShot(flagPickupClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("flagPickupClip is null in PickupSound!");
        }
    }

    public void oilPickupSound()
    {
        if(oilPickupClip != null)
        {
            //Debug.Log($"Playing flag pickup sound: {flagPickupClip.name}, volume={effectsVolume}, isPlaying={pickupSource.isPlaying}");
            pickupSource.PlayOneShot(oilPickupClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("oilPickupClip is null in PickupSound!");
        }
    }

    public void biblePickupSound()
    {
        if (biblePickupClip != null)
        {
            //Debug.Log($"Playing flag pickup sound: {flagPickupClip.name}, volume={effectsVolume}, isPlaying={pickupSource.isPlaying}");
            pickupSource.PlayOneShot(biblePickupClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("biblePickupClip is null in PickupSound!");
        }
    }

    // Sound effect methods
    public void ShootSound()
    {
        Play(shootClip);
    }

    public void missileSound()
    {
        Play(missileClip);
    }

    public void enemyBossShootSound()
    {
        Play(bossShootClip);
    }

    public void tronShootSound()
    {
        Play(tronShootClip);
    }
    public void buttonSound()
    {
        Play(buttonClip);
    }
    public void HurtSound()
    {
        Play(hurtClip);
    }

    public void ExplodeSound()
    {
        Play(explodeClip);
    }

    public void PlayerMoveSound()
    {
        PlayMovementSound(playerMoveClip);
    }

    public void EnemyMoveSound()
    {
        PlayMovementSound(enemyMoveClip);
    }

    public void bossEnemyMoveSound()
    {
        PlayMovementSound(bossEnemyMoveClip);
    }
    public void animeMoveSound()
    {
        PlayMovementSound(animeMoveClip);
    }
    public void demonMoveSound()
    {
        PlayMovementSound(demonMoveClip);
    }
    public void tronMoveSound()
    {
        PlayMovementSound(tronMoveClip);
    }
    // Music methods
    public void MenuMusic()
    {
        PlayMusic(menuMusicClip);
    }

    public void BattleMusic()
    {
        PlayMusic(battleMusicClip);
    }

    public void finalBattleMusic()
    {
        PlayMusic(finalBattleMusicClip);
    }

    public void hellMusic()
    {
        PlayMusic(hellMusicClip);
    }

    public void technoMusic()
    {
        PlayMusic(technoMusicClip);
    }

    public void animeMusic()
    {
        PlayMusic(animeMusicClip);
    }

    public void desertMusic()
    {
        PlayMusic(desertMusicClip);
    }

    public void winterMusic()
    {
        PlayMusic(winterMusicClip);
    }

    public void PauseMusic()
    {
        PlayMusic(pauseMusicClip);
    }

    public void WinMusic()
    {
        PlayMusic(winMusicClip);
    }

    public void LoseMusic()
    {
        PlayMusic(loseMusicClip);
    }
}