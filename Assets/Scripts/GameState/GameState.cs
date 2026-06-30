using System;
using UnityEngine;

public static class GameState
{ 
    public static event Action<GameStates> OnGameStateChanged;

    public static void ChangeState(GameStates state)
    {
        OnGameStateChanged?.Invoke(state);
    }
}

public enum GameStates
{
    Menu, Loading, Playing, Pause, GameOver, PlayComplete
}
