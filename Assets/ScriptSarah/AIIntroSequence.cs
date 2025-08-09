using UnityEngine;

public class AIIntroSequence : MonoBehaviour
{
    public AudioSource aiVoiceAudio;          // From the surveillance camera
    public Transform doorTransform;           // The door to rotates
    public AudioSource doorAudioSource;       // Sound when door opens
    public float openYRotation = -90f;        // Final Y rotation
    public float rotationSpeed = 90f;         // Degrees per second

    private bool doorOpening = false;
    private Quaternion targetRotation;

    void Start()
    {
        StartCoroutine(PlayVoiceAndOpenDoor());
    }

    private System.Collections.IEnumerator PlayVoiceAndOpenDoor()
    {
        aiVoiceAudio.Play();
        yield return new WaitForSeconds(aiVoiceAudio.clip.length);

        Vector3 currentEuler = doorTransform.eulerAngles;
        targetRotation = Quaternion.Euler(currentEuler.x, openYRotation, currentEuler.z);
        doorOpening = true;
        doorAudioSource.Play();
    }

    void Update()
    {
        if (doorOpening)
        {
            doorTransform.rotation = Quaternion.RotateTowards(
                doorTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Stop when very close
            if (Quaternion.Angle(doorTransform.rotation, targetRotation) < 0.5f)
            {
                doorTransform.rotation = targetRotation;
                doorOpening = false;
                Debug.Log("Door fully opened.");
            }
        }
    }
}
