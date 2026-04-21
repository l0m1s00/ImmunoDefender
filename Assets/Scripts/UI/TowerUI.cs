using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private SpriteRenderer outline;
    [SerializeField] private SpriteRenderer visualRangeRenderer;

    private void Awake()
    {
        visualRangeRenderer.enabled = false;
    }

    private void Start()
    {
        outline.enabled = false;
    }

    public void InitializeVisuals(float towerRange)
    {
        Vector3 upscaledVisualRange = new Vector3(towerRange * 2 + 1, towerRange * 2 + 1, 1f);
        visualRangeRenderer.transform.localScale = upscaledVisualRange;
    }

    public void ShowVisualRange()
    {
        visualRangeRenderer.enabled = true;
    }

    public void HideVisualRange()
    {
        visualRangeRenderer.enabled = false;
    }

    public void SelectTower()
    {
        outline.enabled = true;
    }

    public void DeselectTower()
    {
        outline.enabled = false;
    }

    public void Reset()
    {
        visualRangeRenderer.transform.localScale = new Vector3( 1f, 1f, 1f );
    }
}
