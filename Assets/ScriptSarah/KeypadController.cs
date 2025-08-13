using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("Code")]
    [SerializeField] private string targetCode = "2736";
    [SerializeField] private bool autoSubmitAtLength = false;

    [Header("UI")]
    [SerializeField] private TMP_Text display;
    [SerializeField] private string hiddenChar = "•";
    [SerializeField] private int maxLength = 4;

    [Header("Result")]
    [SerializeField] private GameObject revealOnSuccess;
    public UnityEvent onCorrect;
    public UnityEvent onWrong;

    private string input = "";

    public void PressKey(string k)
    {
        if (string.IsNullOrEmpty(k)) return;

        // accept only 0-9 from buttons
        char c = k[0];
        if (c < '0' || c > '9') return;

        // keep a rolling window the same length as the target code
        int targetLen = targetCode.Length;
        if (input.Length >= targetLen)
            input = input.Substring(1); // drop oldest

        input += c;
        RefreshDisplay();

        Debug.Log($"[Keypad] +{c}  buffer='{input}'");

        if (autoSubmitAtLength && input.Length == targetLen)
            Submit();
    }

    public void Backspace()
    {
        if (input.Length == 0) return;
        input = input.Substring(0, input.Length - 1);
        RefreshDisplay();
        Debug.Log($"[Keypad] backspace -> '{input}'");
    }

    public void ClearAll()
    {
        input = "";
        RefreshDisplay();
        Debug.Log("[Keypad] clear");
    }

    public void Submit()
    {
        Debug.Log($"[Keypad] submit: buffer='{input}' target='{targetCode}' len={input.Length}");
        if (input == targetCode)
        {
            Debug.Log("[Keypad] ✅ correct");
            if (revealOnSuccess) revealOnSuccess.SetActive(true);
            onCorrect?.Invoke();
        }
        else
        {
            Debug.Log("[Keypad] ❌ wrong");
            onWrong?.Invoke();
        }
        // optional: clear after submit
        input = "";
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (!display) return;
        int n = input.Length;
        display.text = new string(hiddenChar[0], n);
    }
}
