using TMPro;
using UnityEngine;

public class EconomyUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup currencyCanvasGroup;
    [SerializeField] private TMP_Text playerCurrencyText;

    private void OnEnable()
    {
        EconomyManager.OnCurrencyChanged += UpdateCurrency;
        LevelManager.OnStartGame += DisplayCurrency;
    }

    private void OnDisable()
    {
        EconomyManager.OnCurrencyChanged -= UpdateCurrency;
        LevelManager.OnStartGame -= DisplayCurrency;
    }

    private void Start()
    {
        HideCurrency();
    }

    private void HideCurrency()
    {
        currencyCanvasGroup.alpha = 0f;
        currencyCanvasGroup.blocksRaycasts = false;
    }

    private void DisplayCurrency()
    {
        currencyCanvasGroup.alpha = 1f;
        currencyCanvasGroup.blocksRaycasts = true;
    }

    private void UpdateCurrency(int amount)
    {
        playerCurrencyText.text = amount.ToString();
    }
}
