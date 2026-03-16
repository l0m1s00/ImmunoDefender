using UnityEngine;

public class NegativeStates : MonoBehaviour
{
    [SerializeField] private NegativeStatesUI negativeStatesUIRef;
    [SerializeField] private float maxToxicity;
    private float currentToxicity = 0f;
    private bool septicShockState = false;

    private void OnEnable()
    {
        FungiEnemy.OnIncreaseToxicBar += IncreaseToxicity;
        MacrophageImmuneCell.OnToxicWasteCleaned += DecreaseToxicity;
    }

    private void OnDisable()
    {
        FungiEnemy.OnIncreaseToxicBar -= IncreaseToxicity;
        MacrophageImmuneCell.OnToxicWasteCleaned -= DecreaseToxicity;
    }


    private void Update()
    {
        Debug.Log(currentToxicity);
    }

    private void DecreaseToxicity(float cleanAmount)
    {
        currentToxicity -= cleanAmount;

        if (currentToxicity < 0f) currentToxicity = 0f;

        if (septicShockState && currentToxicity < maxToxicity) septicShockState = false;

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }

    private void IncreaseToxicity(float toxicAmount)
    {
        currentToxicity += toxicAmount;

        if (currentToxicity >= maxToxicity)
        {
            currentToxicity = maxToxicity;
            septicShockState = true;
        }

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }
}
