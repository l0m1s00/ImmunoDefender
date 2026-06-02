using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NegativeStatesUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup toxicityBarCanvasGroup;
    [Header("UI References")]
    [SerializeField] private Image toxicBarValueFill;
    [SerializeField] private Color septicShockLowAmountColor;
    [SerializeField] private Color septicShockMediumAmountColor;
    [SerializeField] private Color septicShockMaxAmountColor;
    [SerializeField] private Color septicShockMaxAmountNeonColor;
    [Header("Game Feel Animation Settings")]
    [SerializeField] private float popUpScaleMultiplier;
    [SerializeField] private float popUpDuration;
    [SerializeField] private float toxicBarPulseSpeed;

    private Vector3 originalToxicBarScale;
    private Coroutine activePopUpCoroutine;
    private Coroutine activeSepticShockCoroutine;
    private ToxicityLevel currentToxicityLevel = ToxicityLevel.Low;

    private void Awake()
    {
        InitializeNegativeStatesUI();
    }

    private void OnEnable()
    {
        StartGameButtons.OnStartGame += DisplayToxicityBar;
    }

    private void OnDisable()
    {
        StartGameButtons.OnStartGame -= DisplayToxicityBar;
    }

    private void Start()
    {
        HideToxicityBar();
    }

    private void InitializeNegativeStatesUI()
    {
        //toxicidad shock septico
        toxicBarValueFill.color = septicShockLowAmountColor;
        toxicBarValueFill.fillAmount = 0f;
        originalToxicBarScale = toxicBarValueFill.transform.localScale;
    }

    private void HideToxicityBar()
    {
        toxicityBarCanvasGroup.alpha = 0f;
        toxicityBarCanvasGroup.blocksRaycasts = false;
    }

    private void DisplayToxicityBar()
    {
        toxicityBarCanvasGroup.alpha = 1f;
        toxicityBarCanvasGroup.blocksRaycasts = true;
    } 

    public void UpdateToxicBar(float normalizedToxicAmount, ToxicityLevel toxicityLevel)
    {
        toxicBarValueFill.fillAmount = normalizedToxicAmount;
        ToxicityLevel newToxicityLevel = toxicityLevel;

        if (newToxicityLevel != currentToxicityLevel)
        {
            currentToxicityLevel = newToxicityLevel;
            HandleToxicityLevelChange();
        }
        //else if (newToxicityLevel != ToxicityLevel.Max)
        //{
        //    UpdateToxicBarColor(newToxicityLevel);
        //}
    }

    private void HandleToxicityLevelChange()
    {
        if (activeSepticShockCoroutine != null)
        {
            StopCoroutine(activeSepticShockCoroutine);
            activeSepticShockCoroutine = null;
            toxicBarValueFill.transform.localScale = originalToxicBarScale;
        }

        UpdateToxicBarColor(currentToxicityLevel);

        if (currentToxicityLevel == ToxicityLevel.Max)
        {
            activeSepticShockCoroutine = StartCoroutine(SepticShockAnimationCoroutine());
        }
        else
        {
            if (activePopUpCoroutine != null) StopCoroutine(activePopUpCoroutine);
            activePopUpCoroutine = StartCoroutine(PopUpAnimationCoroutine());
        }
    }

    private void UpdateToxicBarColor(ToxicityLevel toxicityLevel)
    {
        switch (toxicityLevel)
        {
            case ToxicityLevel.Low:
                toxicBarValueFill.color = septicShockLowAmountColor;
                break;
            case ToxicityLevel.Medium:
                toxicBarValueFill.color = septicShockMediumAmountColor;
                break;
            case ToxicityLevel.Max:
                toxicBarValueFill.color = septicShockMaxAmountColor;
                break;
        }
    }

    private IEnumerator PopUpAnimationCoroutine()
    {
        Vector3 targetScale = originalToxicBarScale * popUpScaleMultiplier;
        float halfDuration = popUpDuration / 2;
        float time = 0f;

        //crecer
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            toxicBarValueFill.transform.localScale = Vector3.Lerp(originalToxicBarScale, targetScale, time / halfDuration);
            yield return null;
        }

        time = 0f;

        //decrecer
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            toxicBarValueFill.transform.localScale = Vector3.Lerp(targetScale, originalToxicBarScale, time / halfDuration);
            yield return null;
        }

        toxicBarValueFill.transform.localScale = originalToxicBarScale;
    }

    private IEnumerator SepticShockAnimationCoroutine()
    {
        Vector3 targetScale = originalToxicBarScale * popUpScaleMultiplier;
    
        while (true)
        {
            //obtener valor entre 0 y 1 de forma suave
            float pingPongValue = Mathf.PingPong(Time.time * toxicBarPulseSpeed, 1f);
            //interpolar el color
            toxicBarValueFill.color = Color.Lerp(septicShockMaxAmountColor, septicShockMaxAmountNeonColor, pingPongValue);
            //interpolar escala
            toxicBarValueFill.transform.localScale = Vector3.Lerp(originalToxicBarScale, targetScale, pingPongValue);
        
            yield return null;
        }
    }
}
