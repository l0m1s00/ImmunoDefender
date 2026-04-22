using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private SpriteRenderer outline;
    [SerializeField] private SpriteRenderer visualRangeRenderer;

    private void Awake()
    {
        if (visualRangeRenderer != null) visualRangeRenderer.enabled = false;
    }

    private void Start()
    {
        outline.enabled = false;
    }

    public void InitializeVisuals(float towerRange)
    {
        if (towerRange > 0f) //algunas torres no tienen rango
        {
            Vector3 upscaledVisualRange = new Vector3(towerRange * 2 + 1, towerRange * 2 + 1, 1f);
            visualRangeRenderer.transform.localScale = upscaledVisualRange;
        }
        else visualRangeRenderer = null;
    }

    public void ShowVisualRange()
    {
        if (visualRangeRenderer != null) visualRangeRenderer.enabled = true;
    }

    public void HideVisualRange()
    {
        if (visualRangeRenderer != null) visualRangeRenderer.enabled = false;
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
        if (visualRangeRenderer != null) visualRangeRenderer.transform.localScale = new Vector3( 1f, 1f, 1f );
    }
}
