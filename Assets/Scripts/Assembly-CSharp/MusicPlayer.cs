using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer _instance;
    
    [Header("Music Clips")]
    public AudioClip titleMusic;
    public AudioClip[] battleMusic;
    public AudioClip[] preBattleMusic;
    public AudioClip[] gameMusic;
    
    private AudioSource aui;
    private float baseVolume;
    private float extraVolumeModifier;
    private int targetMusic = -1;
    private AudioClip targetMusicClip;
    private float currentSmoothVolume = 1f;
    private float smoothTargetVolume = 1f;
    private float smoothSide = 1f;
    private bool musicUp = true;
    private AudioClip nextMusic;
    
    public bool HasMusic => aui != null && aui.clip != null && aui.isPlaying;
    public bool HasNextMusic => nextMusic != null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            aui = GetComponent<AudioSource>();
            if (aui == null)
                aui = gameObject.AddComponent<AudioSource>();
            
            aui.loop = true;
            aui.playOnAwake = false;
            baseVolume = 0.5f;
            extraVolumeModifier = 1f;
            aui.volume = baseVolume;
            currentSmoothVolume = baseVolume;
            smoothTargetVolume = baseVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Handle smooth volume transitions
        if (Mathf.Abs(currentSmoothVolume - smoothTargetVolume) > 0.01f)
        {
            currentSmoothVolume = Mathf.Lerp(currentSmoothVolume, smoothTargetVolume, Time.deltaTime * 2f * smoothSide);
            updateVolme();
            
            // Check if we need to change music during fade
            if (!musicUp && currentSmoothVolume <= 0.05f && targetMusicClip != null)
            {
                aui.clip = targetMusicClip;
                if (targetMusicClip != null)
                {
                    aui.Play();
                }
                targetMusicClip = null;
                musicUp = true;
                smoothTargetVolume = baseVolume;
            }
            else if (!musicUp && currentSmoothVolume <= 0.05f && nextMusic != null)
            {
                aui.clip = nextMusic;
                if (nextMusic != null)
                {
                    aui.Play();
                }
                nextMusic = null;
                musicUp = true;
                smoothTargetVolume = baseVolume;
            }
        }
    }

    public void changeMusic(int musicIndex, int chapter = 1, bool play = true)
    {
        AudioClip music = getMusic(musicIndex, chapter);
        if (music != null)
        {
            if (play)
            {
                playMusic(music);
            }
            else
            {
                aui.clip = music;
            }
        }
        targetMusic = musicIndex;
    }

    public void setNextMusic(int musicIndex, int chapter = 1)
    {
        nextMusic = getMusic(musicIndex, chapter);
        targetMusic = musicIndex;
    }

    public void playMusic(AudioClip music)
    {
        if (music == null || !SoundManager._instance.musicOn) return;
        
        aui.clip = music;
        aui.Play();
        ResetSmooth();
        updateVolme();
    }

    private AudioClip getMusic(int musicIndex, int chapter)
    {
        switch (musicIndex)
        {
            case 0: // Title
                return titleMusic;
            case 1: // Game Music
                if (gameMusic != null && gameMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, gameMusic.Length - 1);
                    return gameMusic[index];
                }
                break;
            case 2: // Pre-Battle
                if (preBattleMusic != null && preBattleMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, preBattleMusic.Length - 1);
                    return preBattleMusic[index];
                }
                break;
            case 3: // Battle
                if (battleMusic != null && battleMusic.Length > 0)
                {
                    int index = Mathf.Clamp(chapter - 1, 0, battleMusic.Length - 1);
                    return battleMusic[index];
                }
                break;
        }
        return null;
    }

    public void pauseMusic(bool b)
    {
        if (aui == null) return;
        
        if (b)
            aui.Pause();
        else
            aui.UnPause();
    }

    public void updateVolme()
    {
        if (aui == null) return;
        
        float finalVolume = SoundManager._instance.musicOn ? 
            (baseVolume * currentSmoothVolume * extraVolumeModifier) : 0f;
        aui.volume = finalVolume;
    }

    public void SetExtraVolumeModifier(float extraVolumeModifier)
    {
        this.extraVolumeModifier = extraVolumeModifier;
        updateVolme();
    }

    public void smoothChangeMusic(int musicIndex = -1)
    {
        if (musicIndex != -1)
        {
            targetMusicClip = getMusic(musicIndex, 1);
            targetMusic = musicIndex;
        }
        
        startFadeOut(0f);
        musicUp = false;
        smoothSide = 2f;
    }

    public void smoothChangeMusic(AudioClip musicClip)
    {
        targetMusicClip = musicClip;
        startFadeOut(0f);
        musicUp = false;
        smoothSide = 2f;
    }

    public void startFadeIn(float targetVolume = 1f)
    {
        smoothTargetVolume = targetVolume;
        musicUp = true;
        smoothSide = 1f;
    }

    public void startFadeOut(float targetVolume = 0f)
    {
        smoothTargetVolume = targetVolume;
        musicUp = false;
        smoothSide = 2f;
    }

    public void ResetSmooth()
    {
        currentSmoothVolume = baseVolume;
        smoothTargetVolume = baseVolume;
        musicUp = true;
        smoothSide = 1f;
    }
}