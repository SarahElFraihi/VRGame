using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class PaintingButton : MonoBehaviour
{
    public SequencePuzzleController controller;

    [Header("Visuals")]
    public Renderer targetRenderer;                 // leave empty to use own Renderer
    [Tooltip("If true, the painting's current renderer color becomes the idle color at start.")]
    public bool useRendererColorAsIdle = true;
    public Color idleColor = Color.white;
    public Color wrongFlash = Color.red;
    public float flashDuration = 0.15f;

    [Header("Click Pulse")]
    public Transform pulseTarget;                   // leave empty = this transform
    public float pressScale = 0.92f;
    public float pressTime = 0.06f;
    public float releaseTime = 0.08f;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    Renderer rend;
    Coroutine flashCo, pulseCo;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        rend = targetRenderer ? targetRenderer : GetComponent<Renderer>();
        if (!pulseTarget) pulseTarget = transform;

        // Capture original color as idle if desired
        if (rend)
        {
            if (useRendererColorAsIdle) idleColor = rend.material.color;
            else rend.material.color = idleColor; // explicitly set from inspector
        }

        interactable.selectEntered.AddListener(OnSelect);
    }

    void OnDestroy()
    {
        if (interactable) interactable.selectEntered.RemoveListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs _)
    {
        // tiny press pulse
        if (pulseCo != null) StopCoroutine(pulseCo);
        pulseCo = StartCoroutine(PressPulse());

        controller?.TryPress(this);
    }

    public void SetColor(Color c)
    {
        if (rend) rend.material.color = c;
    }

    public void ResetVisual()
    {
        SetColor(idleColor);
    }

    public void FlashWrong()
    {
        if (flashCo != null) StopCoroutine(flashCo);
        flashCo = StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        if (!rend) yield break;
        var prev = rend.material.color;
        rend.material.color = wrongFlash;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = prev; // controller will reset after delay
    }

    public void SetEnabled(bool v)
    {
        var col = GetComponent<Collider>(); if (col) col.enabled = v;
        if (interactable) interactable.enabled = v;
    }

    IEnumerator PressPulse()
    {
        Vector3 start = pulseTarget.localScale;
        Vector3 down = start * pressScale;

        float t = 0f;
        while (t < pressTime)
        {
            t += Time.deltaTime;
            pulseTarget.localScale = Vector3.Lerp(start, down, t / pressTime);
            yield return null;
        }

        t = 0f;
        while (t < releaseTime)
        {
            t += Time.deltaTime;
            pulseTarget.localScale = Vector3.Lerp(down, start, t / releaseTime);
            yield return null;
        }

        pulseTarget.localScale = start;
    }
}
