using UnityEngine;

public abstract class InputTarget : MonoBehaviour
{
	public abstract void OnClick(Vector2 position);

	public abstract void OnDrag(Vector2 position);

	public abstract void OnRelease();
}
