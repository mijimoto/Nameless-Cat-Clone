using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class AchievementData : ScriptableObject
{
	[SerializeField]
	protected string id;

	[SerializeField]
	protected string titleKey;

	[SerializeField]
	protected string descriptionKey;

	[SerializeField]
	private AchievementProgressReference outerReference;

	[SerializeField]
	protected AchievementProgress[] progresses;

	public AchievementProgress[] Progresses => null;

	public string Id => null;

	public void Init()
	{
	}

	public void UpdateLocaliztion()
	{
	}

	public void LoadProgress()
	{
	}

	public void SaveProgress()
	{
	}

	public void AddProgress(float amount)
	{
	}
}
