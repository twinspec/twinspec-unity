// Assets/Twinspec/Scripts/Runtime/Rig/RigAnimator.cs
using System.Collections;
using UnityEngine;
using Twinspec.State;

namespace Twinspec.Rig
{
    public sealed class RigAnimator : MonoBehaviour
    {
        public RigController rig;

        Coroutine _co;

        public void AnimateTo(GeometryState target, float durationSeconds)
        {
            if (rig == null) return;
            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(AnimateRoutine(target, Mathf.Max(0.01f, durationSeconds)));
        }

        IEnumerator AnimateRoutine(GeometryState target, float duration)
        {
            GeometryState start = rig.Current;

            float t = 0f;
            while (t < duration)
            {
                float u = t / duration;
                float eased = u * u * (3f - 2f * u); // smoothstep

                var s = new GeometryState
                {
                    alpha_i_deg = Mathf.Lerp(start.alpha_i_deg, target.alpha_i_deg, eased),
                    phi_deg = Mathf.LerpAngle(start.phi_deg, target.phi_deg, eased),
                    detector_distance_preset = (eased < 0.5f) ? start.detector_distance_preset : target.detector_distance_preset,
                    detector_tilt_deg = Mathf.Lerp(start.detector_tilt_deg, target.detector_tilt_deg, eased),
                    material_class = (eased < 0.5f) ? start.material_class : target.material_class,
                    beam_center_px = eased < 0.5f ? start.beam_center_px : target.beam_center_px,
                    beamstop_radius_px = Mathf.Lerp(start.beamstop_radius_px, target.beamstop_radius_px, eased),
                    beamstop_offset_px = new BeamstopOffsetPx
                    {
                        dx = Mathf.Lerp(start.beamstop_offset_px.dx, target.beamstop_offset_px.dx, eased),
                        dy = Mathf.Lerp(start.beamstop_offset_px.dy, target.beamstop_offset_px.dy, eased),
                    }
                };

                rig.ApplyImmediate(s);
                t += Time.deltaTime;
                yield return null;
            }

            rig.ApplyImmediate(target); // snap to exact
            _co = null;
        }
    }
}