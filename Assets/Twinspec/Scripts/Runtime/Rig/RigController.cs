// Assets/Twinspec/Scripts/Runtime/Rig/RigController.cs
using UnityEngine;
using Twinspec.State;

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

        // Sanity kick so you SEE motion immediately during hackathon wiring.
        // Remove later once UI / events drive state.
        private void Update()
        {
            if (refs == null) return;

            var g = Current ?? new GeometryState();

            g.alpha_i_deg = 0.12f;
            g.phi_deg = Mathf.PingPong(Time.time * 45f, 90f) - 45f; // oscillate -45..+45
            g.detector_distance_preset = "far";
            g.detector_tilt_deg = Mathf.Sin(Time.time) * 10f;

            ApplyImmediate(g);
        }

        public void ApplyImmediate(GeometryState s)
        {
            if (refs == null)
            {
                Debug.LogError("RigController: refs not set.");
                return;
            }

            if (s == null)
            {
                Debug.LogError("RigController: state is null.");
                return;
            }

            Current = s;

            // -------- Sample --------
            float alphaVisual = s.alpha_i_deg * alphaVisualScale;

            if (refs.sampleAlphaPivot != null)
                refs.sampleAlphaPivot.localRotation = Quaternion.Euler(alphaVisual, 0f, 0f);

            if (refs.samplePhiPivot != null)
                refs.samplePhiPivot.localRotation = Quaternion.Euler(0f, s.phi_deg, 0f);

            // -------- Detector distance --------
            string preset = (s.detector_distance_preset ?? "near").ToLowerInvariant();
            float z = (preset == "far") ? detectorZFar : detectorZNear;

            if (refs.detectorDistancePivot != null)
            {
                Vector3 p = refs.detectorDistancePivot.localPosition;
                refs.detectorDistancePivot.localPosition = new Vector3(p.x, p.y, z);
            }

            // -------- Detector tilt --------
            if (refs.detectorTiltPivot != null)
                refs.detectorTiltPivot.localRotation = Quaternion.Euler(s.detector_tilt_deg, 0f, 0f);

            // -------- Beam center + beamstop placement --------
            if (refs.detectorMapper != null)
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
                    refs.beamstopMesh.localScale = new Vector3(rUnits, rUnits, refs.beamstopMesh.localScale.z);
                }
            }

            // -------- Material styling --------
            // Standardize: styling script should already know its targetRenderer (or can use refs.sampleRenderer).
            if (materialStyling != null)
            {
                // Option A (recommended): MaterialClassStyling.Apply(string materialClass)
                // If your MaterialClassStyling currently uses this signature, keep this call:
                materialStyling.Apply(s.material_class);

                // Option B: If you kept Apply(Renderer, string), comment the line above and use this instead:
                // materialStyling.Apply(refs.sampleRenderer, s.material_class);
            }

            // -------- Optional gizmos --------
            qAxisGizmo?.Refresh();

            // Don’t use named args (labelText:) unless ArcRenderer defines that parameter name.
            // Keep it simple: call with one arg OR two positional args depending on your ArcRenderer.
            if (alphaArc != null)
                alphaArc.SetDegrees(alphaVisual);

            if (phiArc != null)
                phiArc.SetDegrees(s.phi_deg);
        }
    }
}