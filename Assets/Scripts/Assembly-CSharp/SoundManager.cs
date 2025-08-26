using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    
    [Header("Sound Effects")]
    public AudioClip onGroundSound;
    public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip dieSound;
    public AudioClip winSound;
    public AudioClip memoryStarSound;
    public AudioClip checkPointSound;
    public AudioClip godLeaveSound;
    public AudioClip boomSound;
    public AudioClip yellSound;
    public AudioClip collectSound;
    public AudioClip mushroomSound;
    public AudioClip attachInSound;
    public AudioClip attachOutSound;
    public AudioClip pressurePlateSound;
    public AudioClip[] shootSound;
    public AudioClip clickSound;
    
    private AudioSource aui;
    public bool musicOn = true;
    public bool soundOn = true;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            aui = GetComponent<AudioSource>();
            if (aui == null)
                aui = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load sound settings from PlayerPrefs
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
    }

    public void playAudioClip(AudioClip audioClip, float sound)
    {
        if (!soundOn || audioClip == null) return;
        
        aui.PlayOneShot(audioClip, sound);
    }

    public void playCharacterSound(string soundName)
    {
        if (!soundOn) return;
        
        AudioClip clip = getClipByName(soundName);
        if (clip != null)
        {
            aui.PlayOneShot(clip);
        }
    }

    public static float volumeBetweenObj(Transform obj1, Transform obj2, float minDis = 3.5f, float maxDis = 4f)
    {
        if (obj1 == null || obj2 == null) return 0f;
        
        float distance = Vector3.Distance(obj1.position, obj2.position);
        return volumeBetweenObj(obj1.position, obj2.position, minDis, maxDis);
    }

    public static float volumeBetweenObj(Vector3 pos1, Vector3 pos2, float minDis = 3.5f, float maxDis = 4f)
    {
        float distance = Vector3.Distance(pos1, pos2);
        
        if (distance <= minDis)
            return 1f;
        else if (distance >= maxDis)
            return 0f;
        else
            return 1f - (distance - minDis) / (maxDis - minDis);
    }

    public void effectByDistance(string soundName, Transform obj1, Transform obj2, int index = -1)
    {
        if (!soundOn || obj1 == null || obj2 == null) return;
        
        float volume = volumeBetweenObj(obj1, obj2);
        if (volume > 0f)
        {
            effect(soundName, index, volume);
        }
    }

    public void effect(string soundName, int index = -1, float volumeMultiper = 1f)
    {
        if (!soundOn) return;
        
        AudioClip clip = getClipByName(soundName, index);
        if (clip != null)
        {
            aui.PlayOneShot(clip, volumeMultiper);
        }
    }

    public void objectSound(string soundName, int index = -1, float volumeMultiper = 1f)
    {
        effect(soundName, index, volumeMultiper);
    }

    public void uiSound(string soundName)
    {
        if (!soundOn) return;
        
        AudioClip clip = getClipByName(soundName);
        if (clip != null)
        {
            aui.PlayOneShot(clip);
        }
    }

    public void toggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.Save();
        
        if (MusicPlayer._instance != null)
        {
            MusicPlayer._instance.updateVolme();
        }
    }

    public void toggleSound()
    {
        soundOn = !soundOn;
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void setMusic(bool b)
    {
        musicOn = b;
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.Save();
        
        if (MusicPlayer._instance != null)
        {
            MusicPlayer._instance.updateVolme();
        }
    }

    public void setSound(bool b)
    {
        soundOn = b;
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private AudioClip getClipByName(string soundName, int index = -1)
    {
        switch (soundName.ToLower())
        {
            case "onground":
                return onGroundSound;
            case "move":
                return moveSound;
            case "jump":
                return jumpSound;
            case "die":
                return dieSound;
            case "win":
                return winSound;
            case "memorystar":
                return memoryStarSound;
            case "checkpoint":
                return checkPointSound;
            case "godleave":
                return godLeaveSound;
            case "boom":
                return boomSound;
            case "yell":
                return yellSound;
            case "collect":
                return collectSound;
            case "mushroom":
                return mushroomSound;
            case "attachin":
                return attachInSound;
            case "attachout":
                return attachOutSound;
            case "pressureplate":
                return pressurePlateSound;
            case "shoot":
                if (shootSound != null && shootSound.Length > 0)
                {
                    if (index >= 0 && index < shootSound.Length)
                        return shootSound[index];
                    else
                        return shootSound[Random.Range(0, shootSound.Length)];
                }
                break;
            case "click":
                return clickSound;
        }
        return null;
    }
}