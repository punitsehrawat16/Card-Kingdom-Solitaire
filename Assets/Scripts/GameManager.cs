using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Center Card")] 
    [SerializeField] private CardData _centerCardData;
    [SerializeField] private Transform _centerCardPos;
    
    [Header("References")] 
    [SerializeField] private DeckManager _deckManager;
    
    [Header("Player Card")] 
    [SerializeField] private Transform[] _playerCardPos;
    [SerializeField] private List<CardData> cardsInHand = new List<CardData>(5);

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        Card.CardClicked += PlayCard;
    }

    private void Start()
    {
        CreateDeck();
        DrawCenterCard();
        DrawPlayerHandCards();
    }
    
    private void CreateDeck()
    {
        _deckManager.CreateAndShuffleDeck();
    }
    
    private void DrawCenterCard()
    {
        var card = _deckManager.DrawTopCard(_centerCardPos);
        _centerCardData = card.cardData;
        card.CardInteraction(true);
    }
    
    // called one time when game starts
    private void DrawPlayerHandCards()
    {
        for (int i = 0; i < 5; i++)
        {
            Card card = _deckManager.DrawTopCard(_playerCardPos[i]);
            cardsInHand.Add(card.cardData);
        }
    }

    private void DrawPlayerNewCard()
    {
        Debug.Log("Player Drawing New Card");
        /*
        CardData data = new CardData();
        data = _deckManager.DrawTopCard(_playerCardPos[5]);
        cardsInHand.Add(data);*/
    }

    private void PlayCard(Card card)
    {
        CardData data = card.cardData;
        bool isValid = CheckForEligibility(data);
        if (isValid)
        {
            // Make this card center card and disable the center card with it's object
        }
    }
    
    private bool CheckForEligibility(CardData data)
    {
        if (_centerCardData.Rank < data.Rank || _centerCardData.Suits == data.Suits) 
            return true;
        
        return false;
    }
    
}
