using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageCellStats : AttackerCellStats
{
    private float baseDeleteToxicWasteTime;
    public float deleteToxicWasteTime;
    private int baseExtraGeneticsPerClean;
    public int extraGeneticsPerClean;

    public MacrophageCellStats(MacrophageImmuneCellData data) : base(data)
    {
        baseDeleteToxicWasteTime = data.deleteToxicWasteCooldown;
        deleteToxicWasteTime = baseDeleteToxicWasteTime;
        baseExtraGeneticsPerClean = data.extraGeneticsPerClean;
        extraGeneticsPerClean = baseExtraGeneticsPerClean;
    }

    public override void ApplyUpgrade(UpgradeType upgradeType, float amount)
    {
        base.ApplyUpgrade(upgradeType, amount);

        switch (upgradeType)
        {
            case UpgradeType.CleanSpeed:
                deleteToxicWasteTime -= amount;
                break;
            case UpgradeType.MoreGeneticsPerWaste:
                extraGeneticsPerClean += (int)amount;
                break;
        }
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Clean Waste", 
            initialStatValue = baseDeleteToxicWasteTime.ToString("F2") + " s", 
            currentStatValue = deleteToxicWasteTime.ToString("F2") + " s",
            extraInformation = "The time it takes to eliminate a toxic waste"
        });

        //stats.Add(new EntitiyStat
        //{
        //    statName = "Extra Genetics Per Clean",
        //    initialStatValue = baseDeleteToxicWasteTime.ToString("F2") + " s",
        //    currentStatValue = deleteToxicWasteTime.ToString("F2") + " s",
        //    extraInformation = "The time it takes to eliminate a toxic waste"
        //});

        return stats;
    }
}
