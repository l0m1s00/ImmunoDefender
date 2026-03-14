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
    }

    public void ResetGhostRender()
    {
        ghostTowerRenderer.color = validColor;
        towerToBuildRangeRenderer = null;
        ghostTowerRenderer = null;
    }

    public void SetGhostRenderVisualization(bool active)
    {
        if (ghostTowerRenderer != null)
        {
            if (active)
            {
                ghostTowerRenderer.color = validColor;
                towerToBuildRangeRenderer.enabled = true;
            }
            else
            {
                ghostTowerRenderer.color = invalidColor;
                towerToBuildRangeRenderer.enabled = false;
            }
        }
    }
}
