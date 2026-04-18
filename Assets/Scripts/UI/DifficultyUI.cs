using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyUI : AnimationsUI
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI difficultyNameText;
    [SerializeField] private TextMeshProUGUI difficultyDescriptionText;
    [SerializeField] private TextMeshProUGUI difficultyFeaturesText;
    [SerializeField] private Canvas difficultyLocalCanvas;
    [SerializeField] private GameObject selectedOutlineObj;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalOrderInLayer = difficultyLocalCanvas.sortingOrder;
    }

    public void SetupVisuals(DifficultySettingsData data)
    {
        difficultyNameText.text = data.title;
        difficultyDescriptionText.text = data.description;
        difficultyFeaturesText.text = data.features;
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
        originalRot = transform.localRotation;
        selectedOutlineObj.SetActive(false);
    }

    public void DoAnimateHover(bool isHovering)
    {
        AnimateHover(isHovering);
    }

    public void DoAnimateSelected()
    {
        selectedOutlineObj.SetActive(true);
        difficultyLocalCanvas.sortingOrder = selectedOrderInLayer;
        AnimateSelected(true);
    }

    public void DoAnimateDeselected()
    {
        selectedOutlineObj.SetActive(false);
        difficultyLocalCanvas.sortingOrder = originalOrderInLayer;
        AnimateDeselected();
    }

    public void Reset()
    {
        selectedOutlineObj.SetActive(false);
        rectTransform.localScale = originalScale;
        rectTransform.localPosition = originalPos;
        rectTransform.localRotation = originalRot;
        currentAnimationCoroutine = null;
        difficultyLocalCanvas.sortingOrder = originalOrderInLayer;
    }

    //private void Awake()
    //{
    //    selectDifficultyPanelObj.SetActive(false);
    //}

    //public void ShowSelectedDifficultyOutline(GameObject parentObj)
    //{
    //    GameObject currentSelectedDifficultyOutline = parentObj.transform.GetChild(0).gameObject;

    //    if (currentSelectedDifficultyOutline != previousSelectedDifficultyOutline)
    //    {
    //        if (previousSelectedDifficultyOutline != null) previousSelectedDifficultyOutline.SetActive(false);
    //        previousSelectedDifficultyOutline = currentSelectedDifficultyOutline;
    //        currentSelectedDifficultyOutline.SetActive(true);
    //    }
    //}
}
