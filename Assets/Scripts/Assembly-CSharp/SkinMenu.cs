using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : MonoBehaviour
{
	private const float offset = 47f;

	public GameObject holder;

	public Button useButton;

	public Button buyButton;

	public TextMeshProUGUI buyButtonText;

	public Button buyRealButton;

	public TextMeshProUGUI buyRealButtonText;

	public Transform skinsHolder;

	private int currentSkinBox;

	private int maxSkinBox;

	private float offsetCount;

	private SkinBlock[] skinBoxs;

	public GameObject loadingObj;

	private bool loading;

	private void Start()
	{
	}

	private void initSkin()
	{
	}

	public void updateSkinPos()
	{
	}

	public void updateAllSkinUI()
	{
	}

	public void updateSkinUI()
	{
	}

	public void updateSkinUI(int skinBoxIndex, int shiftIndex)
	{
	}

	public void buySkinWithBox()
	{
	}

	private void addSkin(Skin skinData)
	{
	}

	public void buySkin()
	{
	}

	private void LoadAndLock(bool startLoad)
	{
	}

	public void finishBuy(bool success = false)
	{
	}
}
