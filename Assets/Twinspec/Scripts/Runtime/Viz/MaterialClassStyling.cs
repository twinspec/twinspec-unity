using UnityEngine;

public class MaterialClassStyling : MonoBehaviour
{
    [Header("Assign in Inspector (optional)")]
    public Renderer targetRenderer;

    [Tooltip("Fallback color if you haven't wired a palette yet.")]
    public Color defaultColor = new Color(0.7f, 0.3f, 0.3f, 1f);

    // Minimal API expected by RigController.
    // Keep signature aligned with how you call it (see notes below).
    public void Apply(string materialClass)
    {
        if (targetRenderer == null) return;

        // Hackathon-simple mapping. Expand later.
        Color c = defaultColor;

        if (!string.IsNullOrEmpty(materialClass))
        {
            string key = materialClass.ToLowerInvariant();
            if (key.Contains("face")) c = new Color(0.71f, 0.35f, 0.35f, 1f);
            else if (key.Contains("edge")) c = new Color(0.31f, 0.55f, 0.78f, 1f);
            else if (key.Contains("oxide") || key.Contains("powder")) c = new Color(0.75f, 0.75f, 0.75f, 1f);
        }

        // Use a MaterialPropertyBlock to avoid material instancing spam.
        var mpb = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", c); // URP Lit
        mpb.SetColor("_Color", c);     // Built-in fallback
        targetRenderer.SetPropertyBlock(mpb);
    }
}