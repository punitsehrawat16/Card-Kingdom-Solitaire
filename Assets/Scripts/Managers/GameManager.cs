using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
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

    // other
    private int _remainingCards;
    
    private void OnEnable()
    {
        Card.CardClicked += PlayingCard;
        DeckManager.OnRemainingCard += GetRemainingCards;
    }

    private void OnDisable()
    {
        Card.CardClicked -= PlayingCard;
        DeckManager.OnRemainingCard -= GetRemainingCards;
    }

    public async void OnStartInitialiseTheDeck()
    { 
        GameState.ChangeState(GameStates.Loading);
        await Awaitable.WaitForSecondsAsync(2);
        CreateDeck();
        GameState.ChangeState(GameStates.Menu);
    }
    public async void OnGameStart()          // match start
    {
        GameState.ChangeState(GameStates.Loading);
        ShuffleTheDeck();
        await Awaitable.WaitForSecondsAsync(1);
        GameState.ChangeState(GameStates.Playing);
        await Awaitable.WaitForSecondsAsync(.5f);
        DrawCenterCard();
        DrawPlayerHandCards();
    }

    private void Reset() // for replaying the game or if on start if cards which are drawn to player are not valid to play
    {
        _centerCard = null;
        cardsInHand = null;
        ShuffleTheDeck();
        DrawCenterCard();
        DrawPlayerHandCards();
    }
    private void CreateDeck()
    {
        _deckManager.StartCreatingDeck();
    }

    private void ShuffleTheDeck()
    {
        _deckManager.StartShufflingDeck();
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

        if (CheckGameOver())
        {
            Reset();
        }
    }


    private void PlayingCard(Card card)
    {
        bool isValid = CheckForEligibility(card);
        
        if(!isValid)
            return;
        
        RemoveCardFromPlayerHands(card);
            
        _centerCard.gameObject.SetActive(false);
        _centerCard = card;
        _centerCard.transform.DOMove(_centerCardPos.position, .25f).SetEase(Ease.OutCubic);
        _centerCard.CardInteraction(false);
            
        OnCorrectCard?.Invoke();
        OnGameNotification?.Invoke("+10 Points");
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
        
        GameChecker();                             // check whether game is ended or completed
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
    private bool CheckGameOver()
    {
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            if(cardsInHand[i] == null )
                continue;
            
            if(cardsInHand[i].cardData.Rank > _centerCard.cardData.Rank || cardsInHand[i].cardData.Suits.Equals(_centerCard.cardData.Suits)) 
                return false;
        }

        return true;
    }

    private bool CheckGameWon()
    {
        if(_remainingCards == 0 && cardsInHand == null)
            return true;

        return false;
    }
    private void GameChecker()
    {
        if(CheckGameOver())
            OnGameNotification?.Invoke("Game Over! No more possible moves");

        if(CheckGameWon())
            OnGameNotification?.Invoke("You Won!");
    }
    // fetch cards count from deck
    private void GetRemainingCards(int cards)
    {
        _remainingCards = cards;
    }
    
}
