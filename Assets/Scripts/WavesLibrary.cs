using System.Collections.Generic;
using UnityEngine;

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
    public int geneticsReward; //recompensa por finalizar oleada
}
///////////////////////////////////////////////////////////////////////////////////////////////////

public class WavesLibrary : MonoBehaviour
{
    [Header("First Rest Wave Settings")]
    [SerializeField] private float initialRestTime;

    [Header("Normal Waves Configuration")]
    [SerializeField] private List<WaveConfig> waves;
    [SerializeField] private float delayBeforeWaveStarts;
    private int currentWaveIndex = 0;
    private bool isAdvancedWave = false;
    private bool isSpecialWave = false;
    private int currentSpecialWaveIndex;

    [Header("Advanced Waves Configuration")]
    [SerializeField] private int advancedGeneticsReward;
    [SerializeField] private float advancedRestWaitTime;
    [SerializeField] private int minEnemiesPerGroup;
    [SerializeField] private int maxEnemiesPerGroupN;
    private int enemiesPerGroup;
    [SerializeField] private int minGroupN;
    [SerializeField] private int maxGroupN;
    private int groupN;
    [SerializeField] private float minTimeBetweenEnemies;
    [SerializeField] private float maxTimeBetweenEnemies;
    private float timeBetweenEnemies;
    [SerializeField] private float minDelayAfterGroup;
    [SerializeField] private float maxDelayAfterGroup;
    private float delayAfterGroup;
    private List<EnemyData> enemyTypes;
    private EnemyData enemy;

    [Header("Special Waves Configuration")]
    [SerializeField] private List<WaveConfig> specialWaves;
    [SerializeField] private List<string> specialWavesNames;
    [SerializeField][Range(0f, 1f)] private float specialWavePropability;

    private void Start()
    {
        enemyTypes = EntitiesLibrary.Instance.GetEnemyTypes();
    }

    public WaveConfig GetCurrentWave()
    {
        if (isAdvancedWave)
        {
            if (isSpecialWave)
            {
                return specialWaves[currentSpecialWaveIndex];
            }
            else
            {
                return CreateRandomAdvancedWave();
            }
        }

        return waves[currentWaveIndex];
    }

    private WaveConfig CreateRandomAdvancedWave()
    {
        WaveConfig randomAdvanceWave = new WaveConfig();
        EnemyGroup randomEnemyGroup;
        List<EnemyGroup> randomEnemyGroups =  new List<EnemyGroup>();

        groupN = Random.Range(minGroupN, maxGroupN + 1);

        for (int i = 0; i < groupN; i++)
        {
            enemy = enemyTypes[Random.Range(0, enemyTypes.Count)];
            enemiesPerGroup = Random.Range(minEnemiesPerGroup, maxEnemiesPerGroupN + 1);
            timeBetweenEnemies = Random.Range(minTimeBetweenEnemies, maxTimeBetweenEnemies);
            delayAfterGroup = Random.Range(minDelayAfterGroup, maxDelayAfterGroup);

            randomEnemyGroup = new EnemyGroup();
            randomEnemyGroup.enemyData = enemy;
            randomEnemyGroup.count = enemiesPerGroup;
            randomEnemyGroup.timeBetweenEnemies = timeBetweenEnemies;
            randomEnemyGroup.delayAfterGroup = delayAfterGroup;

            randomEnemyGroups.Add(randomEnemyGroup);
        }

        randomAdvanceWave.enemyGroups = randomEnemyGroups;
        return randomAdvanceWave;
    }

    public void IncreaseWaveIndex()
    {
        currentWaveIndex++;

        if (!isAdvancedWave && currentWaveIndex >= waves.Count)
        {
            isAdvancedWave = true;
        }

        if (isAdvancedWave)
        {
            if (Random.value <= specialWavePropability)
            {
                isSpecialWave = true;
                currentSpecialWaveIndex = Random.Range(0, specialWaves.Count);
            }
            else
            {
                isSpecialWave = false;
            }
        }
    }

    public float GetCurrentWaveRestTime()
    {
        if (isAdvancedWave)
        {
            if (isSpecialWave) return specialWaves[currentSpecialWaveIndex].restWaitTime;
            return advancedRestWaitTime;
        }

        return waves[currentWaveIndex].restWaitTime;
    }

    public int GetCurrentWaveBonus()
    {
        if (isAdvancedWave)
        {
            if (isSpecialWave) return specialWaves[currentSpecialWaveIndex].geneticsReward;

            return advancedGeneticsReward;
        }

        return waves[currentWaveIndex].geneticsReward;
    }

    public string GetCurrentWaveIndexString()
    {
        if (isSpecialWave)
        {
            return currentWaveIndex + " | " + specialWavesNames[currentSpecialWaveIndex];
        }

        return currentWaveIndex.ToString();
    }

    public float GetInitialRest()
    {
        return initialRestTime;
    }
}
