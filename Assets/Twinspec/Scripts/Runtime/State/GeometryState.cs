// Assets/Twinspec/Scripts/Runtime/State/GeometryState.cs
using System;

namespace Twinspec.State
{

    [Serializable]
    public sealed class GeometryState
    {
        public float alpha_i_deg = 0.12f;
        public float phi_deg = 0.0f;

        public string detector_distance_preset = "near"; // "near" | "far"
        public float detector_tilt_deg = 0.0f;

        public string material_class = "polymer_face_on";

        public BeamCenterPx beam_center_px = new BeamCenterPx { x = 256, y = 256 };
        public float beamstop_radius_px = 18.0f;
        public BeamstopOffsetPx beamstop_offset_px = new BeamstopOffsetPx { dx = 0.0f, dy = 0.0f };
    }

    [Serializable]
    public struct BeamCenterPx { public int x; public int y; }

    [Serializable]
    public struct BeamstopOffsetPx { public float dx; public float dy; }
}

