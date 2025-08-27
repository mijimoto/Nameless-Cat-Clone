using UnityEngine;

public class Item : MonoBehaviour
{
	public enum FishState
	{
		NotGet = 0,
		Got = 1,
		FirstGet = 2,
		GetAgain = 3
	}

	public static bool itemCollected;
	public static bool firstCollect;

	public bool hideOnStart;
	public bool noResetItem;

	private void Start()
	{
		string levelKey = "Fish_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + transform.position.x + "_" + transform.position.y;
		bool wasCollected = PlayerPrefs.GetInt(levelKey, 0) == 1;
		
		if (wasCollected && !noResetItem)
		{
			if (hideOnStart)
			{
				gameObject.SetActive(false);
			}
			else
			{
				SpriteRenderer sr = GetComponent<SpriteRenderer>();
				if (sr != null)
				{
					Color color = sr.color;
					color.a = 0.3f;
					sr.color = color;
				}
				GetComponent<Collider2D>().enabled = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			string levelKey = "Fish_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + transform.position.x + "_" + transform.position.y;
			bool wasAlreadyCollected = PlayerPrefs.GetInt(levelKey, 0) == 1;
			
			if (!wasAlreadyCollected)
			{
				firstCollect = true;
				itemCollected = true;
				
				PlayerPrefs.SetInt(levelKey, 1);
				PlayerPrefs.Save();
				
				int totalFish = PlayerPrefs.GetInt("TotalFish", 0);
				totalFish++;
				PlayerPrefs.SetInt("TotalFish", totalFish);
				PlayerPrefs.Save();
				
				if (UIManager._instance != null)
				{
					UIManager._instance.getBoxEffect(transform.position);
				}
			}
			else
			{
				firstCollect = false;
				itemCollected = false;
			}
			
			destoryObj();
		}
	}

	private void destoryObj()
	{
		Destroy(gameObject);
	}

	public static FishState GetFishState()
	{
		if (firstCollect && itemCollected)
		{
			return FishState.FirstGet;
		}
		else if (!firstCollect && itemCollected)
		{
			return FishState.GetAgain;
		}
		else if (itemCollected)
		{
			return FishState.Got;
		}
		else
		{
			return FishState.NotGet;
		}
	}
}