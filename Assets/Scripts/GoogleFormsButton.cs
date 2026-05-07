using UnityEngine;
using UnityEngine.UI;

public class GoogleFormsButton : MonoBehaviour
{
    [Header("Forms Settings")]
    [SerializeField] private string googleFormsURL = "https://forms.gle/8Ys9SyiQJhcmrgNj6";
    [SerializeField] private Button openFormsButton;

    private void Awake()
    {
        openFormsButton.onClick.AddListener(OpenFormsLink);
    }

    public void OpenFormsLink()
    {
        if (!string.IsNullOrEmpty(googleFormsURL))
        {
            Application.OpenURL(googleFormsURL);
        }
    }
}
