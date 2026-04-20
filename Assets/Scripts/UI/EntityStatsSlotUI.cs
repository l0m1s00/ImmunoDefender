using UnityEngine;
using TMPro;

public class EntityStatsSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI values;

    public void Initialize(string name, string initialValue, string value, string extraValues)
    {
        statName.text = name;

        string valuesString = ""; //se recoge en una cadena el valor base de la estadistica + valor actual + cantidad extra por buffos (mejoras no aplica)
        if (initialValue != null) valuesString += "(" + initialValue + ") base | ";
        valuesString += value;
        if (extraValues != null) valuesString += extraValues;

        values.text = valuesString;
    }
}
