using UnityEngine;

public class VitalOrganStats
{
    public string vitalOrganName;
    public float maxHealth;
    public float currentHealth;
    public VitalOrganData data;

    public VitalOrganStats(VitalOrganData data)
    {
        this.data = data;
        vitalOrganName = data.vitalOrganName;
        maxHealth = data.health;
        currentHealth = maxHealth;
    }
}
