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

    public static int PIXELS_PER_UNIT = 100;

    public bool maxCameraHalfWidthEnabled;
    public bool maxCameraHalfHeightEnabled;
    public float maxCameraHalfWidth = 10f;
    public float maxCameraHalfHeight = 5f;
    public Dimension targetDimension = Dimension.Width;
    public float targetCameraHalfWidth = 10f;
    public float targetCameraHalfHeight = 5f;
    public bool pixelPerfect = true;
    public bool retroSnap = true;
    public float assetsPixelsPerUnit = 100f;
    public bool showHUD = false;

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

    private float calculatePixelPerfectCameraSize(bool pixelPerfect, Resolution res, float assetsPixelsPerUnit, 
        float maxCameraHalfWidth, float maxCameraHalfHeight, float targetHalfWidth, float targetHalfHeight, 
        Dimension targetDimension)
    {
        if (!pixelPerfect)
        {
            return targetDimension == Dimension.Width ? targetHalfWidth : targetHalfHeight;
        }

        float targetRatio = targetHalfWidth / targetHalfHeight;
        float screenRatio = (float)res.width / res.height;
        
        float cameraSize;
        
        if (targetDimension == Dimension.Width)
        {
            cameraSize = targetHalfWidth;
            if (screenRatio < targetRatio)
            {
                cameraSize = targetHalfHeight * screenRatio;
            }
        }
        else
        {
            cameraSize = targetHalfHeight;
            if (screenRatio > targetRatio)
            {
                cameraSize = targetHalfWidth / screenRatio;
            }
        }

        // Apply max constraints
        if (maxCameraHalfWidthEnabled && cameraSize > maxCameraHalfWidth)
            cameraSize = maxCameraHalfWidth;
        if (maxCameraHalfHeightEnabled && cameraSize > maxCameraHalfHeight)
            cameraSize = maxCameraHalfHeight;

        // Make pixel perfect
        float pixelsPerUnit = assetsPixelsPerUnit;
        float pixels = cameraSize * pixelsPerUnit;
        int pixelsInt = Mathf.RoundToInt(pixels);
        cameraSize = pixelsInt / pixelsPerUnit;

        return cameraSize;
    }

    public void adjustCameraFOV()
    {
        if (cam == null) return;

        res = new Resolution();
        res.width = Screen.width;
        res.height = Screen.height;

        float cameraSize = calculatePixelPerfectCameraSize(pixelPerfect, res, assetsPixelsPerUnit, 
            maxCameraHalfWidth, maxCameraHalfHeight, targetCameraHalfWidth, targetCameraHalfHeight, targetDimension);

        cam.orthographicSize = cameraSize;
        
        // Calculate additional properties
        this.cameraSize = new Vector2(cameraSize * cam.aspect, cameraSize);
        cameraPixelsPerUnit = res.height / (2f * cameraSize);
        ratio = cameraPixelsPerUnit / assetsPixelsPerUnit;
        nativeAssetResolution = new Vector2(this.cameraSize.x * 2f * assetsPixelsPerUnit, this.cameraSize.y * 2f * assetsPixelsPerUnit);
        fovCoverage = this.cameraSize.y * 2f;

        // Apply retro snap
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
        isInitialized = true;
    }

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = gameObject.AddComponent<Camera>();
            cam.orthographic = true;
        }
        adjustCameraFOV();
    }

    private void OnValidate()
    {
        if (Application.isPlaying || !gameObject.activeInHierarchy) return;
        adjustCameraFOV();
    }

    private void Update()
    {
        if (res.width != Screen.width || res.height != Screen.height)
        {
            adjustCameraFOV();
        }

        if (retroSnap && pixelPerfect && isInitialized)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Round(pos.x * cameraPixelsPerUnit) / cameraPixelsPerUnit;
            pos.y = Mathf.Round(pos.y * cameraPixelsPerUnit) / cameraPixelsPerUnit;
            transform.position = pos;
        }
    }

    private void OnGUI()
    {
        if (!showHUD || !isInitialized) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("Pixel Perfect Camera Debug");
        GUILayout.Label($"Camera Size: {cameraSize}");
        GUILayout.Label($"Pixels Per Unit: {cameraPixelsPerUnit:F2}");
        GUILayout.Label($"Ratio: {ratio:F2}");
        GUILayout.Label($"Native Resolution: {nativeAssetResolution}");
        GUILayout.Label($"Screen: {res.width}x{res.height}");
        GUILayout.Label($"Constraint: {contraintUsed}");
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}