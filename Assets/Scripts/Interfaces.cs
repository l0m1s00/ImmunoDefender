using System;
using System.Collections.Generic;

public interface IResettable
{
    void Reset();
}

public interface ISelectable
{
    void OnSelect();
    void OnDeselect();
    string GetEntityName();
    List<EntitiyStat> GetStats();
    event Action OnStatsChanged;
    event Action<ISelectable> OnEntityDestroyed;
}
