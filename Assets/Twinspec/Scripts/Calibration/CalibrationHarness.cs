// Assets/Twinspec/Scripts/Calibration/CalibrationHarness.cs
using System.Collections.Generic;
using UnityEngine;
using Twinspec.Rig;

namespace Twinspec.Calibration
{
    public sealed class CalibrationHarness : MonoBehaviour
    {
        public RigController rig;

        [Header("Assertions")]
        public float epsPos = 0.001f;
        public float epsZ = 0.001f;

        [Header("Preset Z values must match RigController")]
        public float zNear = 4.5f;
        public float zFar = 7.5f;

        [Header("Run on start")]
        public bool runOnStart = true;

        readonly List<CalibrationScenario> _scenarios = new();

        void Awake()
        {
            _scenarios.Add(CalibrationScenario.A_BaselinePolymer());
            _scenarios.Add(CalibrationScenario.D_ThickTooShallow());
            _scenarios.Add(CalibrationScenario.E_BeamstopOccluding());
            _scenarios.Add(CalibrationScenario.F_OxidePowder());
        }

        void Start()
        {
            if (!runOnStart) return;
            RunAll();
        }

        [ContextMenu("Run All Scenarios")]
        public void RunAll()
        {
            if (rig == null || rig.refs == null) { Debug.LogError("CalibrationHarness: rig not set."); return; }

            // Core mapping assertions
            CalibrationAssertions.AssertDetectorCenterMapping(rig.refs, 256, 256, epsPos);

            foreach (var s in _scenarios)
            {
                Debug.Log($"[CAL] Applying scenario {s.id}: {s.description}");
                rig.ApplyImmediate(s.geometry);

                // Distance assertions
                if (s.geometry.detector_distance_preset == "near")
                    CalibrationAssertions.AssertDistancePreset(rig, "near", zNear, epsZ);
                else
                    CalibrationAssertions.AssertDistancePreset(rig, "far", zFar, epsZ);

                // Beamstop anchored to beam center + offset
                if (rig.refs.beamstopRoot != null && rig.refs.beamCenterHandle != null)
                {
                    var expected = rig.refs.beamCenterHandle.localPosition +
                                   rig.refs.detectorMapper.OffsetPxToLocal(
                                       s.geometry.beamstop_offset_px.dx,
                                       s.geometry.beamstop_offset_px.dy);

                    CalibrationAssertions.AssertVectorNearly(rig.refs.beamstopRoot.localPosition, expected, epsPos, "BeamstopRoot anchored correctly");
                }
            }

            Debug.Log("[CAL] Completed.");
        }
    }
}