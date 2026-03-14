using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NegativeStatesUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image toxicBarFill;
    [SerializeField] private Color septicShockLowAmountColor;
    [SerializeField] private Color septicShockMediumAmountColor;
    [SerializeField] private Color septicShockMaxAmountColor;
    [SerializeField] private Color septicShockMaxAmountNeonColor;
    [Header("Game Feel Animation Settings")]
    [SerializeField] private float popUpScaleMultiplier;
    [SerializeField] private float popUpDuration;
    [SerializeField] private float toxicBarPulseSpeed;

    private enum ToxicityLevel { Low, Medium, Max };
    private ToxicityLevel currentToxicityLevel = ToxicityLevel.Low;

    private Vector3 originalToxicBarScale;
    private Coroutine activePopUpCoroutine;
    private Coroutine activeSepticShockCoroutine;

    private void Awake()
    {
        InitializeNegativeStatesUI();
    }

    private void InitializeNegativeStatesUI()
    {
        //toxicidad shock septico
        toxicBarFill.color = septicShockLowAmountColor;
        toxicBarFill.fillAmount = 0f;
        originalToxicBarScale = toxicBarFill.transform.localScale;
    }

    public void UpdateToxicBar(float normalizedToxicAmount)
    {
        toxicBarFill.fillAmount = normalizedToxicAmount;

        ToxicityLevel newToxicityLevel;
        if (toxicBarFill.fillAmount < 0.5f) newToxicityLevel = ToxicityLevel.Low;
        else if (toxicBarFill.fillAmount >= 0.5f && toxicBarFill.fillAmount < 1f) newToxicityLevel = ToxicityLevel.Medium;
        else newToxicityLevel = ToxicityLevel.Max;

        if (newToxicityLevel != currentToxicityLevel)
        {
            HandleToxicityLevelChange(newToxicityLevel);
        }
        else if (newToxicityLevel != ToxicityLevel.Max)
        {
            UpdateToxicBarColor(newToxicityLevel);
        }
    }

    private void HandleToxicityLevelChange(ToxicityLevel newToxicityLevel)
    {
        currentToxicityLevel = newToxicityLevel;

        if (activeSepticShockCoroutine != null)
        {
            StopCoroutine(activeSepticShockCoroutine);
            activeSepticShockCoroutine = null;
            toxicBarFill.transform.localScale = originalToxicBarScale;
        }

        UpdateToxicBarColor(newToxicityLevel);

        if (newToxicityLevel == ToxicityLevel.Max)
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
                toxicBarFill.color = septicShockLowAmountColor;
                break;
            case ToxicityLevel.Medium:
                toxicBarFill.color = septicShockMediumAmountColor;
                break;
            case ToxicityLevel.Max:
                toxicBarFill.color = septicShockMaxAmountColor;
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
            toxicBarFill.transform.localScale = Vector3.Lerp(originalToxicBarScale, targetScale, time / halfDuration);
            yield return null;
        }

        time = 0f;

        //decrecer
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            toxicBarFill.transform.localScale = Vector3.Lerp(targetScale, originalToxicBarScale, time / halfDuration);
            yield return null;
        }

        toxicBarFill.transform.localScale = originalToxicBarScale;
    }

    private IEnumerator SepticShockAnimationCoroutine()
    {
        Vector3 targetScale = originalToxicBarScale * popUpScaleMultiplier;
    
        while (true)
        {
            //obtener valor entre 0 y 1 de forma suave
            float pingPongValue = Mathf.PingPong(Time.time * toxicBarPulseSpeed, 1f);
            //interpolar el color
            toxicBarFill.color = Color.Lerp(septicShockMaxAmountColor, septicShockMaxAmountNeonColor, pingPongValue);
            //interpolar escala
            toxicBarFill.transform.localScale = Vector3.Lerp(originalToxicBarScale, targetScale, pingPongValue);
        
            yield return null;
        }
    }
}
