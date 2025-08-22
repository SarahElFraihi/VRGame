using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup group;          // assign the CanvasGroup
    public float defaultDuration = 1f; // seconds
    public AudioSource sfx;            // optional, assign AudioSource
    public AudioClip clip;             // optional, assign clip if not on sfx

    Coroutine routine;

    void Reset()
    {
        if (!group) group = GetComponentInChildren<CanvasGroup>();
        if (!sfx)   sfx   = GetComponent<AudioSource>();
        if (group) group.alpha = 0f;
    }

    // Parameterless methods so you can call them from UnityEvents
    public void FadeToBlack()  => FadeTo(1f, defaultDuration, true);
    public void FadeFromBlack()=> FadeTo(0f, defaultDuration, false);

    public void FadeTo(float target, float duration, bool playSound)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeRoutine(target, duration, playSound));
    }

    IEnumerator FadeRoutine(float target, float duration, bool playSound)
    {
        if (group == null) yield break;

        if (playSound)
        {
            if (sfx && clip) sfx.PlayOneShot(clip);
            else if (sfx && sfx.clip) sfx.Play();
        }

        float start = group.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        group.alpha = target;
    }
}
