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
    [SerializeField] private float maxFanOffset;

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
        float a;
        float angle;
        for (int i = 0; i < towerObjects.Count; i++)
        {
            a = 0f;
            if (towerObjects.Count > 1)
            {
                a = (float) i / (towerObjects.Count-1);
                a = (a * 2f) - 1f;
            }

            angle = -a * maxFanOffset;
            newCard = Instantiate(towerCardPrefab, towerCardsContainer);
            newCard.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
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
