using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer _instance;

    [Header("Music Clips")]
    public AudioClip titleMusic;
    public AudioClip[] battleMusic;
    public AudioClip[] preBattleMusic;
    public AudioClip[] gameMusic;

    [Header("Volume Settings")]
    [SerializeField] private float fadeSpeed = 1.0f;
    [SerializeField] private string volumePrefsKey = "MusicVolume";

    private AudioSource aui;
    private float baseVolume = 1.0f;
    private float extraVolumeModifier = 1.0f;

    private int targetMusic = -1;
    private AudioClip targetMusicClip;
    private float currentSmoothVolume;
    private float smoothTargetVolume = 1.0f;
    private float smoothSide = 1.0f;
    private bool musicUp = true;
    private AudioClip nextMusic;

    // Smooth transition states
    private bool isFading = false;
    private bool isChangingMusic = false;
    private float fadeDirection = 0f; // -1 for fade out, 1 for fade in

    public bool HasMusic => aui != null && aui.clip != null;
    public bool HasNextMusic => nextMusic != null;

    private void Awake()
    {
        // Singleton pattern
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize audio source
            aui = GetComponent<AudioSource>();
            if (aui == null)
            {
                aui = gameObject.AddComponent<AudioSource>();
            }
            
            // Configure audio source
            aui.loop = true;
            aui.playOnAwake = false;
            
            // Load volume settings
            baseVolume = PlayerPrefs.GetFloat(volumePrefsKey, 1.0f);
            currentSmoothVolume = baseVolume;
            updateVolme();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleSmoothVolumeTransitions();
    }

    private void HandleSmoothVolumeTransitions()
    {
        if (!isFading && !isChangingMusic)
            return;

        // Handle volume fading
        if (isFading)
        {
            currentSmoothVolume = Mathf.MoveTowards(currentSmoothVolume, smoothTargetVolume, 
                fadeSpeed * Time.unscaledDeltaTime);
            
            updateVolme();
            
            // Check if fade is complete
            if (Mathf.Approximately(currentSmoothVolume, smoothTargetVolume))
            {
                isFading = false;
                
                // If we were fading out to change music, now start the new music
                if (isChangingMusic && smoothTargetVolume <= 0f)
                {
                    if (targetMusicClip != null)
                    {
                        aui.clip = targetMusicClip;
                        aui.Play();
                        startFadeIn();
                    }
                    isChangingMusic = false;
                }
            }
        }
    }

    public void changeMusic(int musicIndex, int chapter = 1, bool play = true)
    {
        AudioClip newMusic = getMusic(musicIndex, chapter);
        
        if (newMusic == null)
        {
            Debug.LogWarning($"No music found for index {musicIndex}, chapter {chapter}");
            return;
        }

        targetMusic = musicIndex;
        
        if (play)
        {
            playMusic(newMusic);
        }
        else
        {
            aui.clip = newMusic;
        }
    }

    public void setNextMusic(int musicIndex, int chapter = 1)
    {
        nextMusic = getMusic(musicIndex, chapter);
        targetMusic = musicIndex;
    }

    public void playMusic(AudioClip music)
    {
        if (music == null)
        {
            Debug.LogWarning("Trying to play null music clip");
            return;
        }

        // If same music is already playing, don't restart
        if (aui.clip == music && aui.isPlaying)
            return;

        aui.clip = music;
        aui.Play();
        
        // Reset volume to current settings
        currentSmoothVolume = baseVolume * extraVolumeModifier;
        updateVolme();
    }

    private AudioClip getMusic(int musicIndex, int chapter)
    {
        switch (musicIndex)
        {
            case 0: // Title music
                return titleMusic;
                
            case 1: // Battle music
                if (battleMusic != null && battleMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, battleMusic.Length - 1);
                    return battleMusic[index];
                }
                break;
                
            case 2: // Pre-battle music
                if (preBattleMusic != null && preBattleMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, preBattleMusic.Length - 1);
                    return preBattleMusic[index];
                }
                break;
                
            case 3: // Game music
                if (gameMusic != null && gameMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, gameMusic.Length - 1);
                    return gameMusic[index];
                }
                break;
                
            default:
                Debug.LogWarning($"Unknown music index: {musicIndex}");
                break;
        }
        
        return null;
    }

    public void pauseMusic(bool pause)
    {
        if (aui == null) return;

        if (pause)
        {
            if (aui.isPlaying)
                aui.Pause();
        }
        else
        {
            if (!aui.isPlaying && aui.clip != null)
                aui.UnPause();
        }
    }

    public void updateVolme()
    {
        if (aui != null)
        {
            float finalVolume = baseVolume * extraVolumeModifier * currentSmoothVolume;
            aui.volume = Mathf.Clamp01(finalVolume);
        }
    }

    public void SetExtraVolumeModifier(float extraVolumeModifier)
    {
        this.extraVolumeModifier = Mathf.Clamp01(extraVolumeModifier);
        updateVolme();
    }

    public void smoothChangeMusic(int musicIndex = -1)
    {
        // If no specific music index provided, use next music if available
        if (musicIndex == -1 && HasNextMusic)
        {
            smoothChangeMusic(nextMusic);
            nextMusic = null;
            return;
        }

        // Get the music clip for the specified index
        AudioClip newMusic = getMusic(musicIndex, 1); // Default to chapter 1
        if (newMusic != null)
        {
            smoothChangeMusic(newMusic);
        }
    }

    public void smoothChangeMusic(AudioClip musicClip)
    {
        if (musicClip == null)
        {
            Debug.LogWarning("Trying to smoothly change to null music clip");
            return;
        }

        // If same music is already playing, don't change
        if (aui.clip == musicClip && aui.isPlaying)
            return;

        targetMusicClip = musicClip;
        
        // If no music is currently playing, just start the new one
        if (!HasMusic || !aui.isPlaying)
        {
            playMusic(musicClip);
            return;
        }

        // Start smooth transition: fade out current, then fade in new
        isChangingMusic = true;
        startFadeOut(0f);
    }

    public void startFadeIn(float targetVolume = 1f)
    {
        smoothTargetVolume = Mathf.Clamp01(targetVolume);
        fadeDirection = 1f;
        isFading = true;
        musicUp = true;
    }

    public void startFadeOut(float targetVolume = 0f)
    {
        smoothTargetVolume = Mathf.Clamp01(targetVolume);
        fadeDirection = -1f;
        isFading = true;
        musicUp = false;
    }

    public void ResetSmooth()
    {
        isFading = false;
        isChangingMusic = false;
        currentSmoothVolume = baseVolume * extraVolumeModifier;
        smoothTargetVolume = currentSmoothVolume;
        updateVolme();
    }

    // Public methods for external control
    public void SetMusicVolume(float volume)
    {
        baseVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(volumePrefsKey, baseVolume);
        PlayerPrefs.Save();
        updateVolme();
    }

    public float GetMusicVolume()
    {
        return baseVolume;
    }

    public void StopMusic()
    {
        if (aui != null && aui.isPlaying)
        {
            aui.Stop();
        }
        ResetSmooth();
    }

    public void StopMusicSmooth()
    {
        startFadeOut(0f);
    }

    public bool IsPlaying()
    {
        return aui != null && aui.isPlaying;
    }

    public string GetCurrentMusicName()
    {
        if (HasMusic)
        {
            return aui.clip.name;
        }
        return "None";
    }

    // Called when the application is paused (mobile)
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            pauseMusic(true);
        }
        else
        {
            pauseMusic(false);
        }
    }

    // Called when the application loses focus
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            pauseMusic(true);
        }
        else
        {
            pauseMusic(false);
        }
    }
}