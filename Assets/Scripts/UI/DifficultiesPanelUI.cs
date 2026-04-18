using UnityEngine;
using UnityEngine.UI;

public class DifficultiesPanelUI : BaseMenuPanelUI
{
    private GameObject selectedDifficultyPanelObj;

    protected override void Awake()
    {
        base.Awake();
        selectedDifficultyPanelObj = this.gameObject;
    }

    protected override void Close()
    {
        base.Close();
        selectedDifficultyPanelObj.SetActive(false);
    }

    protected override void Open()
    {
        base.Open();
        selectedDifficultyPanelObj.SetActive(true);
    }

    public void DisplayDifficultyPanel(bool display)
    {
        if (display) Open();
        else Close();
    }

    public void SetButtonColorAlpha(Image buttonImage, bool isEnable)
    {
        Color newAlphaColor = buttonImage.color;
        newAlphaColor.a = isEnable ? 1f : 0.25f;
        buttonImage.color = newAlphaColor;
    }

    public void DisplaySelectDifficultyButton(GameObject buttonObj)
    {
        buttonObj.SetActive(true);
    }

    public void HideSelectDifficultyButton(GameObject buttonObj)
    {
        buttonObj.SetActive(false);
    }
}
