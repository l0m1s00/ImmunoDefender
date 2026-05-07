using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Difficulty Components")]
    [SerializeField] private Button difficultyButton;
    [SerializeField] private DifficultyUI difficultyUIRef;

    private DifficultySettingsData data;
    private int index;
    private bool isSelected = false;

    public void Initialize(DifficultySettingsData data, int index)
    {
        this.data = data;
        this.index = index;
        difficultyUIRef.SetupVisuals(data);
        difficultyButton.onClick.AddListener(OnDifficultyPanelClicked);
    }

    private void OnEnable()
    {
        //MainMenuManager.OnCloseMenu += Reset;
    }

    private void OnDisable()
    {
        //MainMenuManager.OnCloseMenu -= Reset;
    }

    private void OnDifficultyPanelClicked()
    {
        DifficultiesMenu.Instance.SelectDifficulty(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) difficultyUIRef.DoAnimateHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) difficultyUIRef.DoAnimateHover(false);
    }

    public void SetSelected(bool state)
    {
        isSelected = state;

        if (isSelected) difficultyUIRef.DoAnimateSelected();
        else difficultyUIRef.DoAnimateDeselected();
    }

    public DifficultySettingsData GetDifficultyData()
    {
        return data;
    }

    private void Reset()
    {
        if (isSelected) difficultyUIRef.Reset();

        isSelected = false;
    }
}
