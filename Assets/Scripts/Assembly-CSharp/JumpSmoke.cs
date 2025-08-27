using UnityEngine;

public class JumpSmoke : MonoBehaviour
{
	private Animator[] smokes;
	private int spawnPivot;

	private void Start()
	{
		smokes = GetComponentsInChildren<Animator>();
		spawnPivot = 0;
	}

	public void spawnSmoke()
	{
		if (smokes != null && smokes.Length > 0)
		{
			smokes[spawnPivot].SetTrigger("smoke");
			spawnPivot = (spawnPivot + 1) % smokes.Length;
		}
	}
}