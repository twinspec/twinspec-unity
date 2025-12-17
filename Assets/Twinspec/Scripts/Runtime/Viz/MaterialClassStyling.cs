using UnityEngine;

public class MaterialClassStyling : MonoBehaviour
{
    [Header("Assign in Inspector (optional)")]
    public Renderer targetRenderer;

    [Tooltip("Fallback color if you haven't wired a palette yet.")]
    public Color defaultColor = new Color(0.7f, 0.3f, 0.3f, 1f);

    // Existing convenience API
    public void Apply(string materialClass) => Apply(materialClass, null);

    // Existing 2-arg API (string, string)
    public void Apply(string materialClass, string processTag)
    {
        if (targetRenderer == null) return;

        Color c = defaultColor;

        if (!string.IsNullOrEmpty(materialClass))
        {
            string key = materialClass.ToLowerInvariant();
            if (key.Contains("face")) c = new Color(0.71f, 0.35f, 0.35f, 1f);
            else if (key.Contains("edge")) c = new Color(0.31f, 0.55f, 0.78f, 1f);
            else if (key.Contains("oxide") || key.Contains("powder")) c = new Color(0.75f, 0.75f, 0.75f, 1f);
        }

        if (!string.IsNullOrEmpty(processTag))
        {
            string p = processTag.ToLowerInvariant();
            if (p.Contains("anneal")) c *= 1.05f;
            else if (p.Contains("sva")) c *= 1.08f;
            else if (p.Contains("as_cast") || p.Contains("as-cast") || p.Contains("cast")) c *= 0.95f;
            c.a = 1f;
        }

        var mpb = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", c);
        mpb.SetColor("_Color", c);
        targetRenderer.SetPropertyBlock(mpb);
    }

    // NEW: Overload that matches RigController passing a Renderer first
    public void Apply(Renderer renderer, string materialClass)
    {
        targetRenderer = renderer;
        Apply(materialClass, null);
    }

    // NEW: Overload that matches RigController passing (Renderer, materialClass, processTag)
    public void Apply(Renderer renderer, string materialClass, string processTag)
    {
        targetRenderer = renderer;
        Apply(materialClass, processTag);
    }
}