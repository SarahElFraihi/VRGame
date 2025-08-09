using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
    public Transform player;                 // Assign in Inspector if you want, or it will auto-find
    public float rotationSpeed = 2f;
    public float maxRotationAngle = 90f;     // ±90° from original forward

    private Quaternion originalRotation;
    private bool warnedOnce;

    void Start()
    {
        originalRotation = transform.rotation;

        // Auto-find if not assigned
        if (player == null)
        {
            var cam = Camera.main; // requires the VR camera to be tagged MainCamera
            if (cam != null) player = cam.transform;
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Fallback try (in case camera wasn’t ready at Start)
            var anyCam = FindAnyObjectByType<Camera>();
            if (anyCam != null) player = anyCam.transform;

            if (player == null)
            {
                if (!warnedOnce)
                {
                    Debug.LogWarning("[SurveillanceCamera] No player/camera assigned. " +
                                     "Assign XR Main Camera in Inspector or tag your VR camera as MainCamera.");
                    warnedOnce = true;
                }
                return; // prevent null ref
            }
        }

        Vector3 targetPosition = player.position + Vector3.up * 0.1f; // aim around head height
        Vector3 direction = targetPosition - transform.position;
        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        float angle = Quaternion.Angle(originalRotation, targetRot);
        if (angle > maxRotationAngle)
            targetRot = Quaternion.RotateTowards(originalRotation, targetRot, maxRotationAngle);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}
