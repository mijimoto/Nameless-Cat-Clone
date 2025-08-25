using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinBlock : MonoBehaviour
{
	[SerializeField]
	private Image catImage;

	[SerializeField]
	private Image bgImage;

	[SerializeField]
	private GameObject priceCanObj;

	[SerializeField]
	private GameObject priceIAPObj;

	[SerializeField]
	private TextMeshProUGUI priceCanText;

	[SerializeField]
	private TextMeshProUGUI priceIAPText;

	private Skin skinData;

	public void LoadSkin(Skin skinData)
	{
	}
}
