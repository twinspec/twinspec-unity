// Assets/Twinspec/Scripts/Runtime/Interop/WebGLBridge.cs
using UnityEngine;
using Twinspec.Rig;
using Twinspec.State;

namespace Twinspec.Interop
{
    /// <summary>
    /// Attach this to a GameObject named "WebGLBridge" in the scene.
    /// Web console calls:
    ///   SendMessage("WebGLBridge","SetGeometryState", jsonString)
    ///   SendMessage("WebGLBridge","AnimateToGeometryState", jsonWithDuration)
    /// </summary>
    public sealed class WebGLBridge : MonoBehaviour
    {
        [Header("Wiring")]
        public RigController rig;
        public RigAnimator animator;

        [System.Serializable]
        private sealed class AnimatePayload
        {
            public float duration_ms = 250f;
            public GeometryState state = new GeometryState();
        }

        public void SetGeometryState(string json)
        {
            if (rig == null) { Debug.LogError("WebGLBridge: rig not set."); return; }
            var s = JsonCompat.FromJson<GeometryState>(json);
            rig.ApplyImmediate(s);
        }

        public void AnimateToGeometryState(string json)
        {
            if (animator == null || rig == null) { Debug.LogError("WebGLBridge: animator/rig not set."); return; }
            var p = JsonCompat.FromJson<AnimatePayload>(json);
            animator.AnimateTo(p.state, Mathf.Max(0.01f, p.duration_ms / 1000f));
        }
    }
}