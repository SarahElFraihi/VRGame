using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

[RequireComponent(typeof(XRSimpleInteractable))]
public class StartPuzzleButtonVR : MonoBehaviour
{
    public ColorSequenceManager sequenceManager;
    public AudioSource clickSfx;
    public Renderer targetRenderer;    // optional; leave empty to use this object's Renderer
    public Color idleColor = Color.white;
    public Color pressedColor = Color.green;
    public float pressFlashTime = 0.15f;
    public float pressCooldown = 0.2f;

    XRSimpleInteractable xri;
    Renderer rend;
    float lastPress = -999f;

    void Awake()
    {
        xri = GetComponent<XRSimpleInteractable>();
        xri.selectEntered.AddListener(OnPressed);

        rend = targetRenderer ? targetRenderer : GetComponent<Renderer>();
        if (rend) rend.material.color = idleColor;
    }

    void OnDestroy()
    {
        if (xri) xri.selectEntered.RemoveListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs _)
    {
        if (!sequenceManager) return;
        if (Time.time - lastPress < pressCooldown) return;
        lastPress = Time.time;

        clickSfx?.Play();
        if (rend) StartCoroutine(Flash());
        sequenceManager.PlaySequence();
    }

    IEnumerator Flash()
    {
        var c = rend.material.color;
        rend.material.color = pressedColor;
        yield return new WaitForSeconds(pressFlashTime);
        rend.material.color = c;
    }
}
