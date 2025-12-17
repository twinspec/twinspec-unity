// Assets/Twinspec/Scripts/Runtime/Rig/RigController.cs
using UnityEngine;
using Twinspec.State;
using Twinspec.Viz;

namespace Twinspec.Rig
{
    public sealed class RigController : MonoBehaviour
    {
        [Header("Wiring")]
        public RigReferences refs;

        [Header("Visual Exaggeration")]
        [Tooltip("alpha_visual_deg = alpha_i_deg * scale (e.g., 30–50)")]
        [Range(1f, 100f)]
        public float alphaVisualScale = 40f;

        [Header("Distance Presets (Unity units along +Z)")]
        public float detectorZNear = 4.5f;
        public float detectorZFar = 7.5f;

        [Header("Optional: gizmos")]
        public AxisGizmo qAxisGizmo; // can be null
        public ArcRenderer alphaArc; // can be null
        public ArcRenderer phiArc;   // can be null

        [Header("Styling")]
        public MaterialClassStyling materialStyling; // can be null

        public GeometryState Current { get; private set; } = new GeometryState();

        public void ApplyImmediate(GeometryState s)
        {
            if (refs == null) { Debug.LogError("RigController: refs not set."); return; }
            Current = s;

            // Sample alpha (about +X)
            float alphaVisual = s.alpha_i_deg * alphaVisualScale;
            refs.sampleAlphaPivot.localRotation = Quaternion.Euler(alphaVisual, 0f, 0f);

            // Sample phi (about +Y) in tilted frame
            refs.samplePhiPivot.localRotation = Quaternion.Euler(0f, s.phi_deg, 0f);

            // Detector distance
            float z = (s.detector_distance_preset == "far") ? detectorZFar : detectorZNear;
            Vector3 p = refs.detectorDistancePivot.localPosition;
            refs.detectorDistancePivot.localPosition = new Vector3(p.x, p.y, z);

            // Detector tilt about +X
            refs.detectorTiltPivot.localRotation = Quaternion.Euler(s.detector_tilt_deg, 0f, 0f);

            // Beam center + beamstop placement (in detector plane local coords)
            if (refs.detectorMapper != null && refs.detectorPlane != null)
            {
                Vector3 centerLocal = refs.detectorMapper.PixelToLocal(s.beam_center_px.x, s.beam_center_px.y);
                Vector3 offsetLocal = refs.detectorMapper.OffsetPxToLocal(s.beamstop_offset_px.dx, s.beamstop_offset_px.dy);

                if (refs.beamCenterHandle != null)
                    refs.beamCenterHandle.localPosition = centerLocal;

                if (refs.beamstopRoot != null)
                    refs.beamstopRoot.localPosition = centerLocal + offsetLocal;

                if (refs.beamstopMesh != null)
                {
                    float rUnits = refs.detectorMapper.RadiusPxToLocalScale(s.beamstop_radius_px);
                    // BeamstopMesh should be a unit disc scaled in X/Y
                    refs.beamstopMesh.localScale = new Vector3(rUnits, rUnits, refs.beamstopMesh.localScale.z);
                }
            }

            // Material styling
            if (materialStyling != null)
                materialStyling.Apply(refs.sampleRenderer, s.material_class);

            // Optional gizmos
            qAxisGizmo?.Refresh();
            alphaArc?.SetDegrees(alphaVisual, labelText: $"{s.alpha_i_deg:0.00}°");
            phiArc?.SetDegrees(s.phi_deg, labelText: $"{s.phi_deg:0.#}°");
        }
    }
}