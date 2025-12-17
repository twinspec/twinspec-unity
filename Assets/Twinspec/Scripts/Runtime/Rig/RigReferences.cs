// Assets/Twinspec/Scripts/Runtime/Rig/RigReferences.cs
using UnityEngine;

namespace Twinspec.Rig
{
    public sealed class RigReferences : MonoBehaviour
    {
        [Header("Sample Rig")]
        public Transform sampleAlphaPivot;
        public Transform samplePhiPivot;
        public Renderer sampleRenderer;

        [Header("Detector Rig")]
        public Transform detectorDistancePivot;
        public Transform detectorTiltPivot;
        public Transform detectorPlane;            // parent that defines plane local space
        public Transform beamCenterHandle;
        public Transform beamstopRoot;
        public Transform beamstopMesh;

        [Header("Beam Rig")]
        public Transform beamRoot;                 // optional
        public LineRenderer beamLine;              // optional

        [Header("Helpers")]
        public DetectorPlaneMapper detectorMapper;
    }
}
