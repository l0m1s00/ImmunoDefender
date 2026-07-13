using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup restWaveTimerCanvasGroup;
    [SerializeField] private TextMeshProUGUI restTimerText;
    [SerializeField] private Image leftTimeFill;
    [SerializeField] private Image rightTimeFill;

    private void Start()
    {
        HideRestWaveTimer();
    }

    public void UpdateRestWaveTimerText(int newTime, float normalizedTime)
    {
        restTimerText.text = newTime.ToString();
        leftTimeFill.fillAmount = normalizedTime;
        rightTimeFill.fillAmount = normalizedTime;
    }

    public void HideRestWaveTimer()
    {
        restWaveTimerCanvasGroup.alpha = 0f;
        restWaveTimerCanvasGroup.blocksRaycasts = false;
        leftTimeFill.fillAmount = 1f;
        rightTimeFill.fillAmount = 1f;
    }

    public void DisplayRestWaveTimer(int newTime)
    {
        restTimerText.text = newTime.ToString();
        restWaveTimerCanvasGroup.alpha = 1f;
        restWaveTimerCanvasGroup.blocksRaycasts = true;
    }
}
