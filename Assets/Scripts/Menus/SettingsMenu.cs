using UnityEngine;

public class SettingsMenu : BaseMenu
{
    private SettingsMenuUI settingsMenuUI_Ref;

    private void OnEnable()
    {
        MenusManager.OnOpenSettings += Open;
    }

    private void OnDisable()
    {
        MenusManager.OnOpenSettings -= Open;
    }

    protected override void Start()
    {
        base.Start();
        settingsMenuUI_Ref = (SettingsMenuUI)baseMenuUI_Ref;
    }

    protected override void Open()
    {
        base.Open();
    }

    protected override void Close()
    {
        base.Close();
    }
}
