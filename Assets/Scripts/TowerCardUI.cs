using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCardUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    //[SerializeField] private Image selectedOutline;
    [SerializeField] private Button cardButton;

    private TowerDataSO towerData;
    private TowerSelectionManager manager;
    private int cardIndex;

    public void CardSetup(TowerDataSO data, int index, TowerSelectionManager manager)
    {
        cardIndex = index;
        towerData = data;
        iconImage.sprite = data.towerIcon;
        costText.text = data.cost.ToString();
        cardNameText.text = data.name;
        this.manager = manager;
        //selectedOutline.enabled = false;

        cardButton.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        manager.SelectTower(cardIndex);
    }

    //outline bool isSelected SetSelected(bool)

    public string GetCardName()
    {
        return towerData.name;
    }

    public int GetCardCost()
    {
        return towerData.cost;
    }
}


