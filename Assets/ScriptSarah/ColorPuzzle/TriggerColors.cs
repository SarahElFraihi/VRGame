using UnityEngine;

public class TriggerColors : MonoBehaviour
{
    public AudioSource aiPuzzleAudio;           // AI camera voice
    public ColorSequenceManager sequence;       // Your sequence script
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;
            StartCoroutine(StartPuzzleAfterVoice());
        }
    }

    private System.Collections.IEnumerator StartPuzzleAfterVoice()
    {
        aiPuzzleAudio.Play();
        yield return new WaitForSeconds(aiPuzzleAudio.clip.length);
        sequence.PlaySequence(); // Start the color sequence
    }
}
