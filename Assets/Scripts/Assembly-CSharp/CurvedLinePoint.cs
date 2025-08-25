using UnityEngine;

public class CurvedLinePoint : MonoBehaviour
{
	[HideInInspector]
	public bool showGizmo;

	[HideInInspector]
	public float gizmoSize;

	[HideInInspector]
	public Color gizmoColor;

	private void OnDrawGizmos()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}
}
