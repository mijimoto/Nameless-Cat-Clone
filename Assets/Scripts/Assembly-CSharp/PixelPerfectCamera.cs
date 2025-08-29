using System;
using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour
{
    public enum Dimension { Width = 0, Height = 1 }
    public enum ConstraintType { None = 0, Horizontal = 1, Vertical = 2 }

    public static int PIXELS_PER_UNIT = 100;

    public bool maxCameraHalfWidthEnabled;
    public bool maxCameraHalfHeightEnabled;
    public float maxCameraHalfWidth = 10f;   // in world units (half-width)
    public float maxCameraHalfHeight = 5f;   // in world units (half-height)
    public Dimension targetDimension = Dimension.Width;
    public float targetCameraHalfWidth = 10f;
    public float targetCameraHalfHeight = 5f;
    public bool pixelPerfect = true;
    public bool retroSnap = true;
    public float assetsPixelsPerUnit = 100f;
    public bool showHUD = false;

    [NonSerialized] public Vector2 cameraSize; // x = halfWidth, y = halfHeight
    [NonSerialized] public ConstraintType contraintUsed;
    [NonSerialized] public float cameraPixelsPerUnit;
    [NonSerialized] public float ratio;
    [NonSerialized] public Vector2 nativeAssetResolution;
    [NonSerialized] public float fovCoverage;
    [NonSerialized] public bool isInitialized;

    private Resolution res;
    private Camera cam;

    // Returns orthographicSize (half-height)
    private float calculatePixelPerfectCameraSize(bool pixelPerfect, Resolution res, float assetsPixelsPerUnit,
        float maxCameraHalfWidth, float maxCameraHalfHeight, float targetHalfWidth, float targetHalfHeight,
        Dimension targetDimension)
    {
        float screenRatio = (float)res.width / res.height; // width / height
        float halfHeight;

        if (!pixelPerfect)
        {
            // Return half-height depending on target dimension
            halfHeight = (targetDimension == Dimension.Width) ? (targetHalfWidth / screenRatio) : targetHalfHeight;
            // constraints below will still apply
        }
        else
        {
            // For width-target: we want to keep the half-width constant; orthographicSize (half-height) = halfWidth / aspect
            // For height-target: orthographicSize == targetHalfHeight
            if (targetDimension == Dimension.Width)
            {
                halfHeight = targetHalfWidth / screenRatio;
            }
            else
            {
                halfHeight = targetHalfHeight;
            }
        }

        // Compute halfWidth for constraint checks
        float halfWidth = halfHeight * screenRatio;

        // Apply max constraints (apply to the correct dimension)
        contraintUsed = ConstraintType.None;
        if (maxCameraHalfWidthEnabled && halfWidth > maxCameraHalfWidth)
        {
            halfWidth = maxCameraHalfWidth;
            halfHeight = halfWidth / screenRatio;
            contraintUsed = ConstraintType.Horizontal;
        }

        if (maxCameraHalfHeightEnabled && halfHeight > maxCameraHalfHeight)
        {
            halfHeight = maxCameraHalfHeight;
            halfWidth = halfHeight * screenRatio;
            contraintUsed = (contraintUsed == ConstraintType.Horizontal) ? contraintUsed : ConstraintType.Vertical;
        }

        // Make pixel perfect rounding on half-height (orthographic size) based on assets PPU
        float ppu = Mathf.Max(1f, assetsPixelsPerUnit);
        float pixels = halfHeight * ppu;
        int pixelsInt = Mathf.RoundToInt(pixels);
        // avoid division by zero just in case
        halfHeight = pixelsInt / ppu;

        return halfHeight;
    }

    public void adjustCameraFOV()
    {
        if (cam == null) return;

        res = new Resolution();
        res.width = Screen.width;
        res.height = Screen.height;

        float cameraHalfHeight = calculatePixelPerfectCameraSize(pixelPerfect, res, assetsPixelsPerUnit,
            maxCameraHalfWidth, maxCameraHalfHeight, targetCameraHalfWidth, targetCameraHalfHeight, targetDimension);

        cam.orthographicSize = cameraHalfHeight;

        // Calculate additional properties
        float halfWidth = cameraHalfHeight * cam.aspect;
        this.cameraSize = new Vector2(halfWidth, cameraHalfHeight);
        cameraPixelsPerUnit = res.height / (2f * cameraHalfHeight); // pixels / unit
        ratio = cameraPixelsPerUnit / assetsPixelsPerUnit;
        nativeAssetResolution = new Vector2(this.cameraSize.x * 2f * assetsPixelsPerUnit, this.cameraSize.y * 2f * assetsPixelsPerUnit);
        fovCoverage = this.cameraSize.y * 2f;

        isInitialized = true;

        // Do initial retro snap (position rounding) once here
        if (retroSnap && pixelPerfect)
        {
            if (!float.IsNaN(cameraPixelsPerUnit) && cameraPixelsPerUnit > 0f)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Round(pos.x * cameraPixelsPerUnit) / cameraPixelsPerUnit;
                pos.y = Mathf.Round(pos.y * cameraPixelsPerUnit) / cameraPixelsPerUnit;
                transform.position = pos;
            }
        }
    }

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = gameObject.AddComponent<Camera>();
            cam.orthographic = true;
        }
        // If camera is not orthographic, force it (pixel perfect only makes sense orthographic)
        cam.orthographic = true;

        adjustCameraFOV();
    }

    private void OnValidate()
    {
        if (Application.isPlaying || !gameObject.activeInHierarchy) return;
        adjustCameraFOV();
    }

    // Use LateUpdate so snapping happens after CameraManager/other systems moved the camera
    private void LateUpdate()
    {
        if (res.width != Screen.width || res.height != Screen.height)
        {
            adjustCameraFOV();
        }

        if (retroSnap && pixelPerfect && isInitialized)
        {
            if (!float.IsNaN(cameraPixelsPerUnit) && cameraPixelsPerUnit > 0f)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Round(pos.x * cameraPixelsPerUnit) / cameraPixelsPerUnit;
                pos.y = Mathf.Round(pos.y * cameraPixelsPerUnit) / cameraPixelsPerUnit;
                transform.position = pos;
            }
        }
    }

    private void OnGUI()
    {
        if (!showHUD || !isInitialized) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Pixel Perfect Camera Debug");
        GUILayout.Label($"Camera Size (halfWidth,halfHeight): {cameraSize}");
        GUILayout.Label($"Pixels Per Unit: {cameraPixelsPerUnit:F2}");
        GUILayout.Label($"Ratio: {ratio:F2}");
        GUILayout.Label($"Native Resolution: {nativeAssetResolution}");
        GUILayout.Label($"Screen: {res.width}x{res.height}");
        GUILayout.Label($"Constraint: {contraintUsed}");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
