using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    private SoundManager soundManager;

    public Image musicBtnImg;
    public Image soundBtnImg;
    public Sprite[] musicBtnSprites;
    public Sprite[] soundBtnSprites;
    public GameObject restorePurcahseButton;

    private void Start()
    {
        soundManager = SoundManager._instance;
        
        // Initialize button images based on current settings
        updateMusicImage();
        updateSoundImage();
        
        // Show/hide restore purchase button based on platform
        if (restorePurcahseButton != null)
        {
#if UNITY_IOS
            restorePurcahseButton.SetActive(true);
#else
            restorePurcahseButton.SetActive(false);
#endif
        }
    }

    public void toggleMusicBtn()
    {
        if (soundManager != null)
        {
            soundManager.toggleMusic();
            updateMusicImage();
            
            // Play UI sound feedback
            soundManager.uiSound("click");
        }
    }

    private void updateMusicImage()
    {
        if (musicBtnImg != null && musicBtnSprites != null && musicBtnSprites.Length >= 2)
        {
            bool musicOn = soundManager != null ? soundManager.musicOn : true;
            musicBtnImg.sprite = musicOn ? musicBtnSprites[0] : musicBtnSprites[1];
        }
    }

    public void toggleSoundBtn()
    {
        if (soundManager != null)
        {
            // Play sound before toggling (so user can hear the click)
            soundManager.uiSound("click");
            soundManager.toggleSound();
            updateSoundImage();
        }
    }

    private void updateSoundImage()
    {
        if (soundBtnImg != null && soundBtnSprites != null && soundBtnSprites.Length >= 2)
        {
            bool soundOn = soundManager != null ? soundManager.soundOn : true;
            soundBtnImg.sprite = soundOn ? soundBtnSprites[0] : soundBtnSprites[1];
        }
    }

    public void RestorePurcahse()
    {
#if UNITY_IOS
        // Play UI sound
        if (soundManager != null)
            soundManager.uiSound("click");
            
        // iOS specific restore purchases logic
        try
        {
            // This would typically integrate with Unity IAP or iOS Store Kit
            // For now, we'll simulate the restore process
            RestorePurchasesSuccess();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Restore purchase failed: " + e.Message);
            RestorePurchasesFail();
        }
#endif
    }

    private void RestorePurchasesSuccess()
    {
        Debug.Log("Purchases restored successfully!");
        
        // You might want to show a success message to the user
        // This could involve updating UI elements or showing a popup
        
        // Refresh any premium content or features
        // GameManager.Instance?.RefreshPremiumContent();
    }

    private void RestorePurchasesFail()
    {
        Debug.Log("Failed to restore purchases.");
        
        // You might want to show an error message to the user
        // This could involve showing an error popup or message
    }
}