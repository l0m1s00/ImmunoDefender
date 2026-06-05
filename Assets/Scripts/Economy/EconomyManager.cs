using System;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economy Settings")]
    [SerializeField][Range(0f, 1f)] private float returnInvestmentPer;
    [Header("Clean Enemy Wave Bonus")]
    [SerializeField][Range(0f, 1f)] private float enemiesBonusPer; //porcentaje de la recomenpensa del enemigo
    private int currentCleanWaveBonus = 0; //cuantos mas enemigos sean derrotados, mayor sera

    private int startingCurrency;
    private int currentCurrency;

    //Inversion total del jugador en cada torre construida
    private Dictionary<Tower, int> playerInvestment = new Dictionary<Tower, int>();

    public static event Action<int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += AddTowerToInvestment;
        TowerUpgradeManager.OnTowerDemolish += ReturnInvestment;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= AddTowerToInvestment;
        TowerUpgradeManager.OnTowerDemolish -= ReturnInvestment;
    }

    private void Start()
    {
        InitializeCurrency();
        currentCurrency = startingCurrency;
        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    private void AddTowerToInvestment(Tower newBuiltTower)
    {
        int buildCost = newBuiltTower.GetTowerStats().baseData.cost;
        playerInvestment.Add(newBuiltTower, buildCost);
    }

    public void IncreaseInvestmentOfTower(Tower tower, int amount)
    {
        if (playerInvestment.ContainsKey(tower))
        {
            playerInvestment[tower] += amount;
        }
    }

    private void ReturnInvestment(Tower tower)
    {
        int amount = Mathf.RoundToInt(playerInvestment[tower] * returnInvestmentPer);
        AddImmediateCurrency(amount);
        playerInvestment.Remove(tower);
    }

    private void InitializeCurrency()
    {
        DifficultySettingsData difficultySettingsData = EntitiesLibrary.Instance.GetSelectedDifficultyData();
        startingCurrency = difficultySettingsData.initialCurrency;
    }

    public void AddImmediateCurrency(int amount)
    {
        currentCurrency += amount;
        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    public void AddCurrencyByEnemyDefeat(int amount) //amount == recompensa del enemigo
    {
        currentCurrency += amount;
        OnCurrencyChanged?.Invoke(currentCurrency);
        AddCurrencyBonus(amount);
    }

    private void AddCurrencyBonus(int amount)
    {
        currentCleanWaveBonus += Mathf.RoundToInt(amount * enemiesBonusPer);
    }

    public void WaveEndedAddBonus(int waveBonusAmount)
    {
        currentCleanWaveBonus += waveBonusAmount;
        currentCurrency += currentCleanWaveBonus;
        OnCurrencyChanged?.Invoke(currentCurrency);
        currentCleanWaveBonus = 0;
    }

    public bool CanAfford(int amount)
    {
        return currentCurrency >= amount;
    }

    public void SpendCurrency(int amount)
    {
        if (CanAfford(amount))
        {
            currentCurrency -= amount;
            OnCurrencyChanged?.Invoke(currentCurrency);
        }
    }
}
