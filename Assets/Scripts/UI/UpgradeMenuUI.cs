using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenuUI : BasePanelUI
{
    [SerializeField] private TextMeshProUGUI towerTypeText;

    [Header("Upgrades Elements")]
    [SerializeField] private Transform upgradesContainer; //verticallayout
    [SerializeField] private GameObject upgradeSlotPrefab;

    public void InitializePanelElements(TowerData data, Dictionary<TowerUpgradeData, int> towerUpgrades)
    {
        towerTypeText.text = data.towerName;
        CreateUpgradesSlots(towerUpgrades);
        DisplayUpgradePanel();
    }

    private void CreateUpgradesSlots(Dictionary<TowerUpgradeData, int> towerUpgrades)
    {
        //limpiar slots de otra torre anteriormente seleccionada
        foreach (Transform upgradeSlot in upgradesContainer)
        {
            Destroy(upgradeSlot.gameObject);
        }

        //por si no hubiera mejoras
        if (towerUpgrades == null || towerUpgrades.Count == 0) return;

        //crear slots de las mejoras asociadas a la torre
        foreach (KeyValuePair<TowerUpgradeData, int> upgrade in towerUpgrades)
        {
            TowerUpgradeData upgradeData = upgrade.Key;
            int upgradeCurrentLevel = upgrade.Value;

            GameObject newSlotObj = Instantiate(upgradeSlotPrefab, upgradesContainer);

            if (newSlotObj.TryGetComponent<UpgradeSlotUI>(out UpgradeSlotUI upgradeSlotUI))
            {
                upgradeSlotUI.InitializeUpgradeSlot(upgradeData, upgradeCurrentLevel);
            }
        }
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
