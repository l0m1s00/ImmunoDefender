using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardUI : AnimationsUI
{
    [Header("UI References")]
    [SerializeField] private Image towerIconImage;
    [SerializeField] private Image towerTypeIconImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private CanvasGroup informationPanelCanvasGroup;
    [SerializeField] private Canvas cardLocalCanvas;
    [SerializeField] private Image selectedOutline;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalOrderInLayer = cardLocalCanvas.sortingOrder;
    }

    public void SetupVisuals(TowerData data)
    {
        towerIconImage.sprite = data.towerIcon;
        towerTypeIconImage.sprite = data.towerTypeIcon;
        costText.text = data.cost.ToString();
        cardNameText.text = data.towerName;
        cardDescriptionText.text = data.description;
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
        originalRot = transform.localRotation;
        informationPanelCanvasGroup.alpha = 0f;
        informationPanelCanvasGroup.blocksRaycasts = false;
        selectedOutline.enabled = false;
    }

    public void DoAnimateHover(bool isHovering)
    {
        if (isHovering)
        {
            informationPanelCanvasGroup.alpha = 1f;
            informationPanelCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            informationPanelCanvasGroup.alpha = 0f;
            informationPanelCanvasGroup.blocksRaycasts = false;
        }

        AnimateHover(isHovering);
    }

    public void DoAnimateSelected()
    {
        selectedOutline.enabled = true;
        cardLocalCanvas.sortingOrder = selectedOrderInLayer;
        informationPanelCanvasGroup.alpha = 0f;
        informationPanelCanvasGroup.blocksRaycasts = false;
        AnimateSelected(false);
    }

    public void DoAnimateDeselected()
    {
        selectedOutline.enabled = false;
        cardLocalCanvas.sortingOrder = originalOrderInLayer;
        AnimateDeselected();
    }
}


