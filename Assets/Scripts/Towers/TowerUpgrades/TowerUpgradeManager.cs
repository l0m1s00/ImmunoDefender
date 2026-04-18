using UnityEngine;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    [SerializeField] private UpgradeMenuUI upgradeMenuUI_Ref;
    private TowerData currentSelectedTowerDataSO;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitializeUpgradeMenu(BasicTowerStats data)
    {
        currentSelectedTowerDataSO = data.baseData;
        upgradeMenuUI_Ref.InitializePanelElements(currentSelectedTowerDataSO);
    }

    public void ResetUpgradeMenu()
    {
        currentSelectedTowerDataSO = null;
        upgradeMenuUI_Ref.CloseUpgradePanel();
    }
}
