using UnityEngine;
using System.Collections;

public class PuzzleSphere : MonoBehaviour
{
    public Color offColor = Color.gray;
    public Color onColor = Color.white;
    public float litTime = 0.5f;

    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        SetOff();
    }

    public void LightUp() => LightUp(litTime);

    public void LightUp(float time)
    {
        StopAllCoroutines();
        StartCoroutine(LightRoutine(time));
    }

    IEnumerator LightRoutine(float t)
    {
        rend.material.color = onColor;
        yield return new WaitForSeconds(t);
        SetOff();
    }

    public void SetOff()
    {
        rend.material.color = offColor;
    }
}
