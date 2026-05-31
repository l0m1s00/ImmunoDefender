using UnityEngine;

public class NK_Tower : TargetingTower
{
    private CellSpawnerModule spawnerModule;
    protected NK_TowerStats NK_Stats => (NK_TowerStats)basicStats;


    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is NK_TowerData NK_TowerData)
        {
            basicStats = new NK_TowerStats(NK_TowerData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, NK_Stats.spawnerStats);
        }
    }

    protected override void Update()
    {
        base.Update();
        spawnerModule.HandleSpawning(true);
    }

    public override void Reset()
    {
        spawnerModule.ResetInMapTowerCells();

        base.Reset();
    }
}
