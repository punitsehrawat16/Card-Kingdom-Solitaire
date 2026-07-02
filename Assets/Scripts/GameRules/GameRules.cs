using UnityEngine;
using System;
public class GameRules : MonoBehaviour
{
    //events
    public static event Action<string> OnGameRulesNotification;

    [SerializeField] private TableManager _tableManager;
    // other
    private int _remainingCards;
    private void OnEnable()
    {
        DeckManager.OnRemainingCard += GetRemainingCards;
    }

    private void OnDisable()
    {
        DeckManager.OnRemainingCard -= GetRemainingCards;
    }
    
    // game rule
    public bool CheckForEligibility(Card card)
    {
        if (_tableManager.CenterCard.cardData.Rank < card.cardData.Rank ||
            _tableManager.CenterCard.cardData.Suits == card.cardData.Suits)
        {
            OnGameRulesNotification?.Invoke("+10 Points");
            return true;
        }
        
        OnGameRulesNotification?.Invoke("Play a card with the same suit or a higher rank.");
        return false;
    }
    private void GetRemainingCards(int cards)
    {
        _remainingCards = cards;
    }
    private bool CheckGameOver()
    {
        if (!IsHandFull() && _remainingCards > 0)
            return false;
        
        for (int i = 0; i < _tableManager.HandCards.Length; i++)
        {
            if(_tableManager.HandCards[i] == null)
                continue;
            
            if(_tableManager.HandCards[i].cardData.Rank > _tableManager.CenterCard.cardData.Rank || 
               _tableManager.HandCards[i].cardData.Suits.Equals(_tableManager.CenterCard.cardData.Suits)) 
                return false;
        }

        return true;
    }

    private bool IsHandFull()
    {
        for (int i = 0; i < _tableManager.HandCards.Length; i++)
        {
            if (_tableManager.HandCards[i] == null)
                return false;
        }

        return true;
    }
    private bool CheckGameWon()
    {
        return IsHandEmpty() && _remainingCards <= 0;
    }

    private bool IsHandEmpty()
    {
        foreach (var card in _tableManager.HandCards )
        {
            if (card != null)
                return false;
        }
        return true;
    }
    // game checker: win/loss
    public async void GameChecker()
    {
        if (CheckGameWon())
        {
            OnGameRulesNotification?.Invoke("You Won!");
            GameState.ChangeState(GameStates.PlayComplete);
        }
        else if (CheckGameOver())
        {
            OnGameRulesNotification?.Invoke("Game Over! No more possible moves");
            await Awaitable.WaitForSecondsAsync(2f);
            GameState.ChangeState(GameStates.GameOver);
        }

    }
}
