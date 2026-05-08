using System;
using System.Collections.Generic;
using UnityEngine;

public class VitalOrgan : MonoBehaviour, IResettable
{
    [Header("Components")]
    [SerializeField] private VitalOrganUI vitalOrganUIRef;

    //STATS
    protected VitalOrganStats basicStats;

    //EVENTOS
    public static event Action OnVitalOrganDamaged;

    public virtual void Initialize(VitalOrganData vitalOrganData)
    {
        //stats
        basicStats = new VitalOrganStats(vitalOrganData);
        vitalOrganUIRef.InitializeVisuals(basicStats);
    }

    public void TakeDamage(float damage)
    {
        basicStats.currentHealth -= damage;

        if (basicStats.currentHealth <= 0f) //fin de la partida
        {
            basicStats.currentHealth = 0f;
        }

        vitalOrganUIRef.UpdateHealthBar(basicStats.currentHealth, basicStats.maxHealth);
        OnVitalOrganDamaged?.Invoke();
    }

    public virtual void Reset()
    {
        
    }
}
