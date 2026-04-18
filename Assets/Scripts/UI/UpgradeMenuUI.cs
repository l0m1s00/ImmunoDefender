using TMPro;
using UnityEngine;

public class UpgradeMenuUI : BaseMenuPanelUI
{
    [SerializeField] private TextMeshProUGUI towerTypeText;

    public void InitializePanelElements(TowerData data)
    {
        towerTypeText.text = data.towerName;
        DisplayUpgradePanel();
    }

    private void DisplayUpgradePanel()
    {
        Open();
    }

    public void CloseUpgradePanel()
    {
        Close();
    }
}
