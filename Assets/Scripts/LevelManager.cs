using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////////////////////  ENEMIES WAVE SYSTEM ///////////////////////////////////////////////
[System.Serializable]
public struct EnemyGroup
{
    public EnemyData enemyData; //tipo de enemigo del grupo
    public int count; //numero de enemigos del grupo
    public float timeBetweenEnemies; //delay entre enemigos del mismo grupo
    public float delayAfterGroup; //descanso al terminar de mandar al grupo
}

[System.Serializable]
public struct WaveConfig
{
    public List<EnemyGroup> enemyGroups; //grupos de enemigos que componen una oleada
    public float restWaitTime; //tiempo de espera tras finalizar la oleada
}
///////////////////////////////////////////////////////////////////////////////////////////////////

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Components")]
    [SerializeField] private LevelManagerUI levelManagerUIref;
    [SerializeField] private Button startWaveButton;
    private bool hasWaveStarted = false;
    private Coroutine waveRestTimeCoroutine;
    [Header("First Rest Wave Settings")]
    [SerializeField] private float initialRestTime;
    private float waveRestTimer;
    [Header("Vital Organ")]
    [SerializeField] private VitalOrganData liverVitalOrganData;
    private VitalOrgan vitalOrganOnScene;
    [Header("Wave Configuration")]
    [SerializeField] private List<WaveConfig> waves;
    [SerializeField] private float delayBeforeWaveStarts;
    private int currentWaveIndex = 0;

    //EVENTOS
    public static event Action OnStartWave;
    public static event Action<WaveConfig> OnStartWaveCurrentWaveData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        startWaveButton.onClick.AddListener(ForceWaveStart);
    }

    private void OnEnable()
    {
        Enemy.OnAttackVitalOrgan += AttackVitalOrgan;
        NegativeStates.OnAttackVitalOrgan += AttackVitalOrgan;
        StartGameButtons.OnStartGame += StartInitialRest;
        EnemySpawner.OnWaveEnded += WaveRest;
    }

    private void OnDisable()
    {
        Enemy.OnAttackVitalOrgan -= AttackVitalOrgan;
        NegativeStates.OnAttackVitalOrgan -= AttackVitalOrgan;
        StartGameButtons.OnStartGame -= StartInitialRest;
        EnemySpawner.OnWaveEnded -= WaveRest;
    }

    private void Start()
    {
        InitializeVitalOrgan();
    }

    private void InitializeVitalOrgan()
    {
        //tomar un organo vital para posicionarlo en el mapa
        float x = DataLibrary.Instance.GetSelectedDifficultyData().width - 2.5f;
        float y = DataLibrary.Instance.GetSelectedDifficultyData().height / 2;
        Vector2 pos = new Vector2(x, y);
        GameObject newVitalOrgan = PoolEntitiesManager.Instance.SpawnVitalOrgan(liverVitalOrganData);
        newVitalOrgan.transform.position = pos;

        vitalOrganOnScene = newVitalOrgan.GetComponent<VitalOrgan>();
        vitalOrganOnScene.Initialize(liverVitalOrganData);
    }

    private void AttackVitalOrgan(float damage)
    {
        bool endGame = vitalOrganOnScene.TakeDamage(damage);

        //if (endGame) 
    }

    private void EndGame()
    {
        StopCoroutine(waveRestTimeCoroutine);
    }

    private void WaveRest()
    {
        waveRestTimer = Mathf.RoundToInt(waves[currentWaveIndex].restWaitTime);
        currentWaveIndex++;
        levelManagerUIref.DisplayRestWaveTimer(Mathf.RoundToInt(waveRestTimer));
        waveRestTimeCoroutine = StartCoroutine(StartRest());
    }

    private void StartInitialRest()
    {
        waveRestTimer = Mathf.RoundToInt(initialRestTime);
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
        OnStartWave?.Invoke();
        OnStartWaveCurrentWaveData?.Invoke(waves[currentWaveIndex]);
        hasWaveStarted = true;
        ResetWaveRestTimer();
    }

    private IEnumerator StartRest()
    {
        while (waveRestTimer > 0f)
        {
            waveRestTimer -= Time.deltaTime;
            levelManagerUIref.UpdateRestWaveTimerText(Mathf.RoundToInt(waveRestTimer));
            yield return null;
        }

        StartWave();

        yield return null;
    }

    public bool HasWaveStarted()
    {
        return hasWaveStarted;
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    private void ResetWaveRestTimer()
    {
        waveRestTimer = 0f;
        levelManagerUIref.HideRestWaveTimer();
    }
}
