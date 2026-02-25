using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuilderManager : MonoBehaviour
{
    private TowerCardDataSO towerToBuildSO; //que torre hay que construir
    private GameObject currentGhostTower = null;
    private SpriteRenderer ghostRenderer = null;

    [Header("Containers")]
    [SerializeField] private GameObject towersContainer;

    [Header("Visual Feedback")]
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    private int currentGridX;
    private int currentGridY;
    private bool isCurrentCellValid;

    private void OnEnable()
    {
        TowerSelectionManager.OnTowerCardSelected += SetTowerToBuild; //suscripcion al evento de construccion de torres
        Inputs.OnLeftClickPerformed += BuildTower;
    }

    private void OnDisable()
    {
        TowerSelectionManager.OnTowerCardSelected -= SetTowerToBuild;
        Inputs.OnLeftClickPerformed -= BuildTower;
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        if (towerToBuildSO == null) return;

        UpdateGhostPosition();
    }

    private void SetTowerToBuild(TowerCardDataSO towerData)
    {
        if (currentGhostTower != null)
        {
            ghostRenderer = null;
            Destroy(currentGhostTower);
        }

        towerToBuildSO = towerData;

        if (towerToBuildSO != null)
        {
            Debug.Log("Torre asignada para la construccion: " + towerToBuildSO.towerName);
            currentGhostTower = Instantiate(towerToBuildSO.towerPrefab);
            ghostRenderer = currentGhostTower.GetComponent<SpriteRenderer>();
        }
        else //si es null, se ha deseleccionado la torre que se queria construir
        {
            Debug.Log("Construcción de torre cancelada");
        }
    }

    private void UpdateGhostPosition()
    {
        Vector2 mousePos = Inputs.Instance.mousePosition;
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);      
        currentGridX = Mathf.RoundToInt(worldPos.x);
        currentGridY = Mathf.RoundToInt(worldPos.y);
        //currentGhostTower.transform.position = worldPos; //para que la posición del fantasma siga al mouse
        currentGhostTower.transform.position = new Vector3(currentGridX, currentGridY); //para que la posición del fantasma sea una celda exacta

        if (GridManager.Instance != null)
        {
            isCurrentCellValid = GridManager.Instance.IsBuildableCell(currentGridX, currentGridY);
            SetGhostColor(isCurrentCellValid);
        }
    }

    private void SetGhostColor(bool isValid)
    {
        if (ghostRenderer != null) ghostRenderer.color = isValid ? validColor : invalidColor;
    }

    private void BuildTower()
    {
        //determinar si el click de construccion es valido y si le corresponde a esta funcion
        if (EventSystem.current.IsPointerOverGameObject() || !isCurrentCellValid || towerToBuildSO == null) return;

        GridManager.Instance.BlockCellByTower(currentGridX, currentGridY);
        Transform towersContainerTransform = towersContainer.transform;
        GameObject newTower = Instantiate(towerToBuildSO.towerPrefab, new Vector3(currentGridX, currentGridY, 0), Quaternion.identity);
        newTower.transform.SetParent(towersContainerTransform);
        //bloquear celda en el grid, ocupada
    }
}
