using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    //Economy Settings
    private int startingCurrency;
    private int currentCurrency;

    public static event Action<int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        InitializeCurrency();
    }

    private void Start()
    {
        currentCurrency = startingCurrency;
        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    private void InitializeCurrency()
    {
        DifficultySettingsData difficultySettingsData = DataLibrary.Instance.GetSelectedDifficultyData();
        startingCurrency = difficultySettingsData.initialCurrency;
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        OnCurrencyChanged?.Invoke(currentCurrency);
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
