
using System.Collections.Generic;

public class EnemyStats
{
    public string enemyName;
    public float speed;
    public float damage;
    public float maxHealth;
    public float currentHealth;
    public int rewardDrop;
    public EnemyData data;

    public EnemyStats(EnemyData data)
    {
        this.data = data;
        enemyName = data.enemyName;
        speed = data.speed;
        damage = data.damage;
        maxHealth = data.health;
        currentHealth = maxHealth;
        rewardDrop = data.rewardDrop;
    }

    public virtual List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat
        {
            statName = "Health",
            currentStatValue = currentHealth.ToString("F2") + " / " + maxHealth.ToString("F2")
        });
        stats.Add(new EntitiyStat
        {
            statName = "Damage",
            currentStatValue = damage.ToString("F2")
        });
        stats.Add(new EntitiyStat
        {
            statName = "Speed",
            currentStatValue = speed.ToString("F2")
        });
        stats.Add(new EntitiyStat
        {
            statName = "Reward",
            currentStatValue = rewardDrop.ToString(),
            extraInformation = "The amount of Genetis it drops when defeated"
        });

        return stats;
    }
}
