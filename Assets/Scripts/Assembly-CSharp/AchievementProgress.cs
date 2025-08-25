using System;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class AchievementProgress : ScriptableObject
{
	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private float targetProgress;

	protected float progress;

	protected DateTime completeDate;

	protected bool completed;

	protected string title;

	protected string description;

	protected int index;

	protected string id;

	protected string titleKey;

	protected string descriptionKey;

	protected AchievementProgressReference outerReference;

	public string ProgressId => null;

	public Sprite Icon => null;

	public string Title => null;

	public string Description => null;

	public float Progress => 0f;

	public float TargetProgress => 0f;

	public DateTime CompleteDate => default(DateTime);

	public bool Completed => false;

	public virtual void Init(int index, string id, string titleKey, string descriptionKey, AchievementProgressReference outerReference)
	{
	}

	public void UpdateLocalization()
	{
	}

	public virtual void LoadProgress()
	{
	}

	public virtual void SaveProgress()
	{
	}

	protected void saveDate()
	{
	}

	public virtual void SetProgress(float amount)
	{
	}

	public virtual void AddProgress(float amount)
	{
	}

	protected virtual void UpdateCompleted()
	{
	}
}
