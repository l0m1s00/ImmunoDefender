using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    [Header("Components")]
    [SerializeField] private UpgradeMenuUI upgradeMenuUI_Ref;
    [SerializeField] private HoldConfirmationButton demolishTowerButton;
    [SerializeField] AudioEventData buyUpgradeSound;
    [SerializeField] AudioEventData demolishTowerSound;
    private Tower currentSelectedTower;

    //EVENTOS
    public static event Action<Tower> OnTowerDemolish;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        demolishTowerButton.onHoldCompleted.AddListener(DemolishTower);
    }

    //al pulsar en el boton de demoler torre se le devuelve al jugador un porcentaje de la cantidad total de recursos invertida en la torre
    private void DemolishTower()
    {
        AudioManager.Instance.PlaySFX(demolishTowerSound, currentSelectedTower.transform.position);
        OnTowerDemolish?.Invoke(currentSelectedTower);
    }

    public void InitializeUpgradeMenu(Tower tower)
    {
        currentSelectedTower = tower;

        upgradeMenuUI_Ref.InitializePanelElements(
            currentSelectedTower.GetTowerStats().baseData,
            currentSelectedTower.GetTowerStats().currentUpgradesByLevel
        );
    }

    public void TryBuyUpgrade(TowerUpgradeData upgradeData, UpgradeSlotUI upgradeSlotUI)
    {
        Dictionary<TowerUpgradeData, int> towerUpgrades = currentSelectedTower.GetTowerStats().currentUpgradesByLevel;
        int currentLevel = towerUpgrades[upgradeData];

        //por seguridad
        if (currentLevel >= upgradeData.GetMaxLevel()) return;

        int cost = upgradeData.costPerLevel[currentLevel];

        if (EconomyManager.Instance.CanAfford(cost))
        {
            EconomyManager.Instance.SpendCurrency(cost);

            //añadir coste de la mejora a la inversion total de la torre
            EconomyManager.Instance.IncreaseInvestmentOfTower(currentSelectedTower, cost);

            //determinar tipo de mejora y aplicar
            currentSelectedTower.ApplyUpgrade(upgradeData, currentLevel);

            //cambiar nivel de la mejora
            towerUpgrades[upgradeData]++;
            int newLevel = towerUpgrades[upgradeData];
            upgradeSlotUI.InitializeUpgradeSlot(upgradeData, newLevel);

            AudioManager.Instance.PlaySFX(buyUpgradeSound, transform.position, 0f);
        }
        else
        {
            //no hay dinero para comprar la mejora
        }
    }

    public void ResetUpgradeMenu()
    {
        currentSelectedTower = null;
        upgradeMenuUI_Ref.CloseUpgradePanel();
    }
}
