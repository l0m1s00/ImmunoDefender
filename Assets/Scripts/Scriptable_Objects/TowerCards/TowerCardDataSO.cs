using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerObject", menuName = "Immuno Defender/Tower Card Data")]
public class TowerCardDataSO : ScriptableObject
{
    public string towerName;
    public Sprite towerIcon;
    public Sprite towerCellIcon;
    public int cost;
    public string description;
    public GameObject towerPrefab;
    //public float range;
    //public float fireRate;
}
