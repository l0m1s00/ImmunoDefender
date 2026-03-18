using TMPro;
using UnityEngine;

public class EconomyUI : MonoBehaviour
{
    [Header("Currency Components")]
    [SerializeField] private TMP_Text playerCurrencyText;

    private void OnEnable()
    {
        EconomyManager.OnCurrencyChanged += UpdateCurrency;
    }

    private void OnDisable()
    {
        EconomyManager.OnCurrencyChanged -= UpdateCurrency;
    }

    private void UpdateCurrency(int amount)
    {
        playerCurrencyText.text = amount.ToString();
    }
}
