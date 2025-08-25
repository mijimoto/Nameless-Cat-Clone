using UnityEngine;

[CreateAssetMenu]
[SerializeField]
internal class AchievementProgressReferenceOuterKey : AchievementProgressReference
{
	[SerializeField]
	private string outerReferenceKey;

	[SerializeField]
	private bool outerReferenceEmbedIndex;

	public void Init(int index)
	{
	}

	public override float LoadProgress()
	{
		return 0f;
	}
}
