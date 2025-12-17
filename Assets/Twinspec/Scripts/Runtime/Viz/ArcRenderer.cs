using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcRenderer : MonoBehaviour
{
    public float radius = 0.4f;
    public int segments = 32;

    [Tooltip("Local plane normal. Use Vector3.up for XZ-plane arc, Vector3.forward for XY-plane arc.")]
    public Vector3 planeNormal = Vector3.forward;

    private LineRenderer _lr;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        if (_lr.positionCount < 2) _lr.positionCount = segments + 1;
        _lr.useWorldSpace = false;
    }

    // Minimal API expected by RigController
    public void SetDegrees(float degrees)
    {
        if (_lr == null) _lr = GetComponent<LineRenderer>();

        float rad = Mathf.Deg2Rad * Mathf.Clamp(degrees, 0f, 359f);

        // Build an arc from 0 -> degrees in local space.
        _lr.positionCount = segments + 1;

        // Choose a basis in the plane.
        Vector3 n = planeNormal.normalized;
        Vector3 a = Vector3.right;
        if (Mathf.Abs(Vector3.Dot(a, n)) > 0.9f) a = Vector3.up;
        Vector3 u = Vector3.Normalize(Vector3.Cross(n, a));
        Vector3 v = Vector3.Normalize(Vector3.Cross(n, u));

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float ang = t * rad;
            Vector3 p = (Mathf.Cos(ang) * u + Mathf.Sin(ang) * v) * radius;
            _lr.SetPosition(i, p);
        }
    }
}