using UnityEngine;

public class AltarSlot : MonoBehaviour, Interactable, ItemRequierement
{
    [Header("Item Requirement")]
    [SerializeField] private int requiredItemID;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject activatedVisual;
    [SerializeField] private GameObject deactivatedVisual;

    private bool isActivated = false;
    public bool IsActivated => isActivated;

    public int ItemID => requiredItemID;

    private void Start()
    {
        UpdateVisuals();
    }

    public void Interact()
    {
        if (isActivated) return;

        ActivateSlot();
    }

    private void ActivateSlot()
    {
        if (isActivated) return;

        isActivated = true;
        UpdateVisuals();

        SoundAltar();

        AltarManager manager = FindAltarManager();
        if (manager != null)
        {
            manager.OnSlotActivated(this);
        }
    }

    private void UpdateVisuals()
    {
        if (activatedVisual != null)
        {
            activatedVisual.SetActive(isActivated);
        }

        if (deactivatedVisual != null)
        {
            deactivatedVisual.SetActive(!isActivated);
        }
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
        UpdateVisuals();
    }

    private AltarManager FindAltarManager()
    {
        AltarManager manager = GetComponentInParent<AltarManager>();

        if (manager == null)
        {
            manager = FindAnyObjectByType<AltarManager>();
        }

        return manager;
    }

    public void SoundAltar()
    {
        AudioManager.Instance.PlaySFX3D("Fuego", transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isActivated ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}

