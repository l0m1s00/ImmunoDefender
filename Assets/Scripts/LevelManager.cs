using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Components")]
    [SerializeField] private LevelManagerUI levelManagerUIref;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetMapButton;
    private bool hasWaveStarted = false;
    private Coroutine waveRestTimeCoroutine;

    [Header("Sounds")]
    [SerializeField] private AudioEventData startWaveAudioEvent;
    [SerializeField] private AudioEventData endWaveAudioEvent;

    [Header("Start Game Screen")] //MOVER A UI
    [SerializeField] private GameObject startGameScreen;

    [Header("Waves Library")]
    [SerializeField] private WavesLibrary wavesLibrary;
    private float waveRestTimer;
    private float currentWaveRestTime;

    [Header("Vital Organ")]
    [SerializeField] private VitalOrganData liverVitalOrganData;
    private VitalOrgan vitalOrganOnScene;

    //EVENTOS
    public static event Action OnStartWave;
    public static event Action<WaveConfig> OnStartWaveCurrentWaveData;
    public static event Action OnStartGame;
    public static event Action OnResetGameMap;
    public static event Action OnEndGame;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        startWaveButton.onClick.AddListener(ForceWaveStart);
        startButton.onClick.AddListener(StartGame);
        resetMapButton.onClick.AddListener(ResetMap);
    }

    private void OnEnable()
    {
        Enemy.OnAttackVitalOrgan += AttackVitalOrgan;
        BodyStates.OnAttackVitalOrgan += AttackVitalOrgan;
        EnemySpawner.OnWaveEnded += EndWave;
    }

    private void OnDisable()
    {
        Enemy.OnAttackVitalOrgan -= AttackVitalOrgan;
        BodyStates.OnAttackVitalOrgan -= AttackVitalOrgan;
        EnemySpawner.OnWaveEnded -= EndWave;
    }

    private void Start()
    {
        InitializeVitalOrgan();
    }

    private void InitializeVitalOrgan()
    {
        //tomar un organo vital para posicionarlo en el mapa
        float x = EntitiesLibrary.Instance.GetSelectedDifficultyData().width - 2.5f;
        float y = EntitiesLibrary.Instance.GetSelectedDifficultyData().height / 2;
        Vector2 pos = new Vector2(x, y);
        GameObject newVitalOrgan = PoolEntitiesManager.Instance.SpawnVitalOrgan(liverVitalOrganData);
        newVitalOrgan.transform.position = pos;

        vitalOrganOnScene = newVitalOrgan.GetComponent<VitalOrgan>();
        vitalOrganOnScene.Initialize(liverVitalOrganData);
    }

    private void AttackVitalOrgan(float damage)
    {
        bool endGame = vitalOrganOnScene.TakeDamage(damage);

        if (endGame)
        {
            EndGame();
        } 
    }

    private void EndGame()
    {
        //StopCoroutine(waveRestTimeCoroutine);
        OnEndGame?.Invoke();
    }

    private void EndWave()
    {
        hasWaveStarted = false;
        EconomyManager.Instance.WaveEndedAddBonus(wavesLibrary.GetCurrentWaveBonus());
        AudioManager.Instance.PlaySFX(endWaveAudioEvent, transform.position, 0f);
        WaveRest();
    }

    private void WaveRest()
    {
        currentWaveRestTime = Mathf.RoundToInt(wavesLibrary.GetCurrentWaveRestTime());
        waveRestTimer = currentWaveRestTime;
        wavesLibrary.IncreaseWaveIndex();
        levelManagerUIref.DisplayRestWaveTimer(Mathf.RoundToInt(waveRestTimer));
        waveRestTimeCoroutine = StartCoroutine(StartRest());
    }

    private void StartInitialRest()
    {
        currentWaveRestTime = Mathf.RoundToInt(wavesLibrary.GetInitialRest());
        waveRestTimer = currentWaveRestTime;
        levelManagerUIref.DisplayRestWaveTimer(Mathf.RoundToInt(waveRestTimer));
        waveRestTimeCoroutine = StartCoroutine(StartRest());
    }

    private void ForceWaveStart()
    {
        StopCoroutine(waveRestTimeCoroutine);
        StartWave();
    }

    private void StartWave()
    {
        OnStartWaveCurrentWaveData?.Invoke(wavesLibrary.GetCurrentWave());
        OnStartWave?.Invoke();
        AudioManager.Instance.PlaySFX(startWaveAudioEvent, transform.position, 0f);
        hasWaveStarted = true;
        ResetWaveRestTimer();
    }

    private IEnumerator StartRest()
    {
        while (waveRestTimer > 0f)
        {
            waveRestTimer -= Time.deltaTime;
            float normalizedTime = waveRestTimer / currentWaveRestTime;
            levelManagerUIref.UpdateRestWaveTimerText(Mathf.CeilToInt(waveRestTimer), normalizedTime);
            yield return null;
        }

        StartWave();

        yield return null;
    }

    public bool HasWaveStarted()
    {
        return hasWaveStarted;
    }

    public string GetCurrentWaveIndexString()
    {
        return wavesLibrary.GetCurrentWaveIndexString();
    }

    private void ResetWaveRestTimer()
    {
        waveRestTimer = 0f;
        levelManagerUIref.HideRestWaveTimer();
    }

    private void StartGame()
    {
        OnStartGame?.Invoke();
        StartInitialRest();
        startGameScreen.SetActive(false);
    }

    private void ResetMap()
    {
        OnResetGameMap?.Invoke();
    }
}
