using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardUI : AnimationsUI
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cellImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private Canvas cardLocalCanvas;
    [SerializeField] private Image selectedOutline;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalOrderInLayer = cardLocalCanvas.sortingOrder;
    }

    public void SetupVisuals(TowerData data)
    {
        iconImage.sprite = data.towerIcon;
        cellImage.sprite = data.towerCellIcon;
        costText.text = data.cost.ToString();
        cardNameText.text = data.towerName;
        cardDescriptionText.text = data.description;
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
        originalRot = transform.localRotation;
        informationPanel.SetActive(false);
        selectedOutline.enabled = false;
    }

    public void DoAnimateHover(bool isHovering)
    {
        informationPanel.SetActive(isHovering);
        AnimateHover(isHovering);
    }

    public void DoAnimateSelected()
    {
        selectedOutline.enabled = true;
        cardLocalCanvas.sortingOrder = selectedOrderInLayer;
        informationPanel.SetActive(false);
        AnimateSelected(false);
    }

    public void DoAnimateDeselected()
    {
        selectedOutline.enabled = false;
        cardLocalCanvas.sortingOrder = originalOrderInLayer;
        AnimateDeselected();
    }
}


