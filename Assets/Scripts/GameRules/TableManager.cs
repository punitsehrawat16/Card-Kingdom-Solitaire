using UnityEngine;
using System;
using DG.Tweening;

public class TableManager : MonoBehaviour
{
    // events
    public static event Action<string> OnTableNotification;  // for player's awareness,if player makes moves which aren't valid
    public static event Action OnCorrectCard;    // for score manager, increment score
    
    
    [Header("Center Card")] 
    [SerializeField] private Card _centerCard;
    [SerializeField] private Transform _centerCardPos;
    
    [Header("References")] 
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private GameRules _gameRules;
    
    [Header("Player Card")]
    [SerializeField] private Transform[] _playerCardSlot = new Transform[5];
    [SerializeField] private Card[] cardsInHand = new Card[5];
    
    // other
    private int _remainingCards;
    
    public Card CenterCard => _centerCard;
    public Card[] HandCards => cardsInHand;
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

    
    public void  OnGameStart()          // match start
    {
        ShuffleTheDeck();
        DrawCenterCard();
        DrawPlayerHandCards();
    }

    public void PreparingNewMatch() // for replaying the game or if on start if cards which are drawn to player are not valid to play
    {
        _deckManager.ResetDeckData();
        _centerCard = null;
        //await Awaitable.NextFrameAsync();
        _deckManager.StartShufflingDeck();
       // await Awaitable.NextFrameAsync();
        DrawCenterCard();
        DrawPlayerHandCards();
    }
    

    private void ShuffleTheDeck()
    {
        _deckManager.StartShufflingDeck();
    }
    // fetch cards count from deck
    private void GetRemainingCards(int cards)
    {
        _remainingCards = cards;
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
        bool isValid = _gameRules.CheckForEligibility(card);
        
        if(!isValid)
            return;
        
        RemoveCardFromPlayerHands(card);
            
        _centerCard.gameObject.SetActive(false);
        _centerCard = card;
        _centerCard.transform.DOMove(_centerCardPos.position, .25f).SetEase(Ease.OutCubic);
        _centerCard.CardInteraction(false);
            
        OnCorrectCard?.Invoke();
        
        
        _gameRules.GameChecker();
    }
    public void GetNewCard()
    {
        DrawPlayerNewCard();
    }private void DrawPlayerNewCard()
    {
        if (_remainingCards <= 0)
        {
            OnTableNotification?.Invoke("All cards are drawn");
            return;
        }
        int emptySlot = FindEmptySlot(); // checks that player can draw another card & find empty slot
        if (emptySlot == -1)
        {
            OnTableNotification?.Invoke("Hand is full. Play a valid card before drawing another.");
            return;
        }
        
        Card card = _deckManager.DrawTopCard(_playerCardSlot[emptySlot]);
        cardsInHand[emptySlot] = card;
        card.CardInteraction(true);
        
        _gameRules.GameChecker();                             // check whether game is ended or completed
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
}
