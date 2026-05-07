using UnityEngine;
using UnityEngine.UI;

public class DifficultiesMenuUI : BaseMenuUI
{
    private GameObject selectedDifficultyPanelObj;

    protected override void Awake()
    {
        base.Awake();
        selectedDifficultyPanelObj = this.gameObject;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Close()
    {
        base.Close();
        selectedDifficultyPanelObj.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        selectedDifficultyPanelObj.SetActive(true);
    }

    public void SetButtonColorAlpha(Image buttonImage, bool isEnable)
    {
        Color newAlphaColor = buttonImage.color;
        newAlphaColor.a = isEnable ? 1f : 0.25f;
        buttonImage.color = newAlphaColor;
    }
}
