using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class ColorButtonVR : MonoBehaviour
{
    public int buttonIndex;                     // 0=Green,1=Blue,2=Yellow,3=Red
    public Color pressLitColor = Color.white;   // quick flash when pressed
    public float pressLitTime = 0.15f;
    public AudioSource clickSfx;                // optional

    private Renderer rend;
    private Color original;

    public System.Action<int> OnPressed;        // set by manager

    void Awake()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);

        rend = GetComponent<Renderer>();
        if (rend != null) original = rend.material.color;
    }

    private void OnDestroy()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null) interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        clickSfx?.Play();
        Flash();
        OnPressed?.Invoke(buttonIndex);
    }

    private void Flash()
    {
        if (rend == null) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        rend.material.color = pressLitColor;
        yield return new WaitForSeconds(pressLitTime);
        rend.material.color = original;
    }
}
