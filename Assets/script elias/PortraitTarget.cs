using UnityEngine;

using TMPro;

public class PortraitTarget : MonoBehaviour
{
    [Header("Puzzle")]
    public int ageRank; // 0 = youngest, 1 = middle, 2 = oldest (your hidden truth)

    [Header("Label")]
    public Transform labelAnchor;          // empty child above the portrait
    public GameObject numberLabelPrefab;   // world-space small badge with TMP text
    private GameObject spawnedLabel;
    private TMP_Text labelText;

    [HideInInspector] public bool isChosen = false;
    [HideInInspector] public int chosenOrder = 0; // 1..N

    PortraitPuzzleManager manager;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        manager = FindObjectOfType<PortraitPuzzleManager>();
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // “Click” with trigger/primary or with mouse via simulator
        interactable.selectEntered.AddListener(_ => OnClicked());
        // Optional: allow Activate instead (enable “Activate” in Interactable)
        // interactable.activated.AddListener(_ => OnClicked());
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(_ => OnClicked());
    }

    void OnClicked()
    {
        if (manager == null || isChosen || !manager.CanAcceptClick()) return;

        isChosen = true;
        chosenOrder = manager.RegisterChoice(this); // returns 1..N

        ShowLabel(chosenOrder);
        // Optional: little punch animation / sound here
    }

    void ShowLabel(int order)
    {
        if (spawnedLabel == null && numberLabelPrefab != null && labelAnchor != null)
        {
            spawnedLabel = Instantiate(numberLabelPrefab, labelAnchor.position, labelAnchor.rotation, labelAnchor);
            labelText = spawnedLabel.GetComponentInChildren<TMP_Text>();
        }
        if (labelText != null) labelText.text = order.ToString();
        if (spawnedLabel != null) spawnedLabel.SetActive(true);
    }

    public void ClearChoice()
    {
        isChosen = false;
        chosenOrder = 0;
        if (spawnedLabel != null) spawnedLabel.SetActive(false);
    }
}
