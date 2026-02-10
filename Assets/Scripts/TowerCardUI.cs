using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TowerCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cellImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    //[SerializeField] private Image selectedOutline;
    [SerializeField] private Button cardButton;

    [Header("Animations")]
    [SerializeField] private float onMouseScaleFactor;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float animationDuration;
    [SerializeField][Range(1f, 2f)] private float selectedScaleFactor;
    [SerializeField] private float selectedUpOffset;

    //animacion
    private Vector3 originalScale;
    private Vector3 originalPos;
    private RectTransform rectTransform;
    private Coroutine currentAnimationCoroutine;

    //generales
    private TowerCardDataSO towerData;
    private TowerSelectionManager manager;
    private int cardIndex;
    private bool isSelected = false;

    public void CardSetup(TowerCardDataSO data, int index, TowerSelectionManager manager)
    {
        //generales
        cardIndex = index;
        towerData = data;
        this.manager = manager;
        cardButton.onClick.AddListener(OnCardClicked);
        //UI
        iconImage.sprite = data.towerIcon;
        cellImage.sprite = data.towerCellIcon;
        costText.text = data.cost.ToString();
        cardNameText.text = data.name;
        cardDescriptionText.text = data.description;
        //selectedOutline.enabled = false;
        //animacion
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.localPosition;
        originalScale = rectTransform.localScale;
    }

    private void OnCardClicked()
    {
        manager.SelectTower(cardIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;

        Vector3 targetScale = originalScale * onMouseScaleFactor;
        Vector3 targetPos = originalPos + new Vector3(0, verticalOffset, 0);
        StartAnimation(targetScale, targetPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;

        StartAnimation(originalScale, originalPos);
    }

    private void StartAnimation(Vector3 targetScale, Vector3 targetPos)
    {
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(CardAnimation(targetScale, targetPos));
    }

    private IEnumerator CardAnimation(Vector3 targetScale, Vector3 targetPos)
    {
        float time = 0f;
        Vector3 startScale = rectTransform.localScale;
        Vector3 startPos = rectTransform.localPosition;

        while (time < animationDuration)
        {
            //porcentaje de animacion
            float per = time / animationDuration;
            //movimiento suave con aceleracion y frenado
            float smooth = Mathf.SmoothStep(0f, 1f, per);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, smooth);
            rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, smooth);

            time += Time.deltaTime;
            yield return null;
        }

        //al finalizar, valores exactos
        rectTransform.localScale = targetScale;
        rectTransform.localPosition = targetPos;
        currentAnimationCoroutine = null;
    }

    public void SetSelected(bool state)
    {
        isSelected = state;

        if (isSelected)
        {
            Vector3 targetScale = originalScale * selectedScaleFactor;
            Vector3 targetPos = originalPos + new Vector3(0f, selectedUpOffset, 0f);
            StartAnimation(targetScale, targetPos);
        }
        else
        {
            StartAnimation(originalScale, originalPos);
        }
    }

    //outline bool isSelected SetSelected(bool)

    public string GetCardName()
    {
        return towerData.name;
    }

    public int GetCardCost()
    {
        return towerData.cost;
    }
}


