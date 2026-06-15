using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject selector;
    [SerializeField] private SpriteRenderer visualRangeRenderer;

    private void Awake()
    {
        if (visualRangeRenderer != null) visualRangeRenderer.enabled = false;
    }

    private void Start()
    {
        selector.SetActive(false);
    }

    public void InitializeVisualRange(float towerRange)
    {
        if (towerRange > 0f) //algunas torres no tienen rango
        {
            Vector3 upscaledVisualRange = new Vector3(towerRange * 2f, towerRange * 2f, 1f);
            visualRangeRenderer.transform.localScale = upscaledVisualRange;
        }
        else
        {
            if (visualRangeRenderer != null) visualRangeRenderer.enabled = false;
        }
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
        selector.SetActive(true);
    }

    public void DeselectTower()
    {
        selector.SetActive(false);
    }

    public void Reset()
    {
        if (visualRangeRenderer != null) visualRangeRenderer.transform.localScale = new Vector3( 1f, 1f, 1f );
    }
}
