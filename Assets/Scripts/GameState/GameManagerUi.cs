using System;
using UnityEngine;

public class GameManagerUi : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameManager _gameManager;
    
    [Header("Game State Panels")] 
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _gameOverPanel;

    private void OnEnable()
    {
        GameState.OnGameStateChanged += HandleState;
    }

    private void OnDisable()
    {
        GameState.OnGameStateChanged -= HandleState;
    }

    private void Start()   
    { 
        OnStart();
    }

    private void OnStart()
    {
        _gameManager.OnStartInitialiseTheDeck();
    }

    public void OnGameStart() // from menu to match start
    {
        _gameManager.OnGameStart();
    }

    private void HandleState(GameStates states)
    {
        switch (states)
        {
            case GameStates.Loading:
                HandlePanels();
                _loadingPanel.SetActive(true);
                break;
            case  GameStates.Menu:
                HandlePanels();
                _menuPanel.SetActive(true);
                break;
            case GameStates.Playing:
                HandlePanels();
                _gamePanel.SetActive(true);
                break;
            case GameStates.Pause:
                HandlePanels();
                _pausePanel.SetActive(true);
                break;
            case GameStates.GameOver:
                HandlePanels();
                _gameOverPanel.SetActive(true);
                break;
            case GameStates.PlayComplete:
                HandlePanels();
                _winPanel.SetActive(true);
                break;
        }
    }

    private void HandlePanels()
    {
        _menuPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _loadingPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _winPanel.SetActive(false);
    }
}
