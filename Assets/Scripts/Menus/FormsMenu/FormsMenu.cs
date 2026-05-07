using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FormsMenu : BaseMenu
{
    [Header("Forms Settings")]
    [SerializeField] private string googleFormsURL = "https://forms.gle/8Ys9SyiQJhcmrgNj6";
    [Header("Buttons")]
    [SerializeField] private Button selectDifficultyButton;
    [SerializeField] private Button openFormsButton;

    private FormsMenuUI formsUI_Ref;

    //EVENTOS
    public static event Action OnSelectDifficulty;

    private void OnEnable()
    {
        MenusManager.OnPlay += Open;
    }

    private void OnDisable()
    {
        MenusManager.OnPlay -= Open;
    }

    protected override void Start()
    {
        base.Start();
        selectDifficultyButton.onClick.AddListener(SelectDifficulty);
        openFormsButton.onClick.AddListener(OpenGoogleForms);
        formsUI_Ref = (FormsMenuUI)baseMenuUI_Ref;
    }

    private void OpenGoogleForms()
    {
        if (!string.IsNullOrEmpty(googleFormsURL))
        {
            Application.OpenURL(googleFormsURL);
        }

        OnSelectDifficulty?.Invoke();
    }

    private void SelectDifficulty()
    {
        OnSelectDifficulty?.Invoke();
    }

    protected override void Open()
    {
        base.Open();
    }

    protected override void Close()
    {
        base.Close();
    }
}
