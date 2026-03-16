using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerCardUI : MonoBehaviour
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

    [Header("Animation Settings")]
    [SerializeField][Range(1f, 2f)] private float onMouseScaleFactor;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float animationDuration;
    [SerializeField][Range(1f, 2f)] private float selectedScaleFactor;
    [SerializeField] private float selectedUpOffset;
    [SerializeField] private int selectedOrderInLayer;

    //animacion
    private Vector3 originalScale;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private RectTransform rectTransform;
    private Coroutine currentAnimationCoroutine;
    private int originalOrderInLayer;

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
        //selectedOutline.enabled = false;
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
        originalRot = transform.localRotation;
        informationPanel.SetActive(false);
        selectedOutline.enabled = false;
    }

    public void AnimateHover(bool isHovering)
    {
        Vector3 targetScale = isHovering ? originalScale * onMouseScaleFactor : originalScale;
        Vector3 targetPos = isHovering ? originalPos + new Vector3(0f, verticalOffset, 0f) : originalPos;
        informationPanel.SetActive(isHovering);
        StartAnimation(targetScale, targetPos, originalRot);
    }

    public void AnimateSelected()
    {
        informationPanel.SetActive(false);
        selectedOutline.enabled = true;

        cardLocalCanvas.sortingOrder = selectedOrderInLayer;
        Vector3 targetScale = originalScale * selectedScaleFactor;
        Vector3 targetPos = originalPos + new Vector3(0f, selectedUpOffset, 0f);
        StartAnimation(targetScale, targetPos, Quaternion.identity);
    }

    public void AnimateDeselected()
    {
        selectedOutline.enabled = false;

        cardLocalCanvas.sortingOrder = originalOrderInLayer;
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
}


