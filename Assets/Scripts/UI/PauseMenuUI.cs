using UnityEngine;

public class PauseMenuUI : BaseMenuUI
{
    private void OnEnable()
    {
        PauseMenu.OnPauseGame += HandlePause;
    }

    private void OnDisable()
    {
        PauseMenu.OnPauseGame -= HandlePause;
    }

    private void HandlePause(bool isPaused)
    {
        if (isPaused) Open();
        else Close();
    }
}
