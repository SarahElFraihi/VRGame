using UnityEngine;

public class RoomEntryTrigger : MonoBehaviour
{
    public AudioSource aiPuzzleAudio;
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            aiPuzzleAudio.Play();
            hasPlayed = true;
        }
    }
}
