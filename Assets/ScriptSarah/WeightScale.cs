using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightScale : MonoBehaviour
{
    [Header("Goal")]
    public int targetWeight = 15;

    [Header("UI (optional)")]
    public TextMeshPro currentText;   // shows current total
    public TextMeshPro targetText;    // shows target on the screen

    [Header("Reward")]
    public GameObject codeRevealObject;

    private readonly HashSet<WeightItem> onScale = new HashSet<WeightItem>();
    private int currentWeight;

    void Start()
    {
        if (targetText) targetText.text = targetWeight.ToString();
        if (currentText) currentText.text = "0";
        if (codeRevealObject) codeRevealObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<WeightItem>(); // catches child colliders
        if (item && onScale.Add(item))
        {
            currentWeight += item.weightValue;
            UpdateUI();
            CheckWeight();
        }
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.GetComponentInParent<WeightItem>();
        if (item && onScale.Remove(item))
        {
            currentWeight -= item.weightValue;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (currentText) currentText.text = currentWeight.ToString();
    }

    void CheckWeight()
    {
        if (currentWeight == targetWeight)
        {
            if (codeRevealObject) codeRevealObject.SetActive(true);
            Debug.Log("Correct weight! Code revealed.");
            // Optional: disable the trigger so players can’t break it after success
            // GetComponent<Collider>().enabled = false;
        }
    }
}
