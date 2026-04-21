using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BasicTowerStats
{
    public TowerData baseData; //todas las torres tienen esta informacion

    public BasicTowerStats(TowerData data)
    {
        baseData = data;
    }

    public virtual List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();
        return stats;
    }
}
