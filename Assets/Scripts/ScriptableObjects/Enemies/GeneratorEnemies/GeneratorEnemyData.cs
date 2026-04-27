using UnityEngine;

public abstract class GeneratorEnemyData : EnemyData
{
    [Header("Generate Toxic Waste Settings")]
    public float dropWasteMinCooldown;
    public float dropWasteMaxCooldown;
}
