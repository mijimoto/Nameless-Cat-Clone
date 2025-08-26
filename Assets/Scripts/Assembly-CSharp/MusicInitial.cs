using UnityEngine;

public class MusicInitial : MonoBehaviour
{
    public static MusicInitial _instance;
    
    [SerializeField]
    private AudioClip beforeBattleMusic;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void playPreMusic()
    {
        if (beforeBattleMusic != null && MusicPlayer._instance != null)
        {
            MusicPlayer._instance.playMusic(beforeBattleMusic);
        }
    }
    
    public void active(bool b)
    {
        gameObject.SetActive(b);
        
        if (b && beforeBattleMusic != null)
        {
            playPreMusic();
        }
    }
}