using System.Collections;              // <-- needed for IEnumerator
using UnityEngine;

public class PuzzleSphere : MonoBehaviour
{
    public Color offColor = Color.gray;
    public Color onColor = Color.white;
    public float litTime = 0.5f;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        SetOff();
    }

    // Default light-up using litTime
    public void LightUp()
    {
        LightUp(litTime);
    }

    // Light-up with custom duration
    public void LightUp(float overrideLitTime)
    {
        StopAllCoroutines();
        StartCoroutine(LightUpRoutine(overrideLitTime));
    }

    private IEnumerator LightUpRoutine(float time)
    {
        rend.material.color = onColor;
        yield return new WaitForSeconds(time);
        SetOff();
    }

    public void SetOff()
    {
        rend.material.color = offColor;
    }
}
