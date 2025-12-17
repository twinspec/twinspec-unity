using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcRenderer : MonoBehaviour
{
    public float radius = 0.4f;
    public int segments = 32;
    public Vector3 planeNormal = Vector3.forward;

    private LineRenderer _lr;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.useWorldSpace = false;
    }

    // ✅ Must include parameter named "labelText" to match RigController calls like SetDegrees(..., labelText: "αi")
    public void SetDegrees(float degrees, string labelText = null)
    {
        if (_lr == null) _lr = GetComponent<LineRenderer>();

        float rad = Mathf.Deg2Rad * Mathf.Clamp(degrees, 0f, 359f);
        _lr.positionCount = segments + 1;

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

        // labelText is accepted so RigController compiles.
        // If you later add TextMeshPro, you can render this string.
        _ = labelText;
    }
}