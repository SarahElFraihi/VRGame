using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PortraitOrderOutline : MonoBehaviour
{
    [Header("References")]
    public Renderer outlineRenderer;   // MeshRenderer or SpriteRenderer on the Outline child
    public GameObject outlineObject;   // the Outline child GameObject (enable/disable)

    [Header("Order / Age")]
    [Tooltip("0 = youngest, 1 = middle, 2 = oldest")]
    public int ageRank;

    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public int selectedIndex = 0; // 1..N when selected

    private OutlineOrderPuzzleManager manager;
    private XRSimpleInteractable xr;

    void Awake()
    {
        manager = FindObjectOfType<OutlineOrderPuzzleManager>();

        // Ensure XR click works
        xr = GetComponent<XRSimpleInteractable>();
        if (!xr) xr = gameObject.AddComponent<XRSimpleInteractable>();
        xr.selectEntered.AddListener(_ => manager.HandleClick(this));

        // Mouse fallback (editor)
        var proxy = gameObject.GetComponent<MouseClickProxy>();
        if (!proxy) proxy = gameObject.AddComponent<MouseClickProxy>();
        proxy.onMouseDown = () => manager.HandleClick(this);

        if (outlineObject) outlineObject.SetActive(false);
    }

    public void ApplyOutline(Color c, int index)
    {
        isSelected = true;
        selectedIndex = index;

        if (outlineRenderer != null)
        {
            // Works with URP/Lit (“_BaseColor”) and Unlit/Color (“_BaseColor” as well).
            var mpb = new MaterialPropertyBlock();
            outlineRenderer.GetPropertyBlock(mpb);
            mpb.SetColor("_BaseColor", c);
            outlineRenderer.SetPropertyBlock(mpb);

            // Also set Renderer.color if it's a SpriteRenderer (safety)
            if (outlineRenderer is SpriteRenderer sr) sr.color = c;
        }

        if (outlineObject) outlineObject.SetActive(true);
    }

    public void ClearOutline()
    {
        isSelected = false;
        selectedIndex = 0;
        if (outlineObject) outlineObject.SetActive(false);
    }
}

// Simple mouse helper for testing without VR
public class MouseClickProxy : MonoBehaviour
{
    public System.Action onMouseDown;
    void OnMouseDown() => onMouseDown?.Invoke();
}
