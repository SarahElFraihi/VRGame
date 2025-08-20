using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(Collider))]
public class KeypadButton : MonoBehaviour
{
    public enum Kind { Digit, Backspace, Clear, Enter }

    [Header("Behavior")]
    public Kind kind = Kind.Digit;
    public string digit = "0";                 // used if Kind == Digit
    public KeypadController controller;

    [Header("Visuals")]
    // Leave empty to use this GameObject's Renderer
    public Renderer targetRenderer;
    public Color idleColor = Color.white;
    public Color pressedColor = Color.green;

    [Header("Debounce")]
    public float pressCooldown = 0.15f;

    [Header("Debug")]
    public bool logWhoPressed;

    Renderer rend;
    XRSimpleInteractable interactable;
    float lastPressTime = -999f;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        rend = targetRenderer != null ? targetRenderer : GetComponent<Renderer>();

        // Initial color
        if (rend) rend.material.color = idleColor;

        // Subscribe to XR events
        interactable.selectEntered.AddListener(HandleSelectEntered);
        interactable.selectExited.AddListener(HandleSelectExited);
        // Optional: trigger press on Activate instead of Select
        interactable.activated.AddListener(_ => TryPress());
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(HandleSelectEntered);
            interactable.selectExited.RemoveListener(HandleSelectExited);
            interactable.activated.RemoveAllListeners();
        }
    }

    void HandleSelectEntered(SelectEnterEventArgs args)
    {
        // Change color immediately when the trigger grabs/selects
        if (rend) rend.material.color = pressedColor;
        TryPress(args);
    }

    void HandleSelectExited(SelectExitEventArgs args)
    {
        if (rend) rend.material.color = idleColor;
    }

    void TryPress(SelectEnterEventArgs _ = null)
    {
        if (Time.time - lastPressTime < pressCooldown) return;
        lastPressTime = Time.time;

        if (logWhoPressed)
            Debug.Log($"[Keypad] {(kind == Kind.Digit ? digit : kind.ToString())} pressed by {gameObject.name}");

        if (controller == null) return;

        switch (kind)
        {
            case Kind.Digit:     controller.PressKey(digit); break;
            case Kind.Backspace: controller.Backspace();      break;
            case Kind.Clear:     controller.ClearAll();       break;
            case Kind.Enter:     controller.Submit();         break;
        }
    }
}
