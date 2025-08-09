using UnityEngine;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("References")]
    public PuzzleSphere[] spheres;     // index: 0=Green,1=Blue,2=Yellow,3=Red
    public ColorButtonVR[] buttons;    // same indexing as above

    [Header("Sequence (indices into arrays above)")]
    // Your pattern: Green, Blue, Yellow, Yellow, Red, Green, Blue, Yellow, Yellow, Yellow
    public int[] sequence = {0,1,2,2,3,0,1,2,2,2};

    [Header("Timing")]
    public float delayBetween = 0.25f;   // pause between lights during demo
    public float sphereLitTime = 0.5f;   // how long each sphere stays lit in demo

    [Header("SFX")]
    public AudioSource successSfx;
    public AudioSource failSfx;

    [Header("Reveal on success")]
    public GameObject numberToReveal;    // disabled at start; contains a "7"

    private int inputPos = 0;
    private bool isPlayingSequence = false;
    private bool acceptingInput = false;
    private bool completed = false;

    void Awake()
    {
        // wire button callbacks
        foreach (var b in buttons)
        {
            b.OnPressed += OnButtonPressed;
        }
        if (numberToReveal != null) numberToReveal.SetActive(false);
    }

    public void PlaySequence()
    {
        if (completed) return;
        StopAllCoroutines();
        StartCoroutine(SequenceRoutine());
    }

    private System.Collections.IEnumerator SequenceRoutine()
    {
        acceptingInput = false;
        isPlayingSequence = true;
        inputPos = 0;

        // Ensure all spheres are off
        foreach (var s in spheres) s.SetOff();

        // Play pattern
        foreach (int idx in sequence)
        {
            spheres[idx].LightUp(sphereLitTime);
            yield return new WaitForSeconds(sphereLitTime + delayBetween);
        }

        isPlayingSequence = false;
        acceptingInput = true;
        yield break;
    }

    private void OnButtonPressed(int idx)
    {
        if (completed || !acceptingInput) return;

        // optional feedback: light corresponding sphere briefly when player presses
        spheres[idx].LightUp(0.15f);

        // check vs expected
        if (idx == sequence[inputPos])
        {
            inputPos++;

            // finished the whole sequence correctly
            if (inputPos >= sequence.Length)
            {
                acceptingInput = false;
                completed = true;
                successSfx?.Play();
                if (numberToReveal != null) numberToReveal.SetActive(true); // reveal "7"
            }
        }
        else
        {
            // wrong → replay
            acceptingInput = false;
            failSfx?.Play();
            StartCoroutine(ReplayAfterDelay(0.8f));
        }
    }

    private System.Collections.IEnumerator ReplayAfterDelay(float wait)
    {
        yield return new WaitForSeconds(wait);
        PlaySequence();
    }
}
