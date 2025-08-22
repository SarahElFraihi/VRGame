using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortraitPuzzleManager : MonoBehaviour
{
    [Header("Setup")]
    public PortraitTarget[] portraits; // drag your 3 portraits here in any order
    public DoorController door;        // your door script/animator trigger

    [Header("Flow")]
    public int requiredCount = 3;      // how many to select
    public float resetDelay = 1.2f;    // delay before auto-reset if wrong

    int currentOrder = 0;
    bool solved = false;
    bool checking = false;

    public bool CanAcceptClick() => !solved && !checking && currentOrder < requiredCount;

    // Called by PortraitTarget when clicked; returns order index (1..N)
    public int RegisterChoice(PortraitTarget t)
    {
        currentOrder++;
        if (currentOrder >= requiredCount)
        {
            checking = true;
            Invoke(nameof(CheckSolution), 0.05f);
        }
        return currentOrder;
    }

    void CheckSolution()
    {
        // Collect chosen portraits ordered by chosenOrder
        var chosen = portraits.Where(p => p.isChosen)
                              .OrderBy(p => p.chosenOrder)
                              .ToArray();

        if (chosen.Length != requiredCount)
        {
            checking = false;
            return;
        }

        // Validate order by their ageRank (0 youngest …)
        bool correct = true;
        for (int i = 0; i < requiredCount; i++)
        {
            if (chosen[i].ageRank != i) { correct = false; break; }
        }

        if (correct)
        {
            solved = true;
            OnSolved();
        }
        else
        {
            Invoke(nameof(ResetPuzzle), resetDelay);
        }

        checking = false;
    }

    void OnSolved()
    {
        // Show a “code number” or symbol if you want:
        // e.g., enable a mesh or set a TMP text in the room.
        // Then unlock/open the door:
        if (door != null) door.UnlockAndOpen();
    }

    public void ResetPuzzle()
    {
        currentOrder = 0;
        foreach (var p in portraits) p.ClearChoice();
    }
}
