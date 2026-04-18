using UnityEngine;

public class AttackTowerStats : TargetingTowerStats
{
    public ImmuneCellData soldierData; //soldado que envi la torre
    public int maxSoldiers; //cantidad de celulas que puede enviar para combatir
    public float spawnSoldierCooldown; //tiempo de espera entre el envio de soldados
    public int availableSoldiers; //soldados disponibles para mandar
    public int totalSoldiers; //cantidad de soldados de la torre, desplegados y dentro de la torre

    public AttackTowerStats(AttackerTowerData data) : base(data)
    {
        soldierData = data.soldierDataSO;
        maxSoldiers = data.nSoldiers;
        spawnSoldierCooldown = data.spawnSoldierCooldown;
        totalSoldiers = maxSoldiers;
        availableSoldiers = maxSoldiers;
    }
}
