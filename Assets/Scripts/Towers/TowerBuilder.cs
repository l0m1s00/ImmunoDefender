using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuilder : MonoBehaviour, IResettable
{
    [Header("Components")]
    [SerializeField] private TowerManagerUI towerManagerUIRef;
    [Header("Containers")]
    [SerializeField] private Transform towersContainer;

    private HashSet<GameObject> towerHolderCellObjects;
    private TowerCard towerCardToBuild = null; //que torre hay que construir
    private TowerData towerToBuildData = null; //datos de la torre que hay que construir
    private GameObject currentGhostTower = null;
    private Camera mainCamera;

    private int currentGridX;
    private int currentGridY;
    private bool isCurrentCellValid;

    //EVENTOS
    public static event Action<Tower> OnTowerBuild; //notificar de la construccion de una torre

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        TowerCardsManager.OnTowerCardSelected += SetTowerToBuild; //suscripcion al evento de construccion de torres (carta seleccionada)
        Inputs.OnLeftClickPerformed += BuildTower;
        GridManager.OnMapGenerationCompleted += GetTowerHolderObjects;
    }

    private void OnDisable()
    {
        TowerCardsManager.OnTowerCardSelected -= SetTowerToBuild;
        Inputs.OnLeftClickPerformed -= BuildTower;
        GridManager.OnMapGenerationCompleted -= GetTowerHolderObjects;
    }

    private void Update()
    {
        if (towerCardToBuild == null) return;

        if (currentGhostTower != null && GameManager.Instance.GetCurrentGameState() == GameManager.GameState.Playing) UpdateGhostPosition();
    }

    private void SetTowerToBuild(TowerCard towerCardToBuild)
    {
        if (towerCardToBuild != null)
        {
            if (towerToBuildData != null && currentGhostTower != null)
            {
                towerManagerUIRef.ResetGhostRender();
                PoolManager.Instance.DespawnGhostTower(towerToBuildData, currentGhostTower);
            }

            //datos de la torre a construir
            this.towerCardToBuild = towerCardToBuild;
            towerToBuildData = this.towerCardToBuild.GetTowerCardData();
            //fantasma para visualizar construccion
            currentGhostTower = PoolManager.Instance.SpawnGhostTower(towerToBuildData);
            towerManagerUIRef.SetupGhostRender(currentGhostTower);
        }
        else
        {
            towerManagerUIRef.ResetGhostRender();
            PoolManager.Instance.DespawnGhostTower(towerToBuildData, currentGhostTower);
            this.towerCardToBuild = null;
            towerToBuildData = null;
            currentGhostTower = null;
            //Debug.Log("Construccion de torre cancelada");
        }
    }

    private void UpdateGhostPosition()
    {
        Vector2 mousePos = Inputs.Instance.mousePosition;
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);      

        currentGridX = Mathf.RoundToInt(worldPos.x);
        currentGridY = Mathf.RoundToInt(worldPos.y);
        currentGhostTower.transform.position = new Vector3(currentGridX, currentGridY); //para que la posición del fantasma sea una celda exacta
        
        //currentGhostTower.transform.position = worldPos; //para que la posición del fantasma siga al mouse

        if (GridManager.Instance != null)
        {
            isCurrentCellValid = GridManager.Instance.IsBuildableCell(currentGridX, currentGridY);
            towerManagerUIRef.SetGhostRenderVisualization(isCurrentCellValid);
        }
    }

    private void BuildTower()
    {
        //determinar si el click de construccion es valido y si le corresponde a esta funcion
        if (EventSystem.current.IsPointerOverGameObject() || !isCurrentCellValid || towerCardToBuild == null ||
            GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Playing) return;

        EconomyManager.Instance.SpendCurrency(towerToBuildData.cost);
        GameObject newTowerObj = PoolManager.Instance.SpawnTower(towerToBuildData);
        
        foreach (GameObject towerHolderObj in towerHolderCellObjects)
        {
            if (towerHolderObj.transform.position.x == currentGridX && towerHolderObj.transform.position.y == currentGridY)
            {
                Vector3 buildHolderPos = towerHolderObj.transform.GetChild(0).transform.position;
                newTowerObj.transform.position = buildHolderPos;
            }
        }

        newTowerObj.transform.SetParent(towersContainer);
        newTowerObj.GetComponent<Tower>().Initialize(towerToBuildData);
        OnTowerBuild?.Invoke(newTowerObj.GetComponent<Tower>());
    }

    private void GetTowerHolderObjects()
    {
        towerHolderCellObjects = GridManager.Instance.GetTowerHolderCellsObjects();
    }

    public void Reset() //reseteo del fantasma
    {
        
    }
}
