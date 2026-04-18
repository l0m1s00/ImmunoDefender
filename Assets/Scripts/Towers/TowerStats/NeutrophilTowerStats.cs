using UnityEngine;

public class NeutrophilTowerStats : AttackTowerStats
{
    public float refillCooldown;

    public NeutrophilTowerStats(NeutrophilTowerData data) : base(data)
    {
        refillCooldown = data.refillCooldown;
    }
}
