using UnityEngine;

public class NegativeStates : MonoBehaviour
{
    [SerializeField] private NegativeStatesUI negativeStatesUIRef;
    [SerializeField] private float maxToxicity;
    private float currentToxicity = 0f;
    private bool septicShockState = false;

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

    private void DecreaseToxicity(ToxicWaste waste)
    {
        float cleanAmount = waste.GetToxicityValue();

        currentToxicity -= cleanAmount;

        if (currentToxicity < 0f) currentToxicity = 0f;

        if (septicShockState && currentToxicity < maxToxicity) septicShockState = false;

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
        }

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }
}
