using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePanelUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    [Header("Components")]
    [SerializeField] private Button hidePanelButton;
    [SerializeField] private Button maximizePanelButton;
    [Header("Minimization Settings")]
    [SerializeField] protected Vector2 hiddenPosition; //posicion hacia donde se oculta el panel
    [SerializeField] protected float slideDuration; //tiempo de la animacion de ocultamiento
    [SerializeField] protected AnimationCurve slideCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); //curva de animacion para fluidez en la ocultacion

    private Vector2 originalPos;
    private bool isMinimized = false;
    private Coroutine slideCoroutine;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if (hidePanelButton != null) hidePanelButton.onClick.AddListener(HandleOcultation);
        if (maximizePanelButton != null) maximizePanelButton.onClick.AddListener(HandleOcultation);
    }

    protected virtual void Start()
    {
        //guardar posicion en el editor del panel 
        originalPos = rectTransform.anchoredPosition;
        Close();
    }

    protected virtual void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        //if (isMinimized)
        //{
        //    hidePanelButton.gameObject.SetActive(true);
        //    maximizePanelButton.gameObject.SetActive(false);
        //    Maximize(); //al abrir el panel siempre debe estar en su posicion normal
        //}
    }

    protected virtual void Close()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void HandleOcultation()
    {
        isMinimized = !isMinimized;
        Vector2 targetPos = isMinimized ? hiddenPosition : originalPos;

        if (!isMinimized)
        {
            hidePanelButton.gameObject.SetActive(true);
            maximizePanelButton.gameObject.SetActive(false);
        }
        else
        {
            hidePanelButton.gameObject.SetActive(false);
            maximizePanelButton.gameObject.SetActive(true);
        }


        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideToPosition(targetPos));
    }

    private IEnumerator SlideToPosition(Vector2 targetPos)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < slideDuration)
        {
            float curvePercent = slideCurve.Evaluate(timeElapsed / slideDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, curvePercent); //el inicio y el final de la animacion se producen suavemente

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }

    private void Maximize()
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        isMinimized = false;
        rectTransform.anchoredPosition = originalPos;
    }
}
