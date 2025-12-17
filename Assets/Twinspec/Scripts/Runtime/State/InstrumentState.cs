// Assets/Twinspec/Scripts/Runtime/State/InstrumentState.cs
using System;

namespace Twinspec.State
{
    [Serializable]
    public sealed class InstrumentState
    {
        public GeometryState geometry = new GeometryState();
        public AcquisitionState acquisition = new AcquisitionState();
        public SampleState sample = new SampleState();
        public PlannerState planner = new PlannerState();
    }

    [Serializable] public sealed class AcquisitionState { public int exposure_ms = 200; public int binning = 1; public int frames = 1; }
    [Serializable] public sealed class SampleState { public string material_class = "polymer_face_on"; public string thickness_tag = "thin"; public string process_tag = "as_cast"; }
    [Serializable] public sealed class PlannerState { public string target_peak_family = "pi_pi"; public float target_snr_min = 20f; public float max_saturation_pct = 5f; public float max_occlusion_pct = 1f; }
}
