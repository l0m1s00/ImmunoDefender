using System;
using UnityEngine;
using System.Collections.Generic;

//distintos estados en los que puede estar una celula (soldado)
public enum State
{
    None,
    Chase, //Exclusivo de las Attacker
    Attack, //Exclusivo de las Attacker
    Return, //Exclusivo de las Attacker
    Clean, //Exclusivo de los macrofagos
    InvestigateFarm, //Exclusivo de las NK
}

public abstract class ImmuneCell : MonoBehaviour, IResettable, ISelectable
{
    [Header("Components")]
    [SerializeField] protected ImmuneCellUI immuneCellUIRef;

    protected BasicCellStats basicStats;
    protected Tower headquarters; //torre que ha generado la celula
    protected Enemy currentTarget;

    //EVENTOS
    public event Action OnSelectedEntityStatsChanged;
    public event Action<ISelectable> OnSelectedEntityDestroyed;

    protected virtual void Update()
    {
        
    }

    public virtual void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        headquarters = parentTower;
        currentTarget = target;
        basicStats = new BasicCellStats(data);
    }

    public virtual void ApplyUpgrade(UpgradeType upgradeType, float amount)
    {
        basicStats.ApplyUpgrade(upgradeType, amount);
        NotifyStatsChanged();
    }

    protected void NotifyStatsChanged()
    {
        OnSelectedEntityStatsChanged?.Invoke();
    }

    //protected void NotifyEntityDestruction()
    //{
    //    OnEntityDestroyed?.Invoke(this);
    //}

    public virtual void Reset()
    {
        OnSelectedEntityDestroyed?.Invoke(this);
        headquarters = null;
        currentTarget = null;
    }

    public virtual void OnSelect()
    {
        immuneCellUIRef.SelectCell();
    }

    public virtual void OnDeselect()
    {
        immuneCellUIRef.DeselectCell();
    }

    public string GetEntityName()
    {
        return basicStats.cellName;
    }

    public BasicCellStats GetStats()
    {
        return basicStats;
    }

    public virtual List<EntitiyStat> GetEntityStatsToDisplay()
    {
        return null;
    }
}
