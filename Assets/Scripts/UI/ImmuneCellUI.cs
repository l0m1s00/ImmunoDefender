using UnityEngine;

public class ImmuneCellUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private SpriteRenderer outline;

    protected virtual void Start()
    {
        outline.enabled = false;
    }

    public void SelectCell()
    {
        outline.enabled = true;
    }

    public void DeselectCell()
    {
        outline.enabled = false;
    }
}
