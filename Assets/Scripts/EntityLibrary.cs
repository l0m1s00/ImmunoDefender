using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityLibrary : MonoBehaviour
{
    public static EntityLibrary Instance;

    [Header("Enemy Types")]
    [SerializeField] private List<EnemyData> enemyTypes;
    [Header("Tower Types")]
    [SerializeField] private List<TowerData> towerTypes;
    [Header("Cell Types")]
    [SerializeField] private List<ImmuneCellData> cellTypes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    public List<EnemyData> GetEnemyTypes () { return enemyTypes; }
    public List<TowerData> GetTowerTypes() { return towerTypes; }
    public List<ImmuneCellData> GetCellTypes() { return cellTypes; }
}
