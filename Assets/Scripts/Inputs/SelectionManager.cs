using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    private ISelectable currentSelectedElement;

    //EVENTOS
    public static event Action<string, List<EntitiyStat>> OnEntitySelectedForInfo;
    public static event Action OnEntityDeselected;

    private void OnEnable()
    {
        Inputs.OnLeftClickPerformed += HandleSelection;
        BLymphocyteTower.OnBuffGenerated += RefreshUI;
    }

    private void OnDisable()
    {
        Inputs.OnLeftClickPerformed -= HandleSelection;
        BLymphocyteTower.OnBuffGenerated -= RefreshUI;
    }

    private void HandleSelection()
    {
        //ignorar click sobre elementos de la interfaz
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Inputs.Instance.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            //determinar si el objeto clickado es seleccionable
            if (hit.collider.TryGetComponent<ISelectable>(out ISelectable selectableHit))
            {
                if (currentSelectedElement != selectableHit)
                {
                    SelectNewEntity(selectableHit);
                }
            }
            else //se detecta algo pero no es seleccionable
            {
                SelectNewEntity(null);
            }
        }
        else //se ha hecho click en la nada
        {
            SelectNewEntity(null);
        }
    }

    private void SelectNewEntity(ISelectable newEntity)
    {
        if (currentSelectedElement != null)
        {
            currentSelectedElement.OnStatsChanged -= RefreshUI;
            currentSelectedElement.OnEntityDestroyed -= HandleEntityDestroyed;
            currentSelectedElement.OnDeselect();
        }

        currentSelectedElement = newEntity;

        if (currentSelectedElement != null)
        {
            currentSelectedElement.OnStatsChanged += RefreshUI;
            currentSelectedElement.OnEntityDestroyed += HandleEntityDestroyed;
            currentSelectedElement.OnSelect();
            RefreshUI();
        }
        else
        {
            OnEntityDeselected?.Invoke();
        }
    }

    private void RefreshUI()
    {
        if (currentSelectedElement != null)
        {
            //tomamos la informacion de la entidad para mostrar en el panel de estadisticas
            OnEntitySelectedForInfo?.Invoke(currentSelectedElement.GetEntityName(), currentSelectedElement.GetStats());
        }
    }

    private void HandleEntityDestroyed(ISelectable destroyedEntity)
    {
        if (currentSelectedElement == destroyedEntity)
        {
            SelectNewEntity(null);
        }
    }
}
