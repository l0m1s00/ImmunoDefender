using System.Collections.Generic;
using UnityEngine;

public class StemCell : ImmuneCell
{
    [Header("Wander Settings")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float moveInTileMargin;

    protected StemCellStats stemCellStats => (StemCellStats)basicStats;
    private float getInfectedTimer;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is StemCellData stemCellData)
        {
            basicStats = new StemCellStats(stemCellData);
            getInfectedTimer = stemCellStats.getInfectionCooldown;
        }

        GetNewTargetPostion();
    }

    protected override void Update()
    {
        base.Update();

        if (stemCellStats.infected)
        {
            BecomeInfected();
        }

        HandleMovement();
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

    private void MoveInCell()
    {
        //el movimiento de la celula madre es puramente estetico, no tiene un fin real
        //siempre se mueve dentro de los limites de la casilla en la que esta contenida la torre padre
        //busca una posicion valida dentro de esa casilla, espera un rato y pasa a otra posicion
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stemCellStats.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void GetNewTargetPostion()
    {
        float rX = Random.Range(-moveInTileMargin, moveInTileMargin);
        float rY = Random.Range(-moveInTileMargin, moveInTileMargin);

        targetPosition = headquarters.transform.position + new Vector3(rX, rY, 0f);
    }

    private void BecomeInfected()
    {
        getInfectedTimer -= Time.deltaTime;

        if (getInfectedTimer <= 0f) //tras un tiempo infectada la celula intentara infectar al resto de las celulas asociadas a la torre
        {

        }
    }

    public void GetInfection()
    {
        stemCellStats.infected = true;
    }


    public override List<EntitiyStat> GetStats()
    {
        return stemCellStats.GetStats();
    }

    public override void Reset()
    {
        base.Reset();
        isWaiting = false;
    }
}
