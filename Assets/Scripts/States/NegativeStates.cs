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
    }

    private void OnDisable()
    {
        FungiEnemy.OnToxicWasteGenerated -= IncreaseToxicity;
    }

    public void IncreaseToxicity(float toxicAmount)
    {
        if (currentToxicity + toxicAmount >= maxToxicity)
        {
            currentToxicity = maxToxicity;
            septicShockState = true;
        }
        else
        {
            currentToxicity += toxicAmount;
        }

        negativeStatesUIRef.UpdateToxicBar(currentToxicity / maxToxicity);
    }
}
