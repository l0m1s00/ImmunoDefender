using TMPro;
using UnityEngine;

public class TooltipPanelUI : MonoBehaviour
{
    //se encarga de mostrar un panel de informacion flotante cerca del raton cuando este se posiciona sobre algun elemento de la interfaz
    //que debe mostrar informacion extra al usuario

    public static TooltipPanelUI Instance;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Settings")]
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private bool opened = false;
    private Vector2 offset;
    private RectTransform rectTransform;
    private RectTransform canvasParentRectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        rectTransform = GetComponent<RectTransform>();
        canvasParentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        CloseTooltipPanel();
        offset = new Vector2(offsetX, offsetY);
    }

    private void Update()
    {
        offset = new Vector2(offsetX, offsetY);

        if (opened)
        {
            //transformar coordenadas de raton en coordenadaas de pantalla
            Vector2 mousePos = Inputs.Instance.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasParentRectTransform,
                mousePos,
                null,
                out Vector2 localPoint
                );

            rectTransform.anchoredPosition = localPoint + offset;
        }
    }

    public void OpenTooltipPanel(string content)
    {
        tooltipText.text = content;
        opened = true;
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void CloseTooltipPanel()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        opened = false;
        tooltipText.text = "";
    }
}
