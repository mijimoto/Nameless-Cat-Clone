using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemController : MonoBehaviour
{
	[SerializeField]
	private Color completedColor;

	[SerializeField]
	private Color lockedColor;

	[SerializeField]
	private Image bgImage;

	[SerializeField]
	private Image iconImage;

	[SerializeField]
	private Image lockImage;

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private TextMeshProUGUI descriptionText;

	[SerializeField]
	private TextMeshProUGUI completeDateText;

	[SerializeField]
	private TextMeshProUGUI progressText;

	private AchievementProgress achievementProgress;

	private void Awake()
	{
	}

	public void UpdateTextSetting()
	{
	}

	public void Init(AchievementProgress progress)
	{
	}

	public void UpdateProgress()
	{
	}
}
