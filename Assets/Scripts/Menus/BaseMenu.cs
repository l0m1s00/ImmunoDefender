using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [Header("UI Ref")]
    [SerializeField] protected BaseMenuUI baseMenuUI_Ref;

    protected virtual void Start()
    {
        closeButton.onClick.AddListener(Close);
    }

    protected virtual void Close()
    {
        baseMenuUI_Ref.Close();
    }

    protected virtual void Open()
    {
        baseMenuUI_Ref.Open();
    }
}
