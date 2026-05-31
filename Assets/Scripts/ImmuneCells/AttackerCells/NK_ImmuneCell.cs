using UnityEngine;
using System;
using UnityEngine.UI;

public class NK_ImmuneCell : AttackerCell, IStateInfoProvider
{
    [Header("Wander Settings")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float moveInTileMargin;

    //Provider State Settings
    private string currentStateText;

    protected NK_CellStats NK_CellStats => (NK_CellStats)basicStats;
    private NK_ImmuneCellData nkData;
    //private bool isFree = true; //la celula no tiene nada que hacer, deambualar por la casilla
    private Vector3 targetRoamingPosition;
    private bool isWaiting = false;
    private float waitTimer;
    private bool isRoaming = false;
    private bool isInvestigating = false;
    private bool isAnalyzingFarm = false;
    private CellularFarmTower currentFarmToInvestigate = null;
    private float currentAnalysisAmount = 0f;

    public event Action<string> OnStateInfoChanged;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is NK_ImmuneCellData NK_Data)
        {
            basicStats = new NK_CellStats(NK_Data);
            nkData = NK_Data;
        }

        currentStateText = nkData.noCommandText;

        if (target == null)
        {
            GetNewTargetPostion();
        }
    }

    public string GetCurrentStateInfo()
    {
        return currentStateText;
    }

    protected override void HandleActionsFSM()
    {
        if (currentState == State.InvestigateFarm) //bloquear todas las acciones mientras investiga
        {
            HandleInvestigation();
            return;
        }

        base.HandleActionsFSM();

        if (currentTarget != null)
        {
            isWaiting = false;
            isRoaming = false;
        }
        else if (isRoaming)
        {
            //CAMBIAR PARA QUE LA ACCION DE MOVIMIENTO LO HAGA ACTIONS_FSM
            HandleRoamingMovement(); //movimiento aleatorio por la casilla
        }
    }

    protected override void ActionsFSM()
    {
        if (currentState == State.InvestigateFarm)
        {
            FarmInvestigation();
            return;
        }

        base.ActionsFSM();
    }

    private void FarmInvestigation()
    {
        if (!isAnalyzingFarm) MoveTowardsFarm(); //aproximarse a la granja designada
        else //llegada a la granja, comenzar analisis
        {
            AnalyzeFarm();
        }
    }

    private void MoveTowardsFarm()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentFarmToInvestigate.transform.position, NK_CellStats.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentFarmToInvestigate.transform.position) <= 0.1f) //celula llega a la granja objetivo, comenzar investigacion
        {
            isAnalyzingFarm = true;

            currentStateText = nkData.analizingFarmText;
            OnStateInfoChanged?.Invoke(currentStateText);
        }
    }

    private void AnalyzeFarm()
    {
        float amountPerSecond = NK_CellStats.analysisAmount / NK_CellStats.analysisInterval;
        currentAnalysisAmount += amountPerSecond * Time.deltaTime;

        if (currentAnalysisAmount >= NK_CellStats.maxAnalysisAmount) //analisis terminado
        {
            ConclusionAnalysis();
            EndInvestigation();
            //ResetAnalysis();
        }
        else
        {
            if (immuneCellUIRef is NK_CellUI nkUI) nkUI.UpdateAnalysisBar(currentAnalysisAmount / NK_CellStats.maxAnalysisAmount);
        }
    }

    private void ConclusionAnalysis() //determinar si el jugador ha perdido el tiempo al enviar a la NK o esta ha conseguido curar la infeccion
    {
        Sprite analysisResultIcon;

        if (currentFarmToInvestigate.CureInfectedStemCells()) //habia infeccion
        {
            analysisResultIcon = nkData.goodAnalysisIcon;
        }
        else //tiempo perdido
        {
            analysisResultIcon = nkData.badAnalysisIcon;
        }

        if (immuneCellUIRef is NK_CellUI nkUI) nkUI.UpdateAnalysisResultIcon(analysisResultIcon);
    }

    private void EndInvestigation()
    {
        TowerManager.Instance.RemoveInvestigatedFarm(currentFarmToInvestigate);
        currentFarmToInvestigate = null;
        currentStateText = nkData.noCommandText;

    }

    private void HandleInvestigation()
    {
        if (currentFarmToInvestigate == null || !currentFarmToInvestigate.gameObject.activeInHierarchy)
        {
            isInvestigating = false;
            isAnalyzingFarm = false;

            ResetAnalysis();
            OnStateInfoChanged?.Invoke(currentStateText);

            currentState = State.Return;
        }
    }

    private void ResetAnalysis()
    {
        currentAnalysisAmount = 0f;
        if (immuneCellUIRef is NK_CellUI nkUI) nkUI.HideAnalysisBar();
    }

    public bool IsInvestigating()
    {
        return isInvestigating;
    }

    private void HandleRoamingMovement()
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

    public void SetInvestigation(CellularFarmTower targetFarm)
    {
        currentTarget = null;
        isRoaming = false;
        isWaiting = false;

        isInvestigating = true;
        currentFarmToInvestigate = targetFarm;
        TowerManager.Instance.AddInvestigatedFarm(targetFarm);
        currentState = State.InvestigateFarm;

        currentStateText = nkData.movingToFarmText;
        OnStateInfoChanged?.Invoke(currentStateText);
    }

    private void GetNewTargetPostion()
    {
        float rX = UnityEngine.Random.Range(-moveInTileMargin, moveInTileMargin);
        float rY = UnityEngine.Random.Range(-moveInTileMargin, moveInTileMargin);

        targetRoamingPosition = headquarters.transform.position + new Vector3(rX, rY, 0f);
    }

    private void MoveInCell()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetRoamingPosition, NK_CellStats.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetRoamingPosition) < 0.01f)
        {
            isWaiting = true;
            waitTimer = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
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
        isWaiting = false;
        isRoaming = false;
        isInvestigating = false;
        currentFarmToInvestigate = null;
    }
}
