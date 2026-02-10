using UnityEngine;
using System.Collections.Generic;

public class TowerSelectionManager : MonoBehaviour
{
    [Header("Tower Data")]
    [SerializeField] private List<TowerCardDataSO> towerObjects;

    [Header("UI References")]
    [SerializeField] private Transform towerCardsContainer;
    [SerializeField] private List<Transform> cardHolders;
    [SerializeField] private GameObject towerCardPrefab;

    private List<TowerCardUI> towerCards = new List<TowerCardUI>();
    private int currentSelectedCardIndex = -1; //nada seleccionado

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
        for (int i = 0; i < towerObjects.Count; i++)
        {
            newCard = Instantiate(towerCardPrefab, towerCardsContainer);
            newCard.transform.position = cardHolders[i].position;
            cardUI = newCard.GetComponent<TowerCardUI>();
            cardUI.CardSetup(towerObjects[i], i, this);

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
    }

    public void DeselectTower()
    {
        if (currentSelectedCardIndex != -1)
        {
            towerCards[currentSelectedCardIndex].SetSelected(false);
        }

        currentSelectedCardIndex = -1;
    }
}
