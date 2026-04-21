using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BasicCellStats
{
    public string cellName;
    public float speed;
    protected float baseSpeed;
    public ImmuneCellData data;

    public BasicCellStats(ImmuneCellData data)
    {
        cellName = data.cellName;
        baseSpeed = data.speed;
        speed = baseSpeed;
        this.data = data;
    }

    public virtual List<EntitiyStat> GetStats(Dictionary<EnemyData, float> damageBonuses) //CAMBIR EXTRA DAMAGE PARA QUE SEA UNA ESTRUCTURA DE DATOS QUE ALMACENE TODOS LOS BUFFOS
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { 
            statName = "Speed", 
            initialStatValue = baseSpeed.ToString("F2"), 
            currentStatValue = speed.ToString("F2") 
        });

        return stats;
    }
}
