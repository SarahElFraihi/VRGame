using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequencePuzzleController : MonoBehaviour
{
    [Tooltip("Drag the portraits here in the REQUIRED order.")]
    public List<PaintingButton> sequence = new();

    [Header("Reveal on success")]
    public GameObject revealOnSuccess;      // e.g., a TextMeshPro number, set inactive at start
    public AudioSource successSfx;
    public AudioSource errorSfx;

    [Header("Step visuals")]
    public Color[] stepColors;              // optional colors per correct step (fallback = green)
    public TextMeshPro progressText;        // optional "1/3" display

    [Header("Timing")]
    [Tooltip("Delay before resetting after a wrong click (lets sound/flash play).")]
    public float wrongResetDelay = 0.35f;

    int index;
    bool solved;
    bool resetting;

    void Start()
    {
        if (revealOnSuccess) revealOnSuccess.SetActive(false);
        ResetSequence();
    }

    public void TryPress(PaintingButton pressed)
    {
        if (solved || resetting || sequence.Count == 0) return;

        if (pressed == sequence[index])
        {
            // Correct step: turn this one green (or step color) and lock it
            var c = (stepColors != null && stepColors.Length > index) ? stepColors[index] : Color.green;
            pressed.SetColor(c);
            pressed.SetEnabled(false);
            index++;
            UpdateUI();

            // Completed full sequence
            if (index >= sequence.Count)
            {
                solved = true;
                if (successSfx) successSfx.Play();
                if (revealOnSuccess) revealOnSuccess.SetActive(true);
                foreach (var p in sequence) p.SetEnabled(false);
            }
        }
        else
        {
            // Wrong: flash the one they clicked, play fail, then reset ALL back to their idle colors
            if (errorSfx) errorSfx.Play();
            pressed.FlashWrong();
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        resetting = true;
        yield return new WaitForSeconds(wrongResetDelay);
        ResetSequence();
        resetting = false;
    }

    public void ResetSequence()
    {
        solved = false;
        index = 0;
        foreach (var p in sequence)
        {
            p.ResetVisual();    // back to each painting's own idle/original color
            p.SetEnabled(true);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (progressText) progressText.text = $"{index}/{sequence.Count}";
    }
}
