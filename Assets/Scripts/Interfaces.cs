using System;
using System.Collections.Generic;

public interface IResettable //requisito de toda entidad que pertenece al pool de optimizacion de entidades
{
    void Reset();
}

public interface ISelectable //requisitos de toda entidad seleccionable
{
    void OnSelect();
    void OnDeselect();
    string GetEntityName();
    List<EntitiyStat> GetEntityStatsToDisplay();
    event Action OnStatsChanged;
    event Action<ISelectable> OnEntityDestroyed;
}

public interface IStateInfoProvider //requisitos de toda entidad que debe mostrar información sobre distintas acciones que realice
{
    string GetCurrentStateInfo();
    event Action<string> OnStateInfoChanged;
}
