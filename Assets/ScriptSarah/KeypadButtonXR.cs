using UnityEngine;


[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class KeypadButtonXR : MonoBehaviour
{
    public NavKeypad.Keypad keypad;  // drag your Keypad component here
    public string value = "0";       // set per key: "0".."9" or "enter"

    void Awake()
    {
        var xri = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        xri.selectEntered.AddListener(_ => Press());
    }

    public void Press()
    {
        Debug.Log("Pressed " + value);
        if (keypad != null) keypad.AddInput(value);
    }
}
