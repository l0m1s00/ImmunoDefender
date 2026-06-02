using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI upgradeNameText;
    //public Image upgradeIcon;
    public TextMeshProUGUI costText;
    public Button buyButton;
    public Image infoImage;

    [Header("Upgrade Indicators Per Level")]
    public Transform levelSquaresContainer; //horizontalLayoutGroup
    public GameObject levelSquarePrefab;
    public Color purchasedColor = Color.green;
    public Color unpurchasedColor = Color.white;

    private TowerUpgradeData currentData;

    public void InitializeUpgradeSlot(TowerUpgradeData data, int currentLevel)
    {
        currentData = data;

        upgradeNameText.text = data.upgradeName;

        int maxLevel = data.GetMaxLevel();
        bool isMaxed = currentLevel >= maxLevel;

        buyButton.onClick.RemoveAllListeners();

        //establecer texto de coste de la mejora
        if (isMaxed)
        {
            costText.text = "MAX";
            buyButton.interactable = false;
        }
        else
        {
            costText.text = data.costPerLevel[currentLevel].ToString();
            buyButton.interactable = true;

            buyButton.onClick.AddListener(() => TowerUpgradeManager.Instance.TryBuyUpgrade(data, this));
        }

        //configuracion del tooltip para mostrar las descripciones
        //si el boton no tiene la caracteristica para mostrar el tooltip se pone
        TooltipTrigger tooltip = buyButton.GetComponent<TooltipTrigger>();
        if (tooltip == null)
        {
            tooltip = infoImage.gameObject.AddComponent<TooltipTrigger>();
        }
        tooltip.content = data.description;

        //eliminar referencias anteriores
        foreach (Transform levelUpgradeSquare in levelSquaresContainer)
        {
            Destroy(levelUpgradeSquare.gameObject);
        }

        //establecer el color de los cuadrados que indican el nivel de la mejora
        for (int i = 0; i < currentData.GetMaxLevel(); i++)
        {
            GameObject square = Instantiate(levelSquarePrefab, levelSquaresContainer);
            Image squareImage = square.GetComponent<Image>();

            if (i < currentLevel)
            {
                squareImage.color = purchasedColor;
            }
            else
            {
                squareImage.color = unpurchasedColor;
            }
        }
    }
}
