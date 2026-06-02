using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class LevelManagerUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup restWaveTimerCanvasGroup;
    [SerializeField] private TextMeshProUGUI restTimerText;

    private void Start()
    {
        HideRestWaveTimer();
    }

    public void UpdateRestWaveTimerText(int newTime)
    {
        restTimerText.text = newTime.ToString();
    }

    public void HideRestWaveTimer()
    {
        restWaveTimerCanvasGroup.alpha = 0f;
        restWaveTimerCanvasGroup.blocksRaycasts = false;
    }

    public void DisplayRestWaveTimer(int newTime)
    {
        restTimerText.text = newTime.ToString();
        restWaveTimerCanvasGroup.alpha = 1f;
        restWaveTimerCanvasGroup.blocksRaycasts = true;
    }
}
