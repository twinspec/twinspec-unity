using UnityEngine;

public class AxisGizmo : MonoBehaviour
{
    [Header("Optional axis arrows")]
    public Transform xAxis;
    public Transform yAxis;
    public Transform zAxis;

    public float length = 0.5f;

    // Minimal API expected by RigController
    public void Refresh()
    {
        // If you haven't assigned arrows, do nothing.
        if (xAxis != null) { xAxis.localPosition = Vector3.zero; xAxis.localRotation = Quaternion.identity; xAxis.localScale = new Vector3(length, length, length); }
        if (yAxis != null) { yAxis.localPosition = Vector3.zero; yAxis.localRotation = Quaternion.identity; yAxis.localScale = new Vector3(length, length, length); }
        if (zAxis != null) { zAxis.localPosition = Vector3.zero; zAxis.localRotation = Quaternion.identity; zAxis.localScale = new Vector3(length, length, length); }
    }
}