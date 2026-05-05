using System;
using UnityEngine;

public class NegativeStates : MonoBehaviour
{
    [SerializeField] private NegativeStatesUI negativeStatesUIRef;
    [SerializeField] private float maxToxicity;
    [SerializeField] private float attackToxicityDamage;
    [SerializeField] private float attackToxicityCooldown;
    private float currentToxicity = 0f;
    private bool septicShockState = false;
    private float attackToxicityTimer = 0f;

    //EVENTOS
    public static event Action<float> OnAttackVitalOrgan;

    private void OnEnable()
    {
        FungiEnemy.OnToxicWasteGenerated += IncreaseToxicity;
        MacrophageImmuneCell.OnToxicWasteCleaned += DecreaseToxicity;
    }

    private void OnDisable()
    {
        FungiEnemy.OnToxicWasteGenerated -= IncreaseToxicity;
        MacrophageImmuneCell.OnToxicWasteCleaned -= DecreaseToxicity;
    }

    private void Update()
    {
        if (septicShockState)
        {
            ToxicityAttack();
        }
    }

    private void DecreaseToxicity(ToxicWaste waste)
    {
        float cleanAmount = waste.GetToxicityValue();

        currentToxicity -= cleanAmount;

        if (currentToxicity < 0f) currentToxicity = 0f;

        if (septicShockState && currentToxicity < maxToxicity)
        {
            septicShockState = false;
        }

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }

    private void IncreaseToxicity(ToxicWaste waste)
    {
        float toxicAmount = waste.GetToxicityValue();

        currentToxicity += toxicAmount;

        if (currentToxicity >= maxToxicity)
        {
            currentToxicity = maxToxicity;
            septicShockState = true;
            StartToxicityAttack();
        }

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }

    private void StartToxicityAttack()
    {
        attackToxicityTimer = attackToxicityCooldown;
    }

    private void ToxicityAttack()
    {
        attackToxicityTimer -= Time.deltaTime;

        if (attackToxicityTimer <= 0f)
        {
            OnAttackVitalOrgan?.Invoke(attackToxicityDamage);
            attackToxicityTimer = attackToxicityCooldown;
        }
    }
}
