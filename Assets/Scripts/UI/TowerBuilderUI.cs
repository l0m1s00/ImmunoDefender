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
        towerToBuildRangeRenderer = towerGhostObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //hacer que el valor logico de rango coincida con el elemento visual de rango
        Vector3 upscaledVisualRange = new Vector3(towerRange * 2 + 1, towerRange * 2 + 1, 1f);
        towerToBuildRangeRenderer.transform.localScale = upscaledVisualRange;
        ghostTowerRenderer = towerGhostObj.transform.GetChild(1).GetComponent<SpriteRenderer>();
        ghostTowerRenderer.enabled = true;
        validColor = ghostTowerRenderer.color;
        validColor.a = invalidColor.a;
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
        towerToBuildRangeRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
        towerToBuildRangeRenderer = null;
    }
}
