using System.Collections.Generic;
using UnityEngine;

public class ProtalDebugger : MonoBehaviour
{
	[SerializeField]
	private GameObject boss3Portal;

	[SerializeField]
	private GameObject smallPortalPrefab;

	[SerializeField]
	private Transform smallPortalHolder;

	[SerializeField]
	private Vector2 portalGenerateRange;

	private Queue<GameObject> portals;

	private bool enabledBoss3Portal;

	private void OnDrawGizmos()
	{
	}

	private void Start()
	{
	}

	public void ToggleBoss3Portal()
	{
	}

	public void CreatePortal()
	{
	}

	public void RemovePortal()
	{
	}
}
