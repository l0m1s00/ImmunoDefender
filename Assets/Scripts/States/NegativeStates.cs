using System;
using UnityEngine;

public enum ToxicityLevel { Low, Medium, Max };

public class NegativeStates : MonoBehaviour
{
    [SerializeField] private NegativeStatesUI negativeStatesUIRef;
    [SerializeField] private float maxToxicity;
    [SerializeField] private float attackToxicityDamage;
    [SerializeField] private float attackToxicityCooldown;
    [SerializeField] private AudioEventData bipAudioEvent;
    [SerializeField] private AudioEventData failureAudioEvent;
    [SerializeField] private AudioEventData shockAudioEvent;
    private float currentToxicity = 0f;
    private bool septicShockState = false;
    private float attackToxicityTimer = 0f;
    private ToxicityLevel currentToxicityLevel = ToxicityLevel.Low;

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

    private void CheckToxicitylevel()
    {
        float toxicityValue = currentToxicity / maxToxicity;
        if (toxicityValue > 1f) toxicityValue = 1f;

        if (toxicityValue < 0.5f) currentToxicityLevel = ToxicityLevel.Low;
        else if (toxicityValue >= 0.5f && toxicityValue < 1f) currentToxicityLevel = ToxicityLevel.Medium;
        else currentToxicityLevel = ToxicityLevel.Max;
    }

    private void DecreaseToxicity(ToxicWaste waste)
    {
        float cleanAmount = waste.GetToxicityValue();
        currentToxicity -= cleanAmount;
        if (currentToxicity < 0f) currentToxicity = 0f;

        CheckToxicitylevel();

        if (septicShockState && currentToxicityLevel != ToxicityLevel.Max)
        {
            septicShockState = false;
            AudioManager.Instance.PlaySFX(bipAudioEvent, transform.position);
        }

        float toxicityPer = currentToxicity / maxToxicity;
        if (toxicityPer > 1f) toxicityPer = 1f;

        negativeStatesUIRef.UpdateToxicBar(toxicityPer, currentToxicityLevel);
    }

    private void IncreaseToxicity(ToxicWaste waste)
    {
        float toxicAmount = waste.GetToxicityValue();
        currentToxicity += toxicAmount;

        CheckToxicitylevel();

        if (currentToxicityLevel == ToxicityLevel.Max && !septicShockState)
        {
            septicShockState = true;
            StartToxicityAttack();
        }
        //else if (currentToxicityLevel == ToxicityLevel.Medium)
        //{

        //}

        float toxicityPer = currentToxicity / maxToxicity;
        if (toxicityPer > 1f) toxicityPer = 1f;

        negativeStatesUIRef.UpdateToxicBar(toxicityPer, currentToxicityLevel);
    }

    private void StartToxicityAttack()
    {
        AudioManager.Instance.PlaySFX(failureAudioEvent, transform.position, 0f);
        attackToxicityTimer = attackToxicityCooldown;
    }

    private void ToxicityAttack()
    {
        attackToxicityTimer -= Time.deltaTime;

        if (attackToxicityTimer <= 0f)
        {
            AudioManager.Instance.PlaySFX(shockAudioEvent, transform.position, 0f);
            OnAttackVitalOrgan?.Invoke(attackToxicityDamage);
            attackToxicityTimer = attackToxicityCooldown;
        }
    }
}
