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
    public Color tooHeavyColor = new Color(1f, 0.85f, 0.3f);
    public Color okColor = Color.green;

    [Header("Reward")]
    public GameObject codeRevealObject;

    [Header("Audio")]
    public AudioSource successSfx;     // assign in Inspector
    public AudioClip successClip;      // optional: if set, uses PlayOneShot

    private readonly System.Collections.Generic.HashSet<WeightItem> onScale = new();
    private int currentWeight;
    private bool solved;

    void Start()
    {
        if (targetText) targetText.text = $"{targetWeight} kg";
        UpdateUI();
        if (codeRevealObject) codeRevealObject.SetActive(false);
        solved = false;
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
        if (solved) return;
        if (currentWeight == targetWeight)
        {
            solved = true;

            if (codeRevealObject) codeRevealObject.SetActive(true);

            if (successSfx)
            {
                if (successClip) successSfx.PlayOneShot(successClip);
                else successSfx.Play();
            }

            // Optional: prevent further changes
            // GetComponent<Collider>().enabled = false;
        }
    }
}
