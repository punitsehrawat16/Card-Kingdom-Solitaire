using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private TableManager _tableManager;
    [SerializeField] private GameRules _gameRules;

    public async void OnStartInitialiseTheDeck()
    { 
        GameState.ChangeState(GameStates.Loading);
        _deckManager.StartCreatingDeck();
        await Awaitable.WaitForSecondsAsync(1f);
        GameState.ChangeState(GameStates.Menu);
    }
    // ui-manager will call these functions directly, ui manager func called from button directly

    public void PlayFirstMatch()
    {
        GameState.ChangeState(GameStates.Loading);
        _tableManager.OnGameStart();
        GameState.ChangeState(GameStates.Playing);
    }
    
    public void GamePause()
    {
        GameState.ChangeState(GameStates.Pause);
    }

    public void GameResume()
    {
        GameState.ChangeState(GameStates.Playing);
    }

    public void GameReplay()
    {
        GameState.ChangeState(GameStates.Loading);
        _tableManager.PreparingNewMatch();
        ScoreManager.Instance.ResetScore();
        GameState.ChangeState(GameStates.Playing);
    }

    public void GameMenu()
    {
        GameState.ChangeState(GameStates.Menu);
    }

    public void CheckGameOver()
    {
        _gameRules.GameChecker();
    }
}