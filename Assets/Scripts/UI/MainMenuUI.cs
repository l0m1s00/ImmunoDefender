using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject selectDifficultyPanelObj;
    private GameObject previousSelectedDifficultyOutline = null;

    private void Awake()
    {
        selectDifficultyPanelObj.SetActive(false);
    }

    public void ShowDifficultyPanel()
    {
        selectDifficultyPanelObj.SetActive(true);
    }

    public void HideSelectDifficultyButton(GameObject buttonObj)
    {
        buttonObj.SetActive(false);
    }

    public void ShowSelectedDifficultyOutline(GameObject parentObj)
    {
        GameObject currentSelectedDifficultyOutline = parentObj.transform.GetChild(0).gameObject;

        if (currentSelectedDifficultyOutline != previousSelectedDifficultyOutline)
        {
            if (previousSelectedDifficultyOutline != null) previousSelectedDifficultyOutline.SetActive(false);
            previousSelectedDifficultyOutline = currentSelectedDifficultyOutline;
            currentSelectedDifficultyOutline.SetActive(true);
        }
    }
}
