using UnityEngine;

public class OutlineOrderPuzzleManager : MonoBehaviour
{
    [Header("Assign your portraits (parents with XRSimpleInteractable + PortraitOrderOutline)")]
    public PortraitOrderOutline[] portraits; // 3 items

    [Header("Selection colors by order")]
    public Color firstColor = Color.green;
    public Color secondColor = Color.red;
    public Color thirdColor = Color.blue;

    [Header("Optional door")]
    public Animator doorAnimator;
    public string doorOpenTrigger = "Open";

    [Header("Flow")]
    public float wrongResetDelay = 1.0f; // seconds to show wrong attempt before reset

    int currentOrder = 0;   // 0..3
    bool solved = false;

    public void HandleClick(PortraitOrderOutline p)
    {
        if (solved) return;

        // Undo last step if clicking the last selected again
        if (p.isSelected && p.selectedIndex == currentOrder)
        {
            p.ClearOutline();
            currentOrder--;
            return;
        }

        // Already selected but not last → ignore (keeps order stable)
        if (p.isSelected) return;

        // Accept ANY selection order
        currentOrder++;
        var color = (currentOrder == 1) ? firstColor :
                    (currentOrder == 2) ? secondColor : thirdColor;

        p.ApplyOutline(color, currentOrder);

        if (currentOrder == portraits.Length)
        {
            CheckSolution();
        }
    }

    void CheckSolution()
    {
        // Build array of selected portraits in the order they were clicked (1..3)
        var clicked = new PortraitOrderOutline[portraits.Length];
        foreach (var po in portraits)
        {
            if (po.isSelected && po.selectedIndex >= 1 && po.selectedIndex <= portraits.Length)
                clicked[po.selectedIndex - 1] = po;
        }

        // Correct if ageRank matches 0,1,2 in the clicked order
        bool ok = true;
        for (int i = 0; i < clicked.Length; i++)
        {
            if (clicked[i] == null || clicked[i].ageRank != i) { ok = false; break; }
        }

        if (ok)
        {
            solved = true;
            OnSolved();
        }
        else
        {
            // Wrong attempt: show outlines briefly, then reset
            Invoke(nameof(ResetPuzzle), wrongResetDelay);
        }
    }

    void OnSolved()
    {
        if (doorAnimator && !string.IsNullOrEmpty(doorOpenTrigger))
            doorAnimator.SetTrigger(doorOpenTrigger);
        // Leave outlines on as confirmation
    }

    public void ResetPuzzle()
    {
        solved = false;
        currentOrder = 0;
        foreach (var p in portraits) p.ClearOutline();
    }
}
