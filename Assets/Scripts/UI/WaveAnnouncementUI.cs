using System.Collections;
using TMPro;
using UnityEngine;

public class WaveAnnouncementUI : MonoBehaviour
{
    [Header("Wave Announcement")]
    [SerializeField] private TextMeshProUGUI waveAnnouncementText;
    [SerializeField] private CanvasGroup waveAnnouncementCanvasGroup;
    [SerializeField] private float waveAnnouncementFadeDuration;
    [SerializeField] private float waveAnnouncementWaitDuration;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float popDuration;

    private void Awake()
    {
        waveAnnouncementCanvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        EnemySpawner.OnStartWave += WaveAnnouncement;
    }

    private void OnDisable()
    {
        EnemySpawner.OnStartWave -= WaveAnnouncement;
    }

    private void WaveAnnouncement(string waveIndex)
    {
        waveAnnouncementText.text = "Wave - " + waveIndex;
        StartCoroutine(WaveAnnouncementAnimation());
    }

    private IEnumerator WaveAnnouncementAnimation()
    {
        waveAnnouncementCanvasGroup.alpha = 1f;
        float elapsedTime = 0f;

        //rebote
        while (elapsedTime < popDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentScale = scaleCurve.Evaluate(elapsedTime/popDuration);
            waveAnnouncementCanvasGroup.transform.localScale = Vector3.one * currentScale;

            yield return null;
        }

        //espera para la lectura
        waveAnnouncementCanvasGroup.transform.localScale = Vector3.one; //estabilizacion del rebote por escala
        yield return new WaitForSeconds(waveAnnouncementWaitDuration);

        //desvanecimiento
        elapsedTime = 0f;
        while (elapsedTime < waveAnnouncementFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            waveAnnouncementCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime/waveAnnouncementFadeDuration);
            waveAnnouncementCanvasGroup.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.2f, elapsedTime / waveAnnouncementFadeDuration);

            yield return null;
        }

        waveAnnouncementCanvasGroup.alpha = 0f; //completamente invisible al terminar
    }
}
