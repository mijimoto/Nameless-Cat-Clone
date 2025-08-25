using System;
using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour
{
	public enum Dimension
	{
		Width = 0,
		Height = 1
	}

	public enum ConstraintType
	{
		None = 0,
		Horizontal = 1,
		Vertical = 2
	}

	public static int PIXELS_PER_UNIT;

	public bool maxCameraHalfWidthEnabled;

	public bool maxCameraHalfHeightEnabled;

	public float maxCameraHalfWidth;

	public float maxCameraHalfHeight;

	public Dimension targetDimension;

	public float targetCameraHalfWidth;

	public float targetCameraHalfHeight;

	public bool pixelPerfect;

	public bool retroSnap;

	public float assetsPixelsPerUnit;

	public bool showHUD;

	[NonSerialized]
	public Vector2 cameraSize;

	[NonSerialized]
	public ConstraintType contraintUsed;

	[NonSerialized]
	public float cameraPixelsPerUnit;

	[NonSerialized]
	public float ratio;

	[NonSerialized]
	public Vector2 nativeAssetResolution;

	[NonSerialized]
	public float fovCoverage;

	[NonSerialized]
	public bool isInitialized;

	private Resolution res;

	private Camera cam;

	private float calculatePixelPerfectCameraSize(bool pixelPerfect, Resolution res, float assetsPixelsPerUnit, float maxCameraHalfWidth, float maxCameraHalfHeight, float targetHalfWidth, float targetHalfHeight, Dimension targetDimension)
	{
		return 0f;
	}

	public void adjustCameraFOV()
	{
	}

	private void OnEnable()
	{
	}

	private void OnValidate()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
	}
}
