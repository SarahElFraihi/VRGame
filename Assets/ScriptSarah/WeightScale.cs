using TMPro;
using UnityEngine;

public class WeightScale : MonoBehaviour
{
    [Header("Goal")]
    public int targetWeight = 15;

    [Header("UI (optional)")]
    public TextMeshPro currentText;
    public TextMeshPro targetText;

    [Header("UI Colors")]
    public Color tooLightColor = Color.white;
    public Color tooHeavyColor = new Color(1f, 0.85f, 0.3f); // warm
    public Color okColor = Color.green;

    [Header("Reward")]
    public GameObject codeRevealObject;

    private readonly System.Collections.Generic.HashSet<WeightItem> onScale = new();
    private int currentWeight;

    void Start()
    {
        if (targetText) targetText.text = $"{targetWeight} kg";
        UpdateUI();
        if (codeRevealObject) codeRevealObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<WeightItem>();
        if (item != null && onScale.Add(item))
        {
            currentWeight += item.weightValue;
            UpdateUI();
            CheckWeight();
        }
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.GetComponentInParent<WeightItem>();
        if (item != null && onScale.Remove(item))
        {
            currentWeight -= item.weightValue;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (!currentText) return;

        currentText.text = $"{currentWeight} kg";
        if (currentWeight == targetWeight) currentText.color = okColor;
        else if (currentWeight > targetWeight) currentText.color = tooHeavyColor;
        else currentText.color = tooLightColor;
    }

    void CheckWeight()
    {
        if (currentWeight == targetWeight)
        {
            if (codeRevealObject) codeRevealObject.SetActive(true);
            Debug.Log("Correct weight! Code revealed.");
        }
    }
}
