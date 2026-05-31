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
    [Header("Infection Settings")]
    [Range(1, 100)] public int resistanceToInfection;
    //existe una probabilidad minima de infeccion de granja, independientemente de la defensa infecciosa y el ataque infeccioso de los virus
    [Range(0, 100)] public int minGetInfectionProbability;
    public float farmInfectionCooldown;
    public GameObject infectionSpawnerObj; //CAMBIAR POR DATA AL IGUAL QUE EN EL RESTO DE ENTIDADES
    [Header("Provider State Texts")]
    public string noInfection;
    public string possibleInfection;
}
