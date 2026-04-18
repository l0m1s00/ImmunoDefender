using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private SpriteRenderer outline;

    private void Start()
    {
        outline.enabled = false;
    }

    public void SelectTower()
    {
        outline.enabled = true;
    }

    public void DeselectTower()
    {
        outline.enabled = false;
    }
}
