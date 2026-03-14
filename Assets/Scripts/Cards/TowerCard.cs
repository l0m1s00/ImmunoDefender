using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Components")]
    [SerializeField] private Button cardButton;
    [SerializeField] private TowerCardUI cardUIRef;

    private TowerData data;
    private int index;
    private bool isSelected = false;

    public void InitializeCard(TowerData data, int index)
    {
        this.data = data;
        this.index = index;
        cardUIRef.SetupVisuals(data);
        cardButton.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        TowerCardsManager.Instance.SelectTower(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) cardUIRef.AnimateHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) cardUIRef.AnimateHover(false);
    }

    public void SetSelected(bool state)
    {
        isSelected = state;

        if (isSelected) cardUIRef.AnimateSelected();
        else cardUIRef.AnimateDeselected();
    }

    public TowerData GetTowerCardData()
    {
        return data;
    }
}
