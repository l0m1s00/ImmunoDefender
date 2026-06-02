using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(content))
        {
            TooltipPanelUI.Instance.OpenTooltipPanel(content);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipPanelUI.Instance != null)
        {
            TooltipPanelUI.Instance.CloseTooltipPanel();
        }
    }

    private void OnDisable()
    {
        if (TooltipPanelUI.Instance != null)
        {
            TooltipPanelUI.Instance.CloseTooltipPanel();
        }
    }
}
