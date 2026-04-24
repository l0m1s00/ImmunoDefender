using System.Collections.Generic;
using UnityEngine;

public class StemCellStats : BasicCellStats
{
    private float baseGetInfectionCooldown;
    public float getInfectionCooldown;
    public bool infected;
    private int baseResistanceToInfection;
    public int resistanceToInfection;

    public StemCellStats(StemCellData data) : base(data)
    {
        baseGetInfectionCooldown = data.getInfectionCooldown;
        getInfectionCooldown = baseGetInfectionCooldown;
        baseResistanceToInfection = data.resistanceToInfection;
        resistanceToInfection = baseResistanceToInfection;
        infected = false; 
    }

    public List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat
        {
            statName = "Time before infection",
            initialStatValue = baseGetInfectionCooldown.ToString("F2") + " s",
            currentStatValue = getInfectionCooldown.ToString("F2") + " s", //cambiar por un temporizador en tiempo real que indique el tiempo restante hasta la infeccion de una celual
            extraInformation = "After the infection, it represents the countdown before the cell becomes useless"
        });

        stats.Add(new EntitiyStat
        {
            statName = "Resistance to infection",
            initialStatValue = baseResistanceToInfection.ToString(),
            currentStatValue = resistanceToInfection.ToString(),
            extraInformation = ""
        });

        return stats;
    }
}
