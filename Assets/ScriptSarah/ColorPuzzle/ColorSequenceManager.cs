using UnityEngine;
using System.Collections;

public class ColorSequenceManager : MonoBehaviour
{
    [Header("References (order matters)")]
    // 0 = Green, 1 = Blue, 2 = Yellow, 3 = Red  ← keep this mapping everywhere
    public PuzzleSphere[] spheres;     // size 4, in the order above
    public ColorButtonVR[] buttons;    // size 4, in the same order

    [Header("Sequence (indices into the arrays above)")]
    // Example: Green, Blue, Yellow, Yellow, Red, Green, Blue, Yellow, Yellow, Yellow
    public int[] sequence = {0,1,2,2,3,0,1,2,2,2};

    [Header("Timing")]
    public float delayBetween = 0.25f;   // time between lights in the demo
    public float sphereLitTime = 0.5f;   // lit time per step in the demo

    [Header("SFX")]
    public AudioSource successSfx;
    public AudioSource failSfx;

    [Header("Reveal on success")]
    public GameObject numberToReveal;    // keep disabled in Hierarchy

    int inputPos = 0;
    bool acceptingInput = false;
    bool playingDemo = false;
    bool completed = false;

    void Awake()
    {
        // wire cube presses
        foreach (var b in buttons) b.OnPressed += OnButtonPressed;
        if (numberToReveal) numberToReveal.SetActive(false);
    }

    public void PlaySequence()
    {
        if (completed || playingDemo) return;
        StopAllCoroutines();
        StartCoroutine(DemoRoutine());
    }

    IEnumerator DemoRoutine()
    {
        acceptingInput = false;
        playingDemo = true;
        inputPos = 0;

        // reset visuals
        foreach (var s in spheres) s.SetOff();

        // play back the pattern
        foreach (var idx in sequence)
        {
            spheres[idx].LightUp(sphereLitTime);
            yield return new WaitForSeconds(sphereLitTime + delayBetween);
        }

        playingDemo = false;
        acceptingInput = true;
    }

    void OnButtonPressed(int idx)
    {
        if (completed || !acceptingInput) return;

        // small feedback on the pressed color
        spheres[idx].LightUp(0.15f);

        if (idx == sequence[inputPos])
        {
            inputPos++;
            if (inputPos >= sequence.Length)
            {
                // success!
                acceptingInput = false;
                completed = true;
                successSfx?.Play();
                if (numberToReveal) numberToReveal.SetActive(true);
            }
            return;
        }

        // wrong → replay the demo
        acceptingInput = false;
        failSfx?.Play();
        StartCoroutine(ReplayAfterDelay(0.8f));
    }

    IEnumerator ReplayAfterDelay(float wait)
    {
        yield return new WaitForSeconds(wait);
        PlaySequence();
    }
}
