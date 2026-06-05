using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    private ISelectable currentSelectedElement;

    //EVENTOS
    public static event Action<string, List<EntitiyStat>> OnRefreshSelectedEntityInfo;
    public static event Action<ISelectable> OnEntitySelected;
    public static event Action OnEntityDeselected;

    private void OnEnable()
    {
        Inputs.OnLeftClickPerformed += HandleSelection;
        Inputs.OnRightClickPerformed += HandleCommand;
        BLymphocyteTower.OnBuffGenerated += RefreshUI;
    }

    private void OnDisable()
    {
        Inputs.OnLeftClickPerformed -= HandleSelection;
        Inputs.OnRightClickPerformed -= HandleCommand;
        BLymphocyteTower.OnBuffGenerated -= RefreshUI;
    }

    private void HandleCommand() //manejar la orden de click derecho para la entidad seleccionada
    {
        if (IsPointerOverRealUI()) return;

        //determinar si el click derecho "ha sido realizado" por una NK
        if (currentSelectedElement is NK_ImmuneCell nkCell && !nkCell.IsInvestigating())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Inputs.Instance.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<CellularFarmTower>(out CellularFarmTower targetFarm))
                {
                    if (!TowerManager.Instance.IsFarmInvestigated(targetFarm))
                    {
                        nkCell.SetInvestigation(targetFarm);
                    }
                }
            }
        }
    }

    private void HandleSelection()
    {
        //ignorar click sobre elementos de la interfaz
        if (IsPointerOverRealUI()) return;

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

    private bool IsPointerOverRealUI()
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Inputs.Instance.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    private void SelectNewEntity(ISelectable newEntity)
    {
        if (currentSelectedElement != null)
        {
            currentSelectedElement.OnSelectedEntityStatsChanged -= RefreshUI;
            currentSelectedElement.OnSelectedEntityDestroyed -= HandleEntityDestroyed;
            currentSelectedElement.OnDeselect();
        }

        currentSelectedElement = newEntity;

        if (currentSelectedElement != null)
        {
            currentSelectedElement.OnSelectedEntityStatsChanged += RefreshUI;
            currentSelectedElement.OnSelectedEntityDestroyed += HandleEntityDestroyed;
            currentSelectedElement.OnSelect();
            RefreshUI();

            OnEntitySelected?.Invoke(currentSelectedElement);
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
            OnRefreshSelectedEntityInfo?.Invoke(currentSelectedElement.GetEntityName(), currentSelectedElement.GetEntityStatsToDisplay());
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
