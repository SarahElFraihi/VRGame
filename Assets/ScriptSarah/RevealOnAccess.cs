using TMPro;
using UnityEngine;

public class RevealOnAccess : MonoBehaviour
{
    [Header("What to reveal")]
    public GameObject toReveal;         // Assign your wall number object (can be this gameObject)
    public TMP_Text numberLabel;        // Optional: assign if you want to set text at runtime
    public string textToShow = "7";     // The number you want to show on the wall
    public AudioSource sfx;             // Optional: a reveal sound

    private bool revealed = false;

    public void Reveal()
    {
        if (revealed) return;
        revealed = true;

        if (toReveal != null) toReveal.SetActive(true);
        if (numberLabel != null) numberLabel.text = textToShow;
        if (sfx != null) sfx.Play();
    }
}
