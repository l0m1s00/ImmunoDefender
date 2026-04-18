using UnityEngine;

public class PauseMenuUI : BaseMenuPanelUI
{
    private void OnEnable()
    {
        PauseMenu.OnPauseGame += HandlePause;
    }

    private void OnDisable()
    {
        PauseMenu.OnPauseGame -= HandlePause;
    }

    private void HandlePause()
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.Paused) Open();
        else Close();
    }
}
