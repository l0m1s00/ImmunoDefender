using System.Collections.Generic;
using UnityEngine;

public abstract class TowerData : ScriptableObject
{
    [Header("General")]
    public Sprite towerIcon;
    public Sprite towerCellIcon;
    public Sprite towerTypeIcon;
    public GameObject towerPrefab;
    public GameObject towerGhostPrefab;
    public string towerName;
    public string description;
    public int cost;
    public AudioEventData buildTowerSounds;

    [Header("Upgrades")]
    public List<TowerUpgradeData> towerUpgrades;
}
