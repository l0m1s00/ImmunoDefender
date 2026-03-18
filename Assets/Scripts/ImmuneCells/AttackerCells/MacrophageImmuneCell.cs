using System;
using UnityEngine;

public class MacrophageImmuneCell : AttackerCell
{
    private float deleteToxicWasteTime;
    private float eatTimer;
    private bool canEatWaste = false;
    private ToxicWaste currentWasteTarget;

    //EVENTOS
    public static event Action<float> OnToxicWasteCleaned;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is MacrophageImmuneCellData macrophageData)
        {
            deleteToxicWasteTime = macrophageData.deleteToxicWasteCooldown;
            eatTimer = deleteToxicWasteTime;
        }
    }

    protected override void ActionsFSM()
    {
        if (currentState == State.Clean) CleanActions();
        else base.ActionsFSM();
    }

    private void CleanActions()
    {
        if (canEatWaste) EatWaste();
        else MoveTowardsTarget(currentWasteTarget.gameObject);
    }

    protected override void HandleActionsFSM()
    {
        if (currentWasteTarget == null) RequestNewToxicWaste();

        if (currentState != State.Clean && currentWasteTarget != null)
        {
            currentTarget = null;
            currentState = State.Clean;
            canEatWaste = false;
            eatTimer = deleteToxicWasteTime;
        }

        if (currentState == State.Clean) HandleCleanState();
        else base.HandleActionsFSM();
    }

    private void HandleCleanState()
    {
        if (currentWasteTarget == null || !currentWasteTarget.gameObject.activeInHierarchy)
        {
            currentWasteTarget = null;
            currentState = State.Return;
            return;
        }

        float distanceToWaste = Vector2.Distance(transform.position, currentWasteTarget.transform.position);
    
        if (distanceToWaste <= attackRange && !canEatWaste)
        {
            canEatWaste = true;
        }
    }

    private void EatWaste()
    {
        eatTimer -= Time.deltaTime;

        if (eatTimer <= 0f)
        {
            OnToxicWasteCleaned?.Invoke(currentWasteTarget.GetToxicityValue());
            currentWasteTarget.EliminateWaste();
            currentWasteTarget = null;
            canEatWaste = false;
            currentState = State.Return;
        }
    }

    public void RequestNewToxicWaste()
    {
        if (headquarters is MacrophageTower macrophageTower)
        {
            ToxicWaste newWasteTarget = macrophageTower.RequestWasteTarget();

            if (newWasteTarget != null)
            {
                currentWasteTarget = newWasteTarget;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        deleteToxicWasteTime = 0f;
        eatTimer = 0f;
        currentWasteTarget = null;
        canEatWaste = false;
    }
}
