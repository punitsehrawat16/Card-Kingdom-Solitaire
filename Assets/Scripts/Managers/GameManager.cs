using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // events
    public static event Action OnCorrectCard;    // for score manager, increment score
    public static event Action<string> OnErrorNotification;  // for player's awareness,if player makes moves which aren't valid
    public static event Action<string> OnGameNotification;  
    
    [Header("Center Card")] 
    [SerializeField] private Card _centerCard;
    [SerializeField] private Transform _centerCardPos;
    
    [Header("References")] 
    [SerializeField] private DeckManager _deckManager;

    [Header("Player Card")]
    [SerializeField] private Transform[] _playerCardSlot = new Transform[5];
    [SerializeField] private Card[] cardsInHand = new Card[5];

    
    private void OnEnable()
    {
        Card.CardClicked += PlayingCard;
    }

    private void OnDisable()
    {
        Card.CardClicked -= PlayingCard;
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
        _centerCard = _deckManager.DrawTopCard(_centerCardPos);
        _centerCard.CardInteraction(false);
    }
    
    // called one time when game starts
    private void DrawPlayerHandCards()
    {
        for (int i = 0; i < 5; i++)
        {
            Card card = _deckManager.DrawTopCard(_playerCardSlot[i]);
            cardsInHand[i] = card;
            card.CardInteraction(true);
        }
    }


    private void PlayingCard(Card card)
    {
        bool isValid = CheckForEligibility(card);
        if (isValid)
        {
            RemoveCardFromPlayerHands(card);
            _centerCard.gameObject.SetActive(false);
            _centerCard = card;
            _centerCard.transform.position = _centerCardPos.position;
            _centerCard.CardInteraction(false);
            OnCorrectCard?.Invoke();
            OnGameNotification?.Invoke("+10 Points");
        }
        else
        {
            Debug.Log("Invalid Card!");
            Debug.Log("Find a card with same suit Or higher rank");
        } 
    }
    public void GetNewCard()
    {
        DrawPlayerNewCard();
    }

    private void DrawPlayerNewCard()
    {
        int emptySlot = FindEmptySlot(); // checks that player can draw another card & find empty slot
        if (emptySlot == -1)
        {
            OnErrorNotification?.Invoke("Hand is full. Play a valid card before drawing another.");
            return;
        }
        
        Card card = _deckManager.DrawTopCard(_playerCardSlot[emptySlot]);
        cardsInHand[emptySlot] = card;
        card.CardInteraction(true);
    }
    private int FindEmptySlot()
    {
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            if (cardsInHand[i] == null)
                return i;
        }
        return -1;
    }

    private void RemoveCardFromPlayerHands(Card card)
    {
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            if(cardsInHand[i] == null)
                continue;
            if (cardsInHand[i].Equals(card))
            {
               // Debug.Log("found " + card.cardData.Rank + " " + card.cardData.Suits);
                cardsInHand[i] = null;
                break;
            }
        }
    }
    // game rule
    private bool CheckForEligibility(Card card)
    {
        if (_centerCard.cardData.Rank < card.cardData.Rank || _centerCard.cardData.Suits == card.cardData.Suits) 
            return true;
        
        OnErrorNotification?.Invoke("Play a card with the same suit or a higher rank.");
        return false;
    }
    // game win/loss
    
}
