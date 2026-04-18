using UnityEngine;

public abstract class BaseMenuPanelUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Close();
    }

    protected virtual void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    protected virtual void Close()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    //public virtual void Toggle()
    //{
    //    if (canvasGroup.alpha > 0f) Close();
    //    else Open();
    //}
}
