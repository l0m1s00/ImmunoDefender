using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuilder : MonoBehaviour
{
    [Header("Containers")]
    [SerializeField] private GameObject towersContainer;

    [Header("Visual Feedback")]
    [SerializeField] private Color invalidColor;
    private Color validColor;

    private TowerCard towerToBuild = null; //que torre hay que construir
    private TowerCardDataSO towerToBuildData = null; //datos de la torre que hay que construir
    private GameObject currentGhostTower = null;
    private SpriteRenderer ghostTowerRenderer = null;
    private Camera mainCamera;

    private int currentGridX;
    private int currentGridY;
    private bool isCurrentCellValid;

    //EVENTOS
    public static event Action<GameObject> OnTowerBuild; //notificar de la construccion de una torre

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        TowerCardsManager.OnTowerCardSelected += SetTowerToBuild; //suscripcion al evento de construccion de torres (carta seleccionada)
        TowerCardsManager.OnTowerCardSelected += SetGhostTower;
        Inputs.OnLeftClickPerformed += BuildTower;
    }

    private void OnDisable()
    {
        TowerCardsManager.OnTowerCardSelected -= SetTowerToBuild;
        TowerCardsManager.OnTowerCardSelected -= SetGhostTower;
        Inputs.OnLeftClickPerformed -= BuildTower;
    }

    private void Update()
    {
        if (towerToBuild == null) return;

        UpdateGhostPosition();
    }

    private void SetTowerToBuild(TowerCard towerCardToBuild)
    {
        if (towerCardToBuild != null)
        {
            towerToBuild = towerCardToBuild;
            towerToBuildData = towerToBuild.GetTowerCardData();
            //Debug.Log("Torre asignada para la construccion: " + towerToBuildSO.towerName);
        }
        else
        {
            towerToBuild = null;
            towerToBuildData = null;
            //Debug.Log("Construccion de torre cancelada");
        }
    }

    private void SetGhostTower(TowerCard towerCardToBuild)
    {
        ghostTowerRenderer = null; //se asume que la construccion de torre ha sido cancelada
        Destroy(currentGhostTower);

        if (towerCardToBuild != null)
        {
            currentGhostTower = Instantiate(towerCardToBuild.GetTowerCardData().towerPrefab);

            ghostTowerRenderer = currentGhostTower.GetComponent<SpriteRenderer>();
            validColor = ghostTowerRenderer.color;
            validColor.a = invalidColor.a;
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
            SetGhostColor(isCurrentCellValid);
        }
    }

    private void SetGhostColor(bool isValid)
    {
        if (ghostTowerRenderer != null) ghostTowerRenderer.color = isValid ? validColor : invalidColor;
    }

    private void BuildTower()
    {
        //determinar si el click de construccion es valido y si le corresponde a esta funcion
        if (EventSystem.current.IsPointerOverGameObject() || !isCurrentCellValid || towerToBuild == null) return;

        Transform towersContainerTransform = towersContainer.transform;
        GameObject newTower = Instantiate(towerToBuildData.towerPrefab, new Vector3(currentGridX, currentGridY, 0), Quaternion.identity);
        newTower.transform.SetParent(towersContainerTransform);
        //Debug.Log("Torre construida: " + (currentGridX, currentGridY) + " | " + towerToBuildData.name);
        OnTowerBuild?.Invoke(newTower);
    }
}
