using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeypadButton : MonoBehaviour
{
    public enum Kind { Digit, Backspace, Clear, Enter }
    [Header("Behavior")]
    public Kind kind = Kind.Digit;
    public string digit = "0";                 // used if Kind == Digit
    public KeypadController controller;

    [Header("Visuals")]
    public Renderer targetRenderer;            // leave empty to use Renderer on this object
    public Color idleColor = Color.white;
    public Color pressedColor = Color.green;

    [Header("Debounce")]
    [Tooltip("Minimum seconds between accepted presses.")]
    public float pressCooldown = 0.15f;

    [Header("Debug")]
    public bool logWhoPressed = false;

    private XRSimpleInteractable xri;
    private Renderer rend;
    private float lastPressTime = -999f;

    void Awake()
    {
        xri = GetComponent<XRSimpleInteractable>();
        rend = targetRenderer ? targetRenderer : GetComponent<Renderer>();
        if (rend) rend.material.color = idleColor; // start color
    }

    void OnEnable()
    {
        xri.selectEntered.AddListener(OnSelectEntered);
        xri.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        xri.selectEntered.RemoveListener(OnSelectEntered);
        xri.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Debounce: ignore if pressed too soon after the last press
        if (Time.time - lastPressTime < pressCooldown) return;
        lastPressTime = Time.time;

        if (logWhoPressed)
        {
            var who = args.interactorObject?.transform?.name ?? "unknown";
            Debug.Log($"[KeypadButton] pressed by {who} -> {digit}");
        }

        if (rend) rend.material.color = pressedColor;

        switch (kind)
        {
            case Kind.Digit:     controller.PressKey(digit); break;
            case Kind.Backspace: controller.Backspace();      break;
            case Kind.Clear:     controller.ClearAll();       break;
            case Kind.Enter:     controller.Submit();         break;
        }
    }

    private void OnSelectExited(SelectExitEventArgs _)
    {
        if (rend) rend.material.color = idleColor;
    }
}
