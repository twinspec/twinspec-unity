using UnityEngine;

public class MaterialClassStyling : MonoBehaviour
{
    [Header("Assign in Inspector (optional)")]
    public Renderer targetRenderer;

    [Tooltip("Fallback color if you haven't wired a palette yet.")]
    public Color defaultColor = new Color(0.7f, 0.3f, 0.3f, 1f);

    // Keep the 1-arg version for convenience.
    public void Apply(string materialClass)
    {
        Apply(materialClass, null);
    }

    // ✅ 2-arg overload to satisfy RigController (Apply takes 2 arguments)
    // Second arg is assumed to be a process tag (as_cast / SVA / annealed).
    public void Apply(string materialClass, string processTag)
    {
        if (targetRenderer == null) return;

        // Base mapping by material class
        Color c = defaultColor;

        if (!string.IsNullOrEmpty(materialClass))
        {
            string key = materialClass.ToLowerInvariant();
            if (key.Contains("face")) c = new Color(0.71f, 0.35f, 0.35f, 1f);
            else if (key.Contains("edge")) c = new Color(0.31f, 0.55f, 0.78f, 1f);
            else if (key.Contains("oxide") || key.Contains("powder")) c = new Color(0.75f, 0.75f, 0.75f, 1f);
        }

        // Optional: tiny visual nudge based on process tag (purely aesthetic)
        if (!string.IsNullOrEmpty(processTag))
        {
            string p = processTag.ToLowerInvariant();

            // Slight brighten for more ordered processing, slight dim for as-cast.
            if (p.Contains("anneal")) c *= 1.05f;
            else if (p.Contains("sva")) c *= 1.08f;
            else if (p.Contains("as_cast") || p.Contains("as-cast") || p.Contains("cast")) c *= 0.95f;

            c.a = 1f; // keep alpha stable
        }

        // Use a MaterialPropertyBlock to avoid material instancing spam.
        var mpb = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(mpb);

        // URP Lit uses _BaseColor; Built-in often uses _Color.
        mpb.SetColor("_BaseColor", c);
        mpb.SetColor("_Color", c);

        targetRenderer.SetPropertyBlock(mpb);
    }
}