using UnityEngine;

public class TowerBuilderUI : MonoBehaviour
{
    [Header("Visual Tower Ghost Feedback")]
    [SerializeField] private Color invalidColor;
    private Color validColor;
    private SpriteRenderer towerToBuildRangeRenderer = null; //rango de la torre a construir
    private SpriteRenderer ghostTowerRenderer = null;

    public void SetupGhostRender(GameObject towerGhostObj, float towerRange)
    {
        if (towerRange > 0f) //algunas torres no tienen rango porque no tienen que interactuar con otras torres o con enemigos
        {
            Transform rangeTransform = towerGhostObj.transform.Find("Range");

            if (rangeTransform != null)
            {
                towerToBuildRangeRenderer = rangeTransform.GetComponent<SpriteRenderer>();
                Vector3 upscaledVisualRange = new Vector3(towerRange * 2 + 1, towerRange * 2 + 1, 1f);
                towerToBuildRangeRenderer.transform.localScale = upscaledVisualRange;
                towerToBuildRangeRenderer.enabled = true;
            }
        }
        else
        {
            towerToBuildRangeRenderer= null;
        }

        //como tanto el rango como la torre son sprites, determinamos cual de ellos es el del fantasma
        ghostTowerRenderer = towerGhostObj.GetComponentInChildren<SpriteRenderer>(true);
        SpriteRenderer[] allRenderers = towerGhostObj.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer renderer in allRenderers)
        {
            if (renderer != towerToBuildRangeRenderer)
            {
                ghostTowerRenderer = renderer;
                break;
            }
        }

        if (ghostTowerRenderer != null)
        {
            ghostTowerRenderer.enabled = true;
            validColor = ghostTowerRenderer.color;
            validColor.a = invalidColor.a;
        }
    }

    public void SetGhostRenderVisualization(bool active)
    {
        if (ghostTowerRenderer != null)
        {
            if (active)
            {
                if (towerToBuildRangeRenderer != null) towerToBuildRangeRenderer.enabled = true;
                ghostTowerRenderer.color = validColor;
            }
            else
            {
                if (towerToBuildRangeRenderer != null) towerToBuildRangeRenderer.enabled = false;
                ghostTowerRenderer.color = invalidColor;
            }
        }
    }

    public void ResetGhostRender()
    {
        ghostTowerRenderer.color = validColor;
        ghostTowerRenderer = null;
        if (towerToBuildRangeRenderer != null) towerToBuildRangeRenderer.enabled = false;
        if (towerToBuildRangeRenderer != null) towerToBuildRangeRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
        towerToBuildRangeRenderer = null;
    }
}
