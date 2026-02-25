using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerSelectionManager : MonoBehaviour
{
    public static TowerSelectionManager Instance;

    [Header("Tower Data")]
    [SerializeField] private List<TowerCardDataSO> towerSO;

    [Header("UI References")]
    [SerializeField] private Transform towerCardsContainer;
    [SerializeField] private List<Transform> cardHolders;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private float maxFanOffset;

    private List<TowerCardUI> towerCards = new List<TowerCardUI>();
    private int currentSelectedCardIndex = -1; //nada seleccionado

    //EVENTOS
    public static event Action<TowerCardDataSO> OnTowerCardSelected;

    private void OnEnable()
    {
        Inputs.OnRightClickPerformed += DeselectTower;
    }

    private void OnDisable()
    {
        Inputs.OnRightClickPerformed -= DeselectTower;
    }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        CreateCards();
    }

    private void Update()
    {
        
    }

    private void CreateCards()
    {
        GameObject newCard;
        TowerCardUI cardUI;
        float a;
        float angle;
        for (int i = 0; i < towerSO.Count; i++)
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
            cardUI = newCard.GetComponent<TowerCardUI>();
            cardUI.CardSetup(towerSO[i], i);

            towerCards.Add(cardUI);
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

        OnTowerCardSelected?.Invoke(towerSO[currentSelectedCardIndex]); //informar que ha saido seleccionada una torre
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
}
