using UnityEngine;

public class TowerData : ScriptableObject
{
    [Header("General")]
    public Sprite towerIcon;
    public Sprite towerCellIcon;
    public GameObject towerPrefab;
    public GameObject towerGhostPrefab;
    public string towerName;
    public string description;
    public int cost;
}
