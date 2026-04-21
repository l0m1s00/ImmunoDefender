using System.Collections.Generic;
using UnityEngine;

public class AttackTowerStats : TargetingTowerStats
{
    public ImmuneCellData soldierData; //soldado que envia la torre
    protected int baseMaxSoldiers; //cantidad de celulas maxima que puede tener la torre
    public int maxSoldiers;
    public float spawnSoldierCooldown; //tiempo de espera entre el envio de soldados
    public int availableSoldiers; //soldados disponibles para mandar
    public int totalSoldiers; //cantidad de soldados de la torre, desplegados y dentro de la torre

    public AttackTowerStats(AttackerTowerData data) : base(data)
    {
        soldierData = data.soldierDataSO;
        baseMaxSoldiers = data.nSoldiers;
        maxSoldiers = baseMaxSoldiers;
        spawnSoldierCooldown = data.spawnSoldierCooldown;
        totalSoldiers = baseMaxSoldiers;
        availableSoldiers = baseMaxSoldiers;
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat { 
            statName = "Available Soldiers", 
            currentStatValue = availableSoldiers.ToString() 
        });

        stats.Add(new EntitiyStat { 
            statName = "Deployed Soldiers", 
            currentStatValue = (totalSoldiers - availableSoldiers).ToString() 
        });

        return stats;
    }
}
