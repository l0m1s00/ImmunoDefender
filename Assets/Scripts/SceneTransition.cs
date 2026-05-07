using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("UI References")]
    [SerializeField] private CanvasGroup whiteFadeCanvasGroup;
    [SerializeField] private CanvasGroup blackFadeCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float whiteFadeDuration = 0.5f;
    [SerializeField] private float blackFadeDuration = 1f;

    private bool isTransitioning = false;

    //EVENTOS
    public static event Action OnTransitionCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnSceneTransition += StartFadeTransition;
    }

    private void OnDisable()
    {
        GameManager.OnSceneTransition -= StartFadeTransition;
    }

    private void StartFadeTransition()
    {
        if (isTransitioning) return;

        StartCoroutine(ToWhiteFadeTransition());
    }

    private IEnumerator ToWhiteFadeTransition()
    {
        isTransitioning = true;

        whiteFadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;

        while (timer < whiteFadeDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / whiteFadeDuration;

            //modificar alpha en base al tiempo de transicion
            whiteFadeCanvasGroup.alpha = normalizedTime;

            yield return null;
        }

        whiteFadeCanvasGroup.alpha = 1f;

        StartCoroutine(ToBlackFadeTransition());
    }

    private IEnumerator ToBlackFadeTransition()
    {
        blackFadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;

        while (timer < blackFadeDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / blackFadeDuration;

            //modificar alpha en base al tiempo de transicion
            blackFadeCanvasGroup.alpha = normalizedTime;

            yield return null;
        }

        blackFadeCanvasGroup.alpha = 1f;
        OnTransitionCompleted?.Invoke();
        isTransitioning = false;
    }
}
