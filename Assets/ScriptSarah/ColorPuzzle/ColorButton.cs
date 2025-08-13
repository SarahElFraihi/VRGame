using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System;
using System.Collections;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ColorButtonVR : MonoBehaviour
{
    [Tooltip("0=Green, 1=Blue, 2=Yellow, 3=Red")]
    public int buttonIndex;

    [Header("Press feedback")]
    public Color pressLitColor = Color.white;
    public float pressLitTime = 0.15f;
    public AudioSource clickSfx;
    public float pressCooldown = 0.15f;   // debounce

    public Action<int> OnPressed;

    XRSimpleInteractable xri;
    Renderer rend;
    Color original;
    float lastPress = -999f;

    void Awake()
    {
        xri = GetComponent<XRSimpleInteractable>();
        xri.selectEntered.AddListener(OnSelectEntered);

        rend = GetComponent<Renderer>();
        if (rend) original = rend.material.color;
    }

    void OnDestroy()
    {
        if (xri) xri.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs _)
    {
        if (Time.time - lastPress < pressCooldown) return;
        lastPress = Time.time;

        clickSfx?.Play();
        if (rend) StartCoroutine(Flash());
        OnPressed?.Invoke(buttonIndex);
    }

    IEnumerator Flash()
    {
        var c = rend.material.color;
        rend.material.color = pressLitColor;
        yield return new WaitForSeconds(pressLitTime);
        rend.material.color = c;
    }
}
