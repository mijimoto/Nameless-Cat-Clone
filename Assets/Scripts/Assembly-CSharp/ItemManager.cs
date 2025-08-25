using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public class SkinManager
	{
		public int count;

		[SerializeField]
		public Skin[] skins;

		public string[] skinName;

		public SkinManager(Skin[] skins, string language)
		{
		}

		private int shift(int start, int shiftIndex)
		{
			return 0;
		}

		public void updateSkinByShift(int shiftIndex)
		{
		}

		public int getSkinIndexByShift(int shiftIndex)
		{
			return 0;
		}

		public Skin getSkinDataByShift(int shiftIndex)
		{
			return null;
		}
	}

	public static ItemManager _instance;

	private int currentSkin;

	[SerializeField]
	public Skin[] skins;

	private Skin[] sourceSkins;

	public static SkinManager skinData;

	private int currentBox;

	private int collectedBox;

	private void Awake()
	{
	}

	public void OrderSkins(Skin[] skins)
	{
	}

	public void loadSkin()
	{
	}

	public int getCurrentSkin()
	{
		return 0;
	}

	public Skin getCurrentSkinData()
	{
		return null;
	}

	public Skin getSkinById(int id)
	{
		return null;
	}

	public bool skinCanBuy(int i = -1, bool IAP = false)
	{
		return false;
	}

	public void selectSkin(int i)
	{
	}

	public void updateSkinData()
	{
	}

	public void loadSkin(Animator playerAnimator)
	{
	}

	public void addBox(string level)
	{
	}

	public void removeBox(int amount)
	{
	}

	public int getCurrentBox()
	{
		return 0;
	}

	public void Reset()
	{
	}
}
