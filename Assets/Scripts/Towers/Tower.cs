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
