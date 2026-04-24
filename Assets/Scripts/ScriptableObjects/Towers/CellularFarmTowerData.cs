using UnityEngine;

[CreateAssetMenu(fileName = "CellularFarmTowerData", menuName = "Immuno Defender/Towers/Cellular Farm Tower Data")]
public class CellularFarmTowerData : TowerData
{
    [Header("Economic Settings")]
    [Header("La cantidad de recursos generada por segundo NO debe dar decimales, debe ser una cantidad entera")]
    public int generatedCurrencyAmountPerCell;
    public float generatedCurrencyInterval;
    public int nStemCells;
    [Header("'Cow' Cells Data")]
    public CellSpawnerData spawnerData;
}
