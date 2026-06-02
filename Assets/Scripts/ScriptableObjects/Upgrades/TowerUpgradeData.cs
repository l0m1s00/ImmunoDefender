using UnityEngine;

public enum UpgradeTarget //aunque las mejoras se compren desde la torre estas se aplican a la propia torre o a sus soldados
{
    Tower, //mejoras aplicadas y gestionadas por y sobre la torre
    Cell, //mejoras aplicadas sobre las celulas pero gestionadas por el spawner module
    SpawnerModule, //mejoras aplicadas y gestionadas por y sobre el spawner module
}

public enum UpgradeType
{
    //DE LA TORRE
    Range,
    MaxSoldiers,
    Refill,
    AnalysisNumber,
    AnalysisSpeed,

    //DE LOS SOLDADOS
    Damage,
    Speed,
    //AttackCooldown,
}

[CreateAssetMenu(fileName = "NewTowerUpgrade", menuName = "Immuno Defender/Towers/Upgrades/Tower Upgrade Data")]
public class TowerUpgradeData : ScriptableObject
{
    [Header("Visual Components")]
    public string upgradeName;
    //descripcion para tooltip
    //public Sprite icon;

    [Header("Settings")]
    public UpgradeTarget upgradeTarget;
    public UpgradeType upgradeType;
    public string description;
    [Header("ESTAS 2 LISTAS DEDBEN COINCIDIR EN LONGITUD")]
    public int[] costPerLevel; //coste de la mejora por nivel
    public float[] statIncrements; //cada nivel proporciona una cantidad de beneficios

    public int GetMaxLevel() { return Mathf.Min(costPerLevel.Length, statIncrements.Length); }
}
