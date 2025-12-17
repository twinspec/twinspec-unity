// Assets/Twinspec/Scripts/Calibration/CalibrationScenario.cs
using System;
using Twinspec.State;

namespace Twinspec.Calibration
{
    [Serializable]
    public sealed class CalibrationScenario
    {
        public string id;
        public string description;
        public GeometryState geometry = new GeometryState();

        public static CalibrationScenario A_BaselinePolymer()
        {
            return new CalibrationScenario
            {
                id = "A",
                description = "Baseline polymer_face_on (thin, as_cast), near, alpha=0.12, exp=200 bin=1",
                geometry = new GeometryState
                {
                    material_class = "polymer_face_on",
                    alpha_i_deg = 0.12f,
                    phi_deg = 0f,
                    detector_distance_preset = "near",
                    detector_tilt_deg = 0f,
                    beam_center_px = new BeamCenterPx { x = 256, y = 256 },
                    beamstop_radius_px = 18f,
                    beamstop_offset_px = new BeamstopOffsetPx { dx = 0f, dy = 0f },
                }
            };
        }

        public static CalibrationScenario B_OverexposedLike()
        {
            // Unity side: same geometry; exposure/binning is backend-only.
            return new CalibrationScenario { id = "B", description = "Overexposed geometry baseline (backend would set exp=800 bin=1)", geometry = A_BaselinePolymer().geometry };
        }

        public static CalibrationScenario D_ThickTooShallow()
        {
            return new CalibrationScenario
            {
                id = "D",
                description = "Thick sample with alpha=0.08 (guard should warn in web/backend)",
                geometry = new GeometryState
                {
                    material_class = "polymer_face_on",
                    alpha_i_deg = 0.08f,
                    phi_deg = 0f,
                    detector_distance_preset = "near",
                    detector_tilt_deg = 0f,
                    beam_center_px = new BeamCenterPx { x = 256, y = 256 },
                    beamstop_radius_px = 18f,
                    beamstop_offset_px = new BeamstopOffsetPx { dx = 0f, dy = 0f },
                }
            };
        }

        public static CalibrationScenario E_BeamstopOccluding()
        {
            return new CalibrationScenario
            {
                id = "E",
                description = "Beamstop large (radius=40px) centered at beam center",
                geometry = new GeometryState
                {
                    material_class = "polymer_face_on",
                    alpha_i_deg = 0.12f,
                    phi_deg = 0f,
                    detector_distance_preset = "near",
                    detector_tilt_deg = 0f,
                    beam_center_px = new BeamCenterPx { x = 256, y = 256 },
                    beamstop_radius_px = 40f,
                    beamstop_offset_px = new BeamstopOffsetPx { dx = 0f, dy = 0f },
                }
            };
        }

        public static CalibrationScenario F_OxidePowder()
        {
            return new CalibrationScenario
            {
                id = "F",
                description = "Oxide powder reference geometry (alpha=0.20, far)",
                geometry = new GeometryState
                {
                    material_class = "oxide_wurtzite_powder",
                    alpha_i_deg = 0.20f,
                    phi_deg = 0f,
                    detector_distance_preset = "far",
                    detector_tilt_deg = 0f,
                    beam_center_px = new BeamCenterPx { x = 256, y = 256 },
                    beamstop_radius_px = 18f,
                    beamstop_offset_px = new BeamstopOffsetPx { dx = 0f, dy = 0f },
                }
            };
        }
    }
}