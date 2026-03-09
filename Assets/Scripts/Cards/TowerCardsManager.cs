using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerCardsManager : MonoBehaviour
{
    public static TowerCardsManager Instance;

    [Header("Tower Data")]
    [SerializeField] private List<TowerCardDataSO> towerSO;

    [Header("UI References")]
    [SerializeField] private Transform towerCardsContainer;
    [SerializeField] private List<Transform> cardHolders;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private float maxFanOffset;

    private List<TowerCard> towerCards = new List<TowerCard>();
    private int currentSelectedCardIndex = -1; //-1 nada seleccionado

    //EVENTOS
    public static event Action<TowerCard> OnTowerCardSelected;
    
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        Inputs.OnRightClickPerformed += DeselectTower;
        TowerBuilder.OnTowerBuild += DeselectTowerWrapper;
    }

    private void OnDisable()
    {
        Inputs.OnRightClickPerformed -= DeselectTower;
        TowerBuilder.OnTowerBuild -= DeselectTowerWrapper;
    }

    private void Start()
    {
        CreateCards();
    }

    private void CreateCards()
    {
        GameObject newCard;
        TowerCard cardLogic;
        float a;
        float angle;

        for (int i = 0; i < towerSO.Count; i++) //numero de cartas a crear
        {
            a = 0f;
            if (towerSO.Count > 1)
            {
                a = (float) i / (towerSO.Count-1);
                a = (a * 2f) - 1f;
            }

            angle = -a * maxFanOffset;
            newCard = Instantiate(towerCardPrefab, towerCardsContainer);
            newCard.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            newCard.transform.position = cardHolders[i].position;
            
            cardLogic = newCard.GetComponent<TowerCard>();
            cardLogic.InitializeCard(towerSO[i], i);

            towerCards.Add(cardLogic);
        }
    }

    public void SelectTower(int index)
    {
        if (currentSelectedCardIndex == index) //click sobre la misma carta seleccionada anteriormente
        {
            DeselectTower();
            return;
        }

        if (currentSelectedCardIndex != -1)
        {
            towerCards[currentSelectedCardIndex].SetSelected(false);
        }

        currentSelectedCardIndex = index;
        towerCards[currentSelectedCardIndex].SetSelected(true);

        OnTowerCardSelected?.Invoke(towerCards[currentSelectedCardIndex]); //informar que ha saido seleccionada una carta
    }

    public void DeselectTower()
    {
        if (currentSelectedCardIndex != -1)
        {
            towerCards[currentSelectedCardIndex].SetSelected(false);
            currentSelectedCardIndex = -1;
            OnTowerCardSelected?.Invoke(null); //informar que ya no se quiere seleccionar esa torre
        }
    }

    public void DeselectTowerWrapper(GameObject obj) //puente para conectar eventos que emplean metodos con parametros distintos
    {
        DeselectTower();
    }
}
