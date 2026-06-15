using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FormsMenu : BaseMenu
{
    [Header("Forms Settings")]
    [SerializeField] private string initialGoogleFormsURL = "https://forms.gle/8Ys9SyiQJhcmrgNj6";
    [SerializeField] private string finalGoogleFormsURL = "https://forms.gle/8Ys9SyiQJhcmrgNj6";
    [Header("Buttons")]
    [SerializeField] private Button ignoreFormsButton;
    [SerializeField] private Button openFormsButton;

    private FormsMenuUI formsUI_Ref;

    //EVENTOS
    public static event Action OnDisplayDifficulties;

    private void OnEnable()
    {
        MenusManager.OnPlay += Open;
        GameManager.OnEndGame += Open;
    }

    private void OnDisable()
    {
        MenusManager.OnPlay -= Open;
        GameManager.OnEndGame -= Open;
    }

    protected override void Start()
    {
        base.Start();
        ignoreFormsButton.onClick.AddListener(IgnoreGoogleForms);
        openFormsButton.onClick.AddListener(OpenGoogleForms);
        formsUI_Ref = (FormsMenuUI)baseMenuUI_Ref;
    }

    private void OpenGoogleForms()
    {
        if (!string.IsNullOrEmpty(initialGoogleFormsURL) && GameManager.Instance.GetCurrentGameState() == GameManager.GameState.MainMenu)
        {
            Application.OpenURL(initialGoogleFormsURL);
            OnDisplayDifficulties?.Invoke();
        }
        else if (!string.IsNullOrEmpty(initialGoogleFormsURL) && GameManager.Instance.GetCurrentGameState() == GameManager.GameState.GameOver)
        {
            Application.OpenURL(finalGoogleFormsURL);
            Close();
        }
    }

    private void IgnoreGoogleForms()
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.MainMenu) OnDisplayDifficulties?.Invoke();
        else if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.GameOver) Close();
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
