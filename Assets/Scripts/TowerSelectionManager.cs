using UnityEngine;
using System.Collections.Generic;

public class TowerSelectionManager : MonoBehaviour
{
    [Header("Tower Data")]
    [SerializeField] private List<TowerDataSO> towersObjects;

    [Header("UI References")]
    [SerializeField] private Transform towerCardsContainer;
    [SerializeField] private List<Transform> cardHolders;
    [SerializeField] private GameObject towerCardPrefab;

    private List<TowerCardUI> towersCards = new List<TowerCardUI>();
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
        for (int i = 0; i < towersObjects.Count; i++)
        {
            newCard = Instantiate(towerCardPrefab, towerCardsContainer);
            newCard.transform.position = cardHolders[i].position;
            cardUI = newCard.GetComponent<TowerCardUI>();
            cardUI.CardSetup(towersObjects[i], i, this);

            towersCards.Add(cardUI);
        }
    }

    public void SelectTower(int index)
    {
        currentSelectedCardIndex = index;
        Debug.Log(currentSelectedCardIndex);
        //towersCards[currentSelectedCardIndex].isSelected
        Debug.Log("Torre seleccionada: " + towersCards[currentSelectedCardIndex].GetCardName());
    }
}
