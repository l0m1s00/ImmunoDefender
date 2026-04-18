using UnityEngine;
using TMPro;

public class EntityStatsSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    public void Initialize(string name, string value)
    {
        statName.text = name;
        statValue.text = value;
    }
}
