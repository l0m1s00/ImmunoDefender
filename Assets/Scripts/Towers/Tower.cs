using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour, IResettable, ISelectable
{
    [Header("Components")]
    [SerializeField] private TowerUI towerUIRef;

    protected BasicTowerStats basicStats;

    public virtual void InitializeTower(TowerData data)
    {
        basicStats = new BasicTowerStats(data);
    }

    public virtual void OnSelect()
    {
        TowerUpgradeManager.Instance.InitializeUpgradeMenu(basicStats);
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

    public virtual List<EntitiyStat> GetStats()
    {
        return null;
    }

    public virtual void Reset() { }

}
