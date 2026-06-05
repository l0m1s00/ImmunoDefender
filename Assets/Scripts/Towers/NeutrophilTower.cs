using System;
using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTower : TargetingTower, IStateInfoProvider
{
    private CellSpawnerModule spawnerModule;
    private float refillTimer;

    //Info provider
    private string currentProviderText;

    //EVENTOS
    public event Action<string> OnStateInfoChanged;

    protected NeutrophilTowerStats neutrophilTowerStats => (NeutrophilTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is NeutrophilTowerData neutrophilTowerData)
        {
            basicStats = new NeutrophilTowerStats(neutrophilTowerData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, neutrophilTowerStats.spawnerStats);
            spawnerModule.OnNewCellGenerated += NotifyStatsChanged;
            refillTimer = neutrophilTowerStats.refillCooldown;
            currentProviderText = neutrophilTowerData.allSoldiersGeneratedText;
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleSoldiersRefill();
        bool shouldSpawn = currentTarget != null;
        spawnerModule.HandleSpawning(shouldSpawn);
    }

    private void HandleSoldiersRefill()
    {
        if (neutrophilTowerStats.spawnerStats.totalSoldiers < neutrophilTowerStats.spawnerStats.maxSoldiers)
        {
            refillTimer -= Time.deltaTime;

            currentProviderText = neutrophilTowerStats.data.reloadSoldierText + " (" + Mathf.RoundToInt(refillTimer) + " s)";
            OnStateInfoChanged?.Invoke(currentProviderText);

            if (refillTimer <= 0f)
            {
                neutrophilTowerStats.spawnerStats.totalSoldiers++;
                neutrophilTowerStats.spawnerStats.availableSoldiers++;
                NotifyStatsChanged();
                refillTimer = neutrophilTowerStats.refillCooldown;
                AudioManager.Instance.PlaySFX(neutrophilTowerStats.data.reloadSoldierSounds, transform.position);
            }
        }
        else
        {
            if (refillTimer != neutrophilTowerStats.refillCooldown) refillTimer = neutrophilTowerStats.refillCooldown;
            if (currentProviderText != neutrophilTowerStats.data.allSoldiersGeneratedText)
            {
                currentProviderText = neutrophilTowerStats.data.allSoldiersGeneratedText;
                OnStateInfoChanged?.Invoke(currentProviderText);
            }
        }
    }

    public void OnNeutrophilSoldierDead()
    {
        neutrophilTowerStats.spawnerStats.totalSoldiers--;
        NotifyStatsChanged();
    }

    public override void Reset()
    {
        spawnerModule.OnNewCellGenerated -= NotifyStatsChanged;
        spawnerModule.ResetInMapTowerCells();

        base.Reset();

        refillTimer = 0f;
    }

    public string GetCurrentStateInfo()
    {
        return currentProviderText;
    }
}
