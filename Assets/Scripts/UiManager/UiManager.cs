using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    
    [Header("Score Visuals")] 
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _highScore;
    
    [Header("Cards In Deck")] 
    [SerializeField] private TextMeshProUGUI _remainCards;

    [Header("Game Texts")] 
    [SerializeField] private TextMeshProUGUI _error;
    [SerializeField] private TextMeshProUGUI _gameMessage;
    [SerializeField] private float _durationOfMessage;
    
    // other 
    private Coroutine _errorRoutine;
    private Coroutine _gameRoutine;
    
    
    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += ScoreUi;
        ScoreManager.OnHighScoreChanged += HighScoreUi;
        DeckManager.OnRemainingCard += CardsInDeckUi;
        TableManager.OnTableNotification += TableMessage;
        GameRules.OnGameRulesNotification += GameMessage;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= ScoreUi;
        ScoreManager.OnHighScoreChanged -= HighScoreUi;
        DeckManager.OnRemainingCard -= CardsInDeckUi;
        TableManager.OnTableNotification -= TableMessage;
        GameRules.OnGameRulesNotification += GameMessage;

        if (_errorRoutine != null)
            StopCoroutine(_errorRoutine);
        if(_gameRoutine != null)
            StopCoroutine(_gameRoutine);
    }

    private void ScoreUi(int sc) => _score.text = sc.ToString();

    private void HighScoreUi(int highsc) => _highScore.text = highsc.ToString();

    private void CardsInDeckUi(int cards) => _remainCards.text = cards.ToString();

    private void GameMessage(string message)
    {
        if (_gameRoutine != null)
            StopCoroutine(_gameRoutine);

        _gameRoutine = StartCoroutine(GameText(message));
    }

    private IEnumerator GameText(string message)
    {
        _gameMessage.text = message;
        yield return new WaitForSeconds(_durationOfMessage);
        _gameMessage.text = null;
    }
    private void TableMessage(string message)
    {
        if (_errorRoutine != null)
            StopCoroutine(_errorRoutine);

        _errorRoutine = StartCoroutine(ErrorText(message));
    }
    private IEnumerator ErrorText(string message)
    {
        _error.text = message;
        yield return new WaitForSeconds(_durationOfMessage);
        _error.text = " ";
    }
}
