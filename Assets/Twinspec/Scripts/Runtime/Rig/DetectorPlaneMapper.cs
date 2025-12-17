// Assets/Twinspec/Scripts/Runtime/Rig/DetectorPlaneMapper.cs
using UnityEngine;

namespace Twinspec.Rig
{
    /// <summary>
    /// Converts detector pixel coords (0..N-1) to DetectorPlane local coordinates on a Quad/Plane.
    /// Assumes detector plane local X=right(qy), local Y=up(qz), local Z=normal(+Z).
    /// Pixel (0,0) is bottom-left; pixel (N/2,N/2) maps to (0,0).
    /// </summary>
    public sealed class DetectorPlaneMapper : MonoBehaviour
    {
        [Header("Detector Mapping")]
        [Min(1)] public int textureSizePx = 512;

        [Tooltip("Width of active area in Unity units (local X span).")]
        [Min(0.0001f)] public float planeWidthUnits = 2.0f;

        [Tooltip("Height of active area in Unity units (local Y span).")]
        [Min(0.0001f)] public float planeHeightUnits = 2.0f;

        public Vector3 PixelToLocal(int xPx, int yPx)
        {
            // normalized [0,1]
            float u = (xPx + 0.5f) / textureSizePx;
            float v = (yPx + 0.5f) / textureSizePx;

            // centered [-0.5, +0.5]
            float cx = u - 0.5f;
            float cy = v - 0.5f;

            return new Vector3(cx * planeWidthUnits, cy * planeHeightUnits, 0f);
        }

        public Vector3 OffsetPxToLocal(float dxPx, float dyPx)
        {
            float dx = (dxPx / textureSizePx) * planeWidthUnits;
            float dy = (dyPx / textureSizePx) * planeHeightUnits;
            return new Vector3(dx, dy, 0f);
        }

        public float RadiusPxToLocalScale(float radiusPx)
        {
            // choose scale based on average pixel-to-units conversion
            float pxToUnitsX = planeWidthUnits / textureSizePx;
            float pxToUnitsY = planeHeightUnits / textureSizePx;
            float pxToUnits = 0.5f * (pxToUnitsX + pxToUnitsY);
            return radiusPx * pxToUnits;
        }
    }
}