using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//MOVER
[System.Serializable]
public struct EntitiyStat
{
    public string statName;
    public string statValue;
    //public Image statIcon;
}

public class SelectionManager : MonoBehaviour
{
    private ISelectable currentSelectedElement;

    //EVENTOS
    public static event Action<string, List<EntitiyStat>> OnEntitySelectedForInfo;
    public static event Action OnEntityDeselected;

    private void OnEnable()
    {
        Inputs.OnLeftClickPerformed += HandleSelection;
    }

    private void OnDisable()
    {
        Inputs.OnLeftClickPerformed -= HandleSelection;
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
                    currentSelectedElement?.OnDeselect();
                    currentSelectedElement = selectableHit;
                    currentSelectedElement.OnSelect();

                    //tomamos la informacion de la entidad para mostrar en el panel de estadisticas
                    OnEntitySelectedForInfo?.Invoke(selectableHit.GetEntityName(), selectableHit.GetStats()); ;
                }
            }
            else
            {
                DeselectCurrentElement();
            }
        }
        else
        {
            DeselectCurrentElement();
        }
    }

    private void DeselectCurrentElement()
    {
        if (currentSelectedElement != null)
        {
            currentSelectedElement.OnDeselect();
            currentSelectedElement = null;
            OnEntityDeselected?.Invoke();
        }
    }
}
