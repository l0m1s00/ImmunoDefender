using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class EntityStatsSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI values;

    private string tooltipData;

    public void Initialize(string name, string initialValue, string value, string extraValues)
    {
        statName.text = name;
        tooltipData = extraValues;

        string valuesString = ""; //se recoge en una cadena el valor base de la estadistica + valor actual + cantidad extra por buffos (mejoras no aplica)
        if (!string.IsNullOrEmpty(initialValue)) valuesString += "(" + initialValue + ") base | ";
        valuesString += value;

        values.text = valuesString;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipData) && TooltipPanelUI.Instance != null)
        {
            TooltipPanelUI.Instance.OpenTooltipPanel(tooltipData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipData) && TooltipPanelUI.Instance != null)
        {
            TooltipPanelUI.Instance.CloseTooltipPanel();
        }
    }
}
