using UnityEngine;

public class CollectionMenu : MonoBehaviour
{
	public GameObject holder;

	[SerializeField]
	private RectTransform pageHolder;

	[SerializeField]
	private RectTransform collectionHolder;

	[SerializeField]
	private Transform collectionIconHolder;

	[SerializeField]
	private Transform underBar;

	[SerializeField]
	private RectTransform changePageButtons;

	private float lastMoveRate;

	private int currentPage;

	private int currentCategory;

	private int[] categoryLastPage;

	private int lastPage;

	private void Start()
	{
	}

	public void init()
	{
	}

	public void updateCollectionPos()
	{
	}

	private void updateCategory()
	{
	}

	public void changePage(bool left)
	{
	}

	public void goCategory(int category)
	{
	}

	private void updateChapterButton()
	{
	}
}
