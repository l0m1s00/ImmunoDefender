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
    [SerializeField] private Canvas cardLocalCanvas;
    //[SerializeField] private Image selectedOutline;
    [SerializeField] private Button cardButton;

    [Header("Animation")]
    [SerializeField] private float onMouseScaleFactor;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float animationDuration;
    [SerializeField][Range(1f, 2f)] private float selectedScaleFactor;
    [SerializeField] private float selectedUpOffset;

    [Header("Render")]
    [SerializeField] private int selectedOrderInLayer;

    //animacion
    private Vector3 originalScale;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private RectTransform rectTransform;
    private Coroutine currentAnimationCoroutine;
    //render
    private int originalOrderInLayer;

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
        originalRot = rectTransform.localRotation;
        originalOrderInLayer = cardLocalCanvas.sortingOrder;
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
        StartAnimation(targetScale, targetPos, originalRot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;

        StartAnimation(originalScale, originalPos, originalRot);
    }

    private void StartAnimation(Vector3 targetScale, Vector3 targetPos, Quaternion targetRot)
    {
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(CardAnimation(targetScale, targetPos, targetRot));
    }

    private IEnumerator CardAnimation(Vector3 targetScale, Vector3 targetPos, Quaternion targetRot)
    {
        float time = 0f;
        Vector3 startScale = rectTransform.localScale;
        Vector3 startPos = rectTransform.localPosition;
        Quaternion startRot = rectTransform.localRotation;

        while (time < animationDuration)
        {
            //porcentaje de animacion
            float per = time / animationDuration;
            //movimiento suave con aceleracion y frenado
            float smooth = Mathf.SmoothStep(0f, 1f, per);

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, smooth);
            rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, smooth);
            rectTransform.localRotation = Quaternion.Lerp(startRot, targetRot, smooth);

            time += Time.deltaTime;
            yield return null;
        }

        //al finalizar, valores exactos
        rectTransform.localScale = targetScale;
        rectTransform.localPosition = targetPos;
        rectTransform.localRotation = targetRot;
        currentAnimationCoroutine = null;
    }

    public void SetSelected(bool state)
    {
        isSelected = state;

        if (isSelected)
        {
            cardLocalCanvas.sortingOrder = selectedOrderInLayer;
            Vector3 targetScale = originalScale * selectedScaleFactor;
            Vector3 targetPos = originalPos + new Vector3(0f, selectedUpOffset, 0f);
            Quaternion targetRot = Quaternion.identity; //0,0,0
            StartAnimation(targetScale, targetPos, targetRot);
        }
        else
        {
            cardLocalCanvas.sortingOrder = originalOrderInLayer;
            StartAnimation(originalScale, originalPos, originalRot);
        }
    }

    //outline bool isSelected SetSelected(bool)
}


