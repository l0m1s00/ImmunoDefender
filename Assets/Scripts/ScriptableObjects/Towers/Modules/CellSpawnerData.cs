using UnityEngine;
using System;

[Serializable]
public class CellSpawnerData
{
    //Modulo de datos que permite a una torre generar entidades
    [Header("Cell soldier settings")]
    public ImmuneCellData soldierDataSO;
    public int nSoldiers; //cantidad maxima de soldados que tiene la torre
    public float spawnSoldierCooldown; //tiempo de espera entre la invocacion de un soldado y el siguiente
}
