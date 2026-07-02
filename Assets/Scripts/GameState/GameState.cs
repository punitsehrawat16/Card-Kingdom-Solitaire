using System;
using UnityEngine;

public static class GameState
{ 
    public static event Action<GameStates> OnGameStateChanged;
    public static GameStates currentState;
    public static void ChangeState(GameStates state)
    {
        currentState = state;
        OnGameStateChanged?.Invoke(state);
    }
}

public enum GameStates
{
    Menu, Loading, Playing, Pause, GameOver, PlayComplete
}
