using UnityEngine;

[ExecuteInEditMode]
public class CustomBloom : MonoBehaviour
{
	public bool allowBloom;

	public Material bloomMat;

	public float bloomIntensity;

	public float blurIntensity;

	public float blur2Factor;

	public Shader blurShader;

	private RenderTexture blurRenderTex1;

	private RenderTexture blurRenderTex2;

	private Material blurMatFull;

	private Material blurMatFull2;

	private Vector2 pixels;

	private void Start()
	{
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
	}
}
