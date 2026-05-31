using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtraInfoPanelUI : BasePanelUI
{
    [SerializeField] private TextMeshProUGUI extraInfoText;

    private IStateInfoProvider currentEntityProvider; //entidad que envia la informacion

    private void OnEnable()
    {
        SelectionManager.OnEntitySelected += EntitySelected;
        SelectionManager.OnEntityDeselected += EntityDeselected;
    }

    private void OnDisable()
    {
        SelectionManager.OnEntitySelected -= EntitySelected;
        SelectionManager.OnEntityDeselected -= EntityDeselected;
    }

    private void EntityDeselected()
    {
        if (currentEntityProvider != null)
        {
            currentEntityProvider.OnStateInfoChanged -= UpdateInfoText;
            currentEntityProvider = null;
        }

        Close();
    }

    private void EntitySelected(ISelectable entity)
    {
        if (entity is IStateInfoProvider provider) //es una entidad que puede mandar informacion
        {
            currentEntityProvider = provider;
            currentEntityProvider.OnStateInfoChanged += UpdateInfoText;
            UpdateInfoText(currentEntityProvider.GetCurrentStateInfo());
            Open();
        }
        else //es una entidad que no puede, desactivar el panel si estuviera activo
        {
            EntityDeselected();
        }
    }

    private void UpdateInfoText(string newText)
    {
        extraInfoText.text = newText;
    }
}
