using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class Skin : ScriptableObject
{
	[SerializeField]
	public int id;

	[SerializeField]
	public bool useIAP;

	[SerializeField]
	public bool canBuy;

	[SerializeField]
	public bool canUse;

	[SerializeField]
	public RuntimeAnimatorController controller;

	[SerializeField]
	public Sprite sprite;

	[SerializeField]
	public int price;

	public void setCanUse(bool able)
	{
	}
}
