using System.Collections;
using UnityEngine;

public class Collection : MonoBehaviour
{
    private const float TOTAL_ANIMATION_SEC = 1f;

    [SerializeField]
    private AudioClip collectionSound;

    [SerializeField]
    private Sprite collectionPhoto;

    [SerializeField]
    private int amountOfStar;

    [SerializeField]
    private bool soundControll;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float minDistance;

    [SerializeField]
    private float maxDistance;

    private float baseVolume;

    [SerializeField]
    private GameObject memoryStar;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private ParticleSystem particleSystemNormal;

    [SerializeField]
    private ParticleSystem particleSystemDissolve;

    [SerializeField]
    private Animator dissolveAnimator;

    [SerializeField]
    private float dissolveAmount;

    private float lastDissolveAmount;

    private Vector3 origialPos;

    private Material material;

    private Transform player;

    private bool collected;

    private string chapter;

    private SoundManager soundManager;

    private MusicPlayer musicPlayer;

    private void Awake()
    {
        soundManager = SoundManager._instance;
        musicPlayer = MusicPlayer._instance;
        
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
        
        if (audioSource != null)
        {
            baseVolume = audioSource.volume;
        }
        
        origialPos = transform.position;
        
        // Get chapter name from scene or parent object
        chapter = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // Check if already collected
        if (hasCollected(chapter))
        {
            collected = true;
            gameObject.SetActive(false);
        }
        
        // Initialize particle systems
        if (particleSystemNormal != null)
            particleSystemNormal.Stop();
        if (particleSystemDissolve != null)
            particleSystemDissolve.Stop();
    }

    private void Update()
    {
        // Update dissolve effect
        if (material != null && dissolveAmount != lastDissolveAmount)
        {
            material.SetFloat("_DissolveAmount", dissolveAmount);
            lastDissolveAmount = dissolveAmount;
        }
        
        // Update audio volume based on distance if sound control is enabled
        if (soundControll && audioSource != null && player != null)
        {
            float volume = SoundManager.volumeBetweenObj(transform.position, player.position, minDistance, maxDistance);
            audioSource.volume = baseVolume * volume;
        }
    }

    public static bool hasCollected(string chapter)
    {
        return PlayerPrefs.GetInt("Collection_" + chapter, 0) == 1;
    }

    public static bool hasCollected(int chapter)
    {
        return hasCollected(chapter.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;
            
        collected = true;
        
        // Save collection state
        PlayerPrefs.SetInt("Collection_" + chapter, 1);
        PlayerPrefs.Save();
        
        // Play collection sound
        if (soundManager != null)
        {
            if (collectionSound != null)
                soundManager.playAudioClip(collectionSound, 1f);
            else
                soundManager.effect("collect");
        }
        
        // Start collection animation
        StartCoroutine(collectionAnimation());
    }

    private IEnumerator collectionAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * 2f;
        
        // Start particle effects
        if (particleSystemNormal != null)
            particleSystemNormal.Play();
            
        // Animate collection
        while (elapsedTime < TOTAL_ANIMATION_SEC)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / TOTAL_ANIMATION_SEC;
            
            // Move upward with easing
            transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, progress));
            
            // Fade out
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, progress);
                spriteRenderer.color = color;
            }
            
            // Scale effect
            float scale = Mathf.Lerp(1f, 1.5f, Mathf.Sin(progress * Mathf.PI));
            transform.localScale = Vector3.one * scale;
            
            yield return null;
        }
        
        // Trigger dissolve effect if available
        if (dissolveAnimator != null)
        {
            dissolveAnimator.SetTrigger("Dissolve");
        }
        else
        {
            animationEnd();
        }
    }

    public void storyEnd()
    {
        // Called when story sequence ends
        if (particleSystemDissolve != null)
            particleSystemDissolve.Play();
            
        // Trigger final dissolve
        if (dissolveAnimator != null)
            dissolveAnimator.SetTrigger("StoryEnd");
    }

    public void animationEnd()
    {
        // Called when animation completes (usually from Animation Event)
        
        // Stop all particle systems
        if (particleSystemNormal != null)
            particleSystemNormal.Stop();
        if (particleSystemDissolve != null)
            particleSystemDissolve.Stop();
            
        // Spawn memory star if configured
        if (memoryStar != null)
        {
            GameObject star = Instantiate(memoryStar, origialPos, Quaternion.identity);
            // You might want to add the star to a parent container
        }
        
        // Play memory star sound
        if (soundManager != null)
            soundManager.effect("memorystar");
            
        // Disable the collection object
        gameObject.SetActive(false);
    }
}