using UnityEngine;

[CreateAssetMenu]
[SerializeField]
internal class AchievementProgressReferenceSkin : AchievementProgressReference
{
	public override float LoadProgress()
	{
		return 0f;
	}
}
