using UnityEngine;

public class PopupMessage : MonoBehaviour
{
	[SerializeField]
	private LocalizationText textLocalization;

	public void Init(string key)
	{
		if (textLocalization != null)
		{
			textLocalization.SetKey(key);
		}
		
		// Auto-close after 3 seconds
		Invoke("Close", 3f);
	}

	public void Close()
	{
		Destroy(gameObject);
	}
}