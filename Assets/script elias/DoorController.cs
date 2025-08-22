using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator anim;   // has a bool "Open" or trigger "Open"
    public bool locked = true;

    public void UnlockAndOpen()
    {
        locked = false;
        if (anim != null) anim.SetBool("Open", true); // or SetTrigger("Open")
        // Or rotate door via script if you don’t use an Animator
    }
}
