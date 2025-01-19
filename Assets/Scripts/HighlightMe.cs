using UnityEngine;
using UnityEngine.Rendering;

public class HighlightMe : MonoBehaviour
{
    [SerializeField]private Renderer rendererr;
    private Color originalColor;
    [SerializeField][ColorUsageAttribute(false, true)] private Color highlightColor;
    private void Start()
    {
        originalColor = rendererr.material.color;
    }
    public void Highlight()
    {
        rendererr.material.color = highlightColor;
    }
    public void Unhighlight()
    {
        rendererr.material.color = originalColor;
    }
}
