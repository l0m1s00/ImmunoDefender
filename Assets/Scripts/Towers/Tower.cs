using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour, IResettable, ISelectable
{
    [Header("Components")]
    [SerializeField] protected TowerUI towerUIRef;

    protected BasicTowerStats basicStats;

    //EVENTOS
    public event Action OnStatsChanged;
    public event Action<ISelectable> OnEntityDestroyed;

    public virtual void Initialize(TowerData data)
    {
        basicStats = new BasicTowerStats(data);
    }

    public virtual void ApplyUpgrade(TowerUpgradeData upgradeData, int upgradeLevel)
    {
        //determinar si la mejora se debe aplicar sobre una torre o sobre una celula
        if (
            (upgradeData.upgradeTarget == UpgradeTarget.Cell || upgradeData.upgradeTarget == UpgradeTarget.SpawnerModule)
            && this.gameObject.TryGetComponent<CellSpawnerModule>(out CellSpawnerModule spawnerModule))
        {
            //delegar control al modulo generador de celulas
            spawnerModule.spawnerStats.ApplyUpgrade(upgradeData, upgradeLevel);
        }
        else
        {
            //delegar control a las estadisticas de la torre
            basicStats.ApplyUpgrade(upgradeData, upgradeLevel);

            //comprobar si la mejora supone un cambio visual para la torre
            if (upgradeData.upgradeType == UpgradeType.Range)
            {
                if (this is TargetingTower targetingTower)
                {
                    towerUIRef.InitializeVisualRange(targetingTower.GetTowerRange());
                }
            }

            NotifyStatsChanged();
        }
    }

    public void NotifyStatsChanged()
    {
        OnStatsChanged?.Invoke();
    }

    public virtual void OnSelect()
    {
        TowerUpgradeManager.Instance.InitializeUpgradeMenu(this);
        towerUIRef.SelectTower();
    }

    public virtual void OnDeselect()
    {
        TowerUpgradeManager.Instance.ResetUpgradeMenu();
        towerUIRef.DeselectTower();
    }

    public string GetEntityName()
    {
        return basicStats.baseData.towerName;
    }

    public BasicTowerStats GetTowerStats()
    {
        return basicStats;
    }

    public virtual List<EntitiyStat> GetEntityStatsToDisplay()
    {
        return basicStats.GetStatsToDisplay();
    }

    public virtual void Reset()
    {
        OnEntityDestroyed?.Invoke(this);
    }
}
