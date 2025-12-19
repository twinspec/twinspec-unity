# TwinSpec Unity — GIWAXS Visual Digital Twin

**TwinSpec Unity** is the **interpretive visual digital twin** for the TwinSpec GIWAXS platform.  
It provides a **state-driven, kinematically correct visualization** of a grazing-incidence X-ray scattering experiment, designed to give users geometric intuition for how experimental parameters map onto scattering space.

This visual twin is **explicitly decoupled** from research-grade data generation, simulation, and machine learning, which are handled by the TwinSpec data twin and lakehouse.

> **Real-time kinematic coupling is already implemented at the code level; this submission prioritizes interpretive clarity while the final physical bindings between geometry and rig controls are being validated.**

---

## Why a visual digital twin?

GIWAXS experiments are difficult to interpret because key variables—incidence angle, detector distance, azimuthal rotation, beam center, and beamstop—are not directly observable during measurement.

TwinSpec Unity exists to:

- Make **instrument geometry legible**
- Show how **user-selected parameters affect scattering geometry**
- Support **education, intuition, and experimental planning**
- Remain **strictly non-authoritative** for scientific data

### Explicit design decision

TwinSpec intentionally **separates concerns**:

> **TwinSpec separates interpretive visualization from research-grade data representation.**  
> The visual twin provides geometric intuition for how measurement parameters map onto scattering space, while the data twin preserves the full experimental state required for modeling, comparison, and machine learning.

Unity **never** generates GIWAXS patterns.  
The backend **never** moves geometry.

Both consume the **same instrument state** from the lab console.

---

## System architecture (high level)
```
User
↓
TwinSpec Web Console (source of truth)
├── sends GeometryState → Unity (visual twin)
├── sends Full InstrumentState → Backend (data twin)
↓
Unity Visual Twin Data Twin + Lakehouse
(state → geometry) (state → synthetic data)
```


- **Single source of truth**: the web console  
- **Unity**: deterministic geometry updates only  
- **Backend**: deterministic data + metrics only  

There is **no feedback loop** from Unity to the console.

---

## What’s implemented vs. what’s pending

### Implemented (and verifiable in this repo)

- **State-driven kinematics**
  - `GeometryState` defines all geometry-relevant parameters
  - `RigController.ApplyImmediate(state)` deterministically applies them
- **Explicit pivot hierarchy**
  - Sample αᵢ tilt, φ rotation, detector translation/tilt are all separate pivots
- **Inspector-based wiring**
  - No name-based lookups; all transforms are referenced explicitly
- **WebGL-ready API surface**
  - Unity can be driven by JSON geometry payloads from the console
- **Public Unity codebase**
  - Judges can inspect and run the kinematic logic directly

### Implemented but under validation (by design)

- **Physical bindings between imported meshes and pivots**
  - Pivot placement
  - Parent-child relationships for imported FBX assets
  - Mesh local origins

This is why the submission may present the visual twin in a **stabilized configuration** rather than showing full motion for all parts.

### Stubbed / optional (not required for submission)

- Smooth animation / lerping
- Educational overlays (arcs, axes)
- Scenario calibration harness
- Bidirectional sync (Unity → console)

---

## Core concepts

### GeometryState (authoritative input)

Unity consumes a **geometry-only subset** of the full instrument state:

- Incidence angle (αᵢ)
- Azimuthal rotation (φ)
- Detector distance preset
- Detector tilt
- Beam center
- Beamstop radius and offset
- Material class (visual styling only)

Defined in:

```Assets/Twinspec/Scripts/Runtime/State/GeometryState.cs```


### RigController (deterministic executor)

All geometry updates flow through a single method:

```RigController.ApplyImmediate(GeometryState state)```


Responsibilities:

- Rotate sample αᵢ pivot (with visual exaggeration)
- Rotate sample φ pivot
- Translate detector along +Z (near/far)
- Apply detector tilt
- Position beam center and beamstop
- Trigger optional visualization helpers

There is **no hidden state** and no physics simulation.

---

## Repository structure

```
twinspec-unity/
├── README.md
├── .gitignore
│
├── Assets/
│ └── Twinspec/
│ ├── Scenes/
│ │ ├── GIWAXS_InstrumentInterior.unity
│ │ └── GIWAXS_CalibrationHarness.unity
│ │
│ ├── Prefabs/
│ │ ├── GIWAXS_Rig.prefab
│ │ └── GIWAXS_SceneHelpers.prefab
│ │
│ ├── Scripts/
│ │ ├── Runtime/
│ │ │ ├── Core/
│ │ │ │ ├── TwinspecEventBus.cs
│ │ │ │ ├── TwinspecRuntimeMode.cs
│ │ │ │ └── Units.cs
│ │ │ │
│ │ │ ├── State/
│ │ │ │ ├── GeometryState.cs
│ │ │ │ ├── InstrumentState.cs
│ │ │ │ └── ScenarioPresets.cs
│ │ │ │
│ │ │ ├── Rig/
│ │ │ │ ├── RigReferences.cs
│ │ │ │ ├── RigController.cs
│ │ │ │ ├── RigAnimator.cs
│ │ │ │ └── DetectorPlaneMapper.cs
│ │ │ │
│ │ │ ├── Interop/
│ │ │ │ ├── WebGLBridge.cs
│ │ │ │ └── JsonCompat.cs
│ │ │ │
│ │ │ └── Viz/
│ │ │ ├── AxisGizmo.cs
│ │ │ ├── ArcRenderer.cs
│ │ │ └── MaterialClassStyling.cs
│ │ │
│ │ └── Calibration/
│ │ ├── CalibrationHarness.cs
│ │ ├── CalibrationScenario.cs
│ │ └── CalibrationAssertions.cs
│ │
│ ├── ScriptableObjects/
│ │ ├── DistancePresets.asset
│ │ └── MaterialClassPalette.asset
│ │
│ ├── Materials/
│ ├── Shaders/
│ └── Textures/
│
├── Packages/
└── ProjectSettings/
```


---

## Rig hierarchy
```
GIWAXS_Rig
├── BeamRoot
│ └── BeamMesh
│
├── SampleStageRoot
│ └── SampleAlphaPivot (+X rotation, αᵢ)
│ └── SamplePhiPivot (+Y rotation, φ)
│ └── SampleMesh
│
├── DetectorRigRoot
│ └── DetectorDistancePivot (+Z translation)
│ └── DetectorTiltPivot (+X rotation)
│ └── DetectorPlane
│ ├── BeamCenterHandle
│ └── BeamstopRoot
│ └── BeamstopMesh
│
└── SceneHelpers
├── WorldAxesGizmo
└── QSpaceAxesNearDetector
```


This hierarchy is **required** for correct kinematic behavior.

---

## How to verify claims (for judges)

1. Open `RigController.cs`
2. Inspect `ApplyImmediate(GeometryState s)`
3. Confirm:
   - No physics
   - No GIWAXS data rendering
   - Pure transform logic
4. Inspect `GeometryState.cs`
5. Inspect `WebGLBridge.cs` for the interop surface

All claims in this README are directly verifiable.

---

## Intended future work (explicit and bounded)

- Finalize physical bindings for imported instrument meshes
- Enable smooth animated transitions (optional)
- Add optional educational overlays (arcs, labels)
- Expand scenario presets for teaching workflows

No architectural changes are required.

---

## Summary

TwinSpec Unity is a **state-driven, kinematically correct visual digital twin** designed to complement—not replace—the TwinSpec data twin.

It prioritizes **interpretive clarity**, architectural correctness, and separation of concerns, while exposing a public Unity codebase that already implements real-time kinematic coupling at the script level.

The remaining work is **mechanical validation**, not conceptual invention.


