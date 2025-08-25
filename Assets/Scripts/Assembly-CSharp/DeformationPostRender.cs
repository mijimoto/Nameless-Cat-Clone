using UnityEngine;

public class DeformationPostRender : MonoBehaviour
{
	public int width;

	public int height;

	public Mesh screenMesh;

	private Camera targetCamera;

	private RenderTexture mainRT;

	public RenderTexture ownRT;

	public Material distortionMaterial;

	public int passNum;

	private Mesh createMesh()
	{
		return null;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
	}
}
