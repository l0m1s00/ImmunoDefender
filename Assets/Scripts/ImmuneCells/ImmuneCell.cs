using System;
using UnityEngine;
using System.Collections.Generic;

//distintos estados en los que puede estar una celula (soldado)
public enum State
{
    None,
    Chase,
    Attack,
    Return,
    Clean, //Exclusivo de los macrofagos
}

public abstract class ImmuneCell : MonoBehaviour, IResettable, ISelectable
{
    [Header("Components")]
    [SerializeField] private ImmuneCellUI immuneCellUIRef;

    protected BasicCellStats basicStats;
    protected Tower headquarters; //torre que ha generado la celula
    protected Enemy currentTarget;

    //EVENTOS
    public event Action OnStatsChanged;
    public event Action<ISelectable> OnEntityDestroyed;

    protected virtual void Update()
    {
        
    }

    public virtual void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        headquarters = parentTower;
        currentTarget = target;
        basicStats = new BasicCellStats(data);
    }

    protected void NotifyStatsChanged()
    {
        OnStatsChanged?.Invoke();
    }

    protected void NotifyEntityDestruction()
    {
        OnEntityDestroyed?.Invoke(this);
    }

    public virtual void Reset()
    {
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

    public virtual List<EntitiyStat> GetStats()
    {
        return null;
    }
}
