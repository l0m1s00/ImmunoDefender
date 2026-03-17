using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButtons : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetMapButton;
    [Header("Start Game Screen")]
    [SerializeField] private GameObject startGameScreen;

    //EVENTOS
    public static event Action OnStartGame;
    public static event Action OnResetGameMap;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        resetMapButton.onClick.AddListener(OnResetMapButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        OnStartGame?.Invoke();
        startGameScreen.SetActive(false);
    }

    private void OnResetMapButtonClicked()
    {
        OnResetGameMap?.Invoke();
    }
}
