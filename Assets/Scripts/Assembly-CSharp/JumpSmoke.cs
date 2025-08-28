using UnityEngine;

public class JumpSmoke : MonoBehaviour
{
	private Animator[] smokes;
	private int spawnPivot;

	private void Start()
	{
		smokes = GetComponentsInChildren<Animator>();
	}

public void spawnSmoke()
{
    if (smokes != null && smokes.Length > 0)
    {
        for (int i = 0; i < smokes.Length; i++)
        {
            smokes[i].SetTrigger("JumpSmokeP"); // all child animators will try to trigger
            Debug.Log("Triggering smoke: " + smokes[i].name);
        }
    }
}

}