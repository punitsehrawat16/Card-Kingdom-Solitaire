using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    // event for letting player know how much cards are in the deck
    public static event Action<int> OnRemainingCard; // doesn't count the cards which are on the table already
    [Header("Cards Visuals")]
    [SerializeField] private List<Sprite> spriteCards = new List<Sprite>(); 
    
    
    [Header("Cards Data")] 
    [SerializeField] private List<GameObject> cardsInDeck = new List<GameObject>();
    [SerializeField] private List<GameObject> totalCards = new List<GameObject>();
    
    
    [Header("Cards Relevant")]
    [SerializeField] private GameObject cardHolder;
    [SerializeField] private GameObject cardPrefab;

    
    
    private void OnEnable()
    {
        GameState.OnGameStateChanged += HandleState;
    }

    private void OnDisable()
    {
        GameState.OnGameStateChanged -= HandleState;
    }

    private void HandleState(GameStates state)
    {
        if(state == GameStates.Playing)
            cardHolder.SetActive(true);
        else
            cardHolder.SetActive(false);
    }
    
    public void StartCreatingDeck()
    {
        totalCards.Clear();
        cardsInDeck.Clear();
        CreateDeck();
    }

    public void StartShufflingDeck()
    {
        ShufflingDeck();
    }
    
    private void CreateDeck()
    {
        int cardIndex = 0; // for sprites 
        foreach (Suits s in System.Enum.GetValues(typeof(Suits)))
        {
            foreach (Rank r in System.Enum.GetValues(typeof(Rank)))
            {
                CardData cardData = new CardData();
                cardData.Suits = s;
                cardData.Rank = r;
                
                GameObject card = Instantiate(cardPrefab,cardHolder.transform);
                
                Sprite sr = spriteCards[cardIndex];
                
                Card _card = card.GetComponent<Card>();
                if(_card != null)
                    _card.CardInitialise(cardData,sr);
                
                string str = cardData.Suits.ToString() +" "+ cardData.Rank.ToString();
                card.name = str;
                
                cardIndex++;
                
                cardsInDeck.Add(card);
                totalCards.Add(card);
            }
        }
    }
    
    private void ShufflingDeck()
    {
        for (int i = cardsInDeck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            var tempObj = cardsInDeck[i];
            cardsInDeck[i] = cardsInDeck[randomIndex];
            cardsInDeck[randomIndex] = tempObj;
        }
    }

    public Card DrawTopCard(Transform pos)
    {
        var card = cardsInDeck[0].GetComponent<Card>();
        
        cardsInDeck[0].transform.DOMove(pos.position,.25f).SetEase(Ease.OutCubic);
        cardsInDeck.RemoveAt(0);
        
        OnRemainingCard?.Invoke(cardsInDeck.Count);
        return card;
    }

    public void ResetDeckData()
    {
        ResetDeck();
    }
    private void ResetDeck()
    {
        cardsInDeck.Clear();
        foreach (var card in totalCards )
        {
            card.transform.position = cardHolder.transform.position;
            card.transform.SetParent(cardHolder.transform);
            card.gameObject.SetActive(true);
        }
        cardsInDeck.Clear();
        cardsInDeck = new List<GameObject>(totalCards);
    }
}