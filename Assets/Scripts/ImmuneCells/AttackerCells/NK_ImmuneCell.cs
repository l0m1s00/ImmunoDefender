using UnityEngine;

public class NK_ImmuneCell : AttackerCell
{
    [Header("Wander Settings")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float moveInTileMargin;

    protected NK_CellStats NK_CellStats => (NK_CellStats)basicStats;
    private bool isFree = true; //la celula no tiene nada que hacer, deambualar por la casilla
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer;
    private bool isRoaming = false;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is NK_ImmuneCellData NK_Data)
        {
            basicStats = new NK_CellStats(NK_Data);
        }

        if (target != null)
        {
            isFree = false;
        }
        else
        {
            GetNewTargetPostion();
        }
    }

    protected override void HandleActionsFSM()
    {
        base.HandleActionsFSM();

        if (currentTarget != null)
        {
            isFree = false; //tiene un objetivo, hacer acciones
            isWaiting = false;
            isRoaming = false;
        }
        else if (isRoaming)
        {
            HandleMovement(); //movimiento aleatorio por la casilla
        }
    }

    private void HandleMovement()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GetNewTargetPostion();
            }
        }
        else MoveInCell();
    }

    private void GetNewTargetPostion()
    {
        float rX = Random.Range(-moveInTileMargin, moveInTileMargin);
        float rY = Random.Range(-moveInTileMargin, moveInTileMargin);

        targetPosition = headquarters.transform.position + new Vector3(rX, rY, 0f);
    }

    private void MoveInCell()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, NK_CellStats.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    //cuando una NK no tiene objetivo y regresa al cuartel, no entra, deambula por la casilla
    protected override void EnterTower()
    {
        //llegada a la casilla de la torre, deambular por ella
        if (!isRoaming)
        {
            isRoaming = true;
            GetNewTargetPostion();
        }
    }

    protected override void ReturnToHeadquarters()
    {
        if (!isRoaming) base.ReturnToHeadquarters();
    }

    public override void Reset()
    {
        base.Reset();
        isFree = true;
        isWaiting = false;
    }
}
