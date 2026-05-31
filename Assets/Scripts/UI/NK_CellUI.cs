using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NK_CellUI : ImmuneCellUI
{
    [Header("Analyze Farm Components")]

    [SerializeField] private Image analysisFarmCircleFill;
    [SerializeField] private GameObject analysisFarmCircleObject;
    [SerializeField] private Image analysisResultIcon;
    [SerializeField] private float displayAnalysisResultTextBoxTime;

    protected override void Start()
    {
        base.Start();

        HideAnalysisBar();
        HideAnalysisResultIcon();
    }

    public void UpdateAnalysisBar(float amount)
    {
        if (!analysisFarmCircleObject.activeSelf) analysisFarmCircleObject.SetActive(true);

        analysisFarmCircleFill.fillAmount = amount;
    }

    public void HideAnalysisBar()
    {
        analysisFarmCircleFill.fillAmount = 0f;
        analysisFarmCircleObject.SetActive(false);
    }

    public void UpdateAnalysisResultIcon(Sprite analysisIcon)
    {
        analysisResultIcon.sprite = analysisIcon;
        StartCoroutine(ShowAnalysisResultTextBox());
    }

    private IEnumerator ShowAnalysisResultTextBox()
    {
        DisplayAnalysisResultIcon();
        yield return new WaitForSeconds(displayAnalysisResultTextBoxTime);
        HideAnalysisResultIcon();
        yield return null;
    }

    private void DisplayAnalysisResultIcon()
    {
        analysisResultIcon.enabled = true;
    }

    public void HideAnalysisResultIcon()
    {
        analysisResultIcon.enabled = false;
    }
}
