using TMPro;
using UnityEngine;

public class TooltipPanelUI : BasePanelUI
{
    //se encarga de mostrar un panel de informacion flotante cerca del raton cuando este se posiciona sobre algun elemento de la interfaz
    //que debe mostrar informacion extra al usuario

    public static TooltipPanelUI Instance;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI tooltipText;
    [Header("Settings")]
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private bool opened = false;
    private Vector2 offset;
    private RectTransform rectTransform;
    private RectTransform canvasParentRectTransform;

    protected override void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        rectTransform = GetComponent<RectTransform>();
        canvasParentRectTransform = transform.parent.GetComponent<RectTransform>();

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
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

    protected override void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OpenTooltipPanel(string content)
    {
        tooltipText.text = content;
        opened = true;
        Open();
    }

    public void CloseTooltipPanel()
    {
        Close();
        opened = false;
        tooltipText.text = "";
    }
}
