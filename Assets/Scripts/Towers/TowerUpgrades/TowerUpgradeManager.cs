using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    [SerializeField] private UpgradeMenuUI upgradeMenuUI_Ref;
    [SerializeField] private Button demolishTowerButton;
    private Tower currentSelectedTower;

    //EVENTOS
    public static event Action<Tower> OnTowerDemolish;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        demolishTowerButton.onClick.AddListener(DemolishTower);
    }

    //al pulsar en el boton de demoler torre se le devuelve al jugador un porcentaje de la cantidad total de recursos invertida en la torre
    private void DemolishTower()
    {
        OnTowerDemolish?.Invoke(currentSelectedTower);
    }

    public void InitializeUpgradeMenu(Tower tower)
    {
        currentSelectedTower = tower;
        upgradeMenuUI_Ref.InitializePanelElements(currentSelectedTower.GetTowerStats().baseData);
    }

    public void ResetUpgradeMenu()
    {
        currentSelectedTower = null;
        upgradeMenuUI_Ref.CloseUpgradePanel();
    }
}
