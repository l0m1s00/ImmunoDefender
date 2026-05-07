using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance;

    [Header("Buttons")]
    [SerializeField] private Button playGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button settingsButton;

    [Header("Audio")]
    [SerializeField] private AudioEventData ostEventData;

    //EVENTOS
    public static event Action OnOpenSettings;
    public static event Action OnPlay;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playGameButton.onClick.AddListener(PlayGame);
        quitGameButton.onClick.AddListener(Application.Quit);
        settingsButton.onClick.AddListener(OpenSettings);
        AudioManager.Instance.PlayBGM(ostEventData);
    }

    private void OpenSettings()
    {
        OnOpenSettings?.Invoke();
    }

    private void PlayGame()
    {
        OnPlay?.Invoke();
    }
}
