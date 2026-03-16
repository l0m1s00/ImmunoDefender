using UnityEngine;

public class TowerManagerUI : MonoBehaviour
{
    [Header("Visual Tower Ghost Feedback")]
    [SerializeField] private Color invalidColor;
    private Color validColor;
    private SpriteRenderer towerToBuildRangeRenderer = null; //rango de la torre a construir
    private SpriteRenderer ghostTowerRenderer = null;

    public void SetupGhostRender(GameObject towerGhostObj)
    {
        towerToBuildRangeRenderer = towerGhostObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ghostTowerRenderer = towerGhostObj.GetComponent<SpriteRenderer>();
        validColor = ghostTowerRenderer.color;
        validColor.a = invalidColor.a;
        ghostTowerRenderer.enabled = true;
    }

    public void SetGhostRenderVisualization(bool active)
    {
        if (ghostTowerRenderer != null)
        {
            if (active)
            {
                towerToBuildRangeRenderer.enabled = true;
                ghostTowerRenderer.color = validColor;
            }
            else
            {
                towerToBuildRangeRenderer.enabled = false;
                ghostTowerRenderer.color = invalidColor;
            }
        }
    }

    public void ResetGhostRender()
    {
        ghostTowerRenderer.color = validColor;
        ghostTowerRenderer = null;
        towerToBuildRangeRenderer.enabled = false;
        towerToBuildRangeRenderer = null;
    }
}
