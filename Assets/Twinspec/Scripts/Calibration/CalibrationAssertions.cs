// Assets/Twinspec/Scripts/Calibration/CalibrationAssertions.cs
using UnityEngine;
using Twinspec.Rig;

namespace Twinspec.Calibration
{
    public static class CalibrationAssertions
    {
        public static void AssertNearly(float a, float b, float eps, string label)
        {
            if (Mathf.Abs(a - b) > eps) Debug.LogError($"[ASSERT] {label}: {a} != {b} (eps {eps})");
        }

        public static void AssertVectorNearly(Vector3 a, Vector3 b, float eps, string label)
        {
            if ((a - b).magnitude > eps) Debug.LogError($"[ASSERT] {label}: {a} != {b} (eps {eps})");
        }

        public static void AssertDetectorCenterMapping(RigReferences refs, int cx, int cy, float eps)
        {
            var local = refs.detectorMapper.PixelToLocal(cx, cy);
            AssertVectorNearly(local, Vector3.zero, eps, "Detector center px maps to local (0,0)");
        }

        public static void AssertDistancePreset(RigController rig, string preset, float expectedZ, float eps)
        {
            var z = rig.refs.detectorDistancePivot.localPosition.z;
            if (preset != rig.Current.detector_distance_preset)
                Debug.LogWarning($"[ASSERT] preset mismatch: expected {preset}, Current has {rig.Current.detector_distance_preset}");

            AssertNearly(z, expectedZ, eps, $"DetectorDistancePivot Z for {preset}");
        }
    }
}