using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class EntityStatsSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI values;

    private string tooltipData; //el tooltipo muestra explicaciones, si las hubiera, sobre la estadistica

    public void Initialize(string name, string initialValue, string value, string statExplanation)
    {
        statName.text = name;
        tooltipData = statExplanation;

        string valuesString = ""; //se recoge en una cadena el valor base de la estadistica + valor actual + cantidad extra por buffos (mejoras no aplica)
        if (!string.IsNullOrEmpty(initialValue)) valuesString += "(" + initialValue + ") base | ";
        valuesString += value;

        values.text = valuesString;

        //no todas las estadisticas deben mostrar una descripcion
        if (!string.IsNullOrEmpty(tooltipData))
        {
            TooltipTrigger tooltip = this.GetComponent<TooltipTrigger>();
            if (tooltip == null)
            {
                tooltip = this.gameObject.AddComponent<TooltipTrigger>();
            }
            tooltip.content = tooltipData;
        }
    }
}
