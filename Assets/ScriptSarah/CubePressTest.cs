using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class CubePressTest : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private Renderer rend;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        rend = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs _)
    {
        Debug.Log("✅ SELECT");
        if (rend) rend.material.color = Color.green;
    }

    void OnSelectExited(SelectExitEventArgs _)
    {
        Debug.Log("❌ DESELECT");
        if (rend) rend.material.color = Color.white;
    }
}
