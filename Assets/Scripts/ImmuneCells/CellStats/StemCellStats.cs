using System.Collections.Generic;
using UnityEngine;

public class StemCellStats : BasicCellStats
{
    private float baseGetInfectionCooldown;
    public float getInfectionCooldown;
    public bool infected;

    public StemCellStats(StemCellData data) : base(data)
    {
        baseGetInfectionCooldown = data.getInfectionCooldown;
        getInfectionCooldown = baseGetInfectionCooldown;
        infected = false; 
    }

    public List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat
        {
            statName = "Time before infection",
            initialStatValue = baseGetInfectionCooldown.ToString("F2") + " s",
            currentStatValue = getInfectionCooldown.ToString("F2") + " s", //cambiar por un temporizador en tiempo real que indique el tiempo restante hasta la infeccion de una celual
            extraInformation = "After the infection, it represents the countdown before the cell becomes useless"
        });

        return stats;
    }
}
