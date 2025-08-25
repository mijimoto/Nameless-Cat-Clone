using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CommandBufferPortal : MonoBehaviour
{
	[SerializeField]
	private Material m_Material;

	private Dictionary<Camera, CommandBuffer> m_Cameras;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	private void Cleanup()
	{
	}

	public void OnWillRenderObject()
	{
	}
}
