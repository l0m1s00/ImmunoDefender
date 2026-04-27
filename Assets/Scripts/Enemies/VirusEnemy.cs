using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirusEnemy : Enemy
{
    //Infect Towers Settings
    private HashSet<CellularFarmTower> attemptedFarms = new HashSet<CellularFarmTower>(); //para evitar que se puedan realizar varios intentos de infeccion sobre la misma granja
    private Vector2Int currentCell = new Vector2Int(-999, -999); //celda anterior redondeada a int para saber si hemos cambiado de casilla
    //Stats
    protected VirusEnemyStats virusEnemyStats => (VirusEnemyStats)basicStats;

    private void OnEnable()
    {
        TowerManager.OnNewCellularFarmBuilt += CheckForAdjacentNewFarmsInCurrentCell;
    }

    private void OnDisable()
    {
        TowerManager.OnNewCellularFarmBuilt -= CheckForAdjacentNewFarmsInCurrentCell;
    }

    public override void Initialize(List<Cell> newRoute, EnemyData data)
    {
        base.Initialize(newRoute, data);

        if (data is VirusEnemyData virusData)
        {
            basicStats = new VirusEnemyStats(virusData);
        }
    }

    private void Update()
    {
        Move();
        HandleInfection();
    }

    private void HandleInfection()
    {
        //tomar casilla actual
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Vector2Int newCell = new Vector2Int(x, y);

        if (currentCell != newCell) //cambio de casilla
        {
            currentCell = newCell;
            CheckForAdjacentFarms(x, y);
        }
    }

    private void CheckForAdjacentFarms(int virusCurrentX, int virusCurrentY)
    {
        foreach(CellularFarmTower cellularFarmTower in TowerManager.Instance.GetBuiltCellularFarmTowers())
        {
            if (attemptedFarms.Contains(cellularFarmTower)) continue;

            int cellularFarmX = Mathf.RoundToInt(cellularFarmTower.transform.position.x);
            int cellularFarmY = Mathf.RoundToInt(cellularFarmTower.transform.position.y);
            //distancia entre el virus y la granja mediante Distancia de Manhattan (adyacencia en cruz, no permitidas las diagonales)
            int diffX = Mathf.Abs(virusCurrentX - cellularFarmX);
            int diffY = Mathf.Abs(virusCurrentY - cellularFarmY);
            //si la suma de diferencias es 1 quiere decir que hay una granja adyacente (en cruz, arriba abajo izquierda derecha)
            if (diffX + diffY == 1) TryToInfectCellularFarm(cellularFarmTower);
        }
    }

    //si una nueva granja es construida adyacente a una casilla en la que ya se han buscado granjas esta no se valora para la infeccion
    //se vuelve a mirar la celda en el momento en el que la nueva granja es construida
    private void CheckForAdjacentNewFarmsInCurrentCell()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Vector2Int newCell = new Vector2Int(x, y);
        CheckForAdjacentFarms(x, y);
    }

    private void TryToInfectCellularFarm(CellularFarmTower cellularFarmTower)
    {
        //solo se permite un intento de infeccion
        attemptedFarms.Add(cellularFarmTower);
        bool success = cellularFarmTower.ResolveInfectionAttempt(virusEnemyStats.infectionProbability);

        if (success) Debug.Log("Granja infectada");
        //else Debug.Log("Infeccion fallida");
    }

    public override void Reset()
    {
        base.Reset();
        attemptedFarms.Clear();
        currentCell = new Vector2Int(-999, -999); //new Vector2Int() == (0, 0) --> si puede ser una casilla, -999 -999 no
    }
}