using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // events for ui update
    public static event Action<int> OnScoreIncreased;
    public static event Action<int> OnHighScoreIncreased;
    
    // score values
    private int _score = 0;
    private int _highScore = 0;
    
    
    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("HIGHSCORE", _highScore);
    }
    
    private void OnEnable()
    {
        GameManager.OnCorrectCard += ScoreUpdate;
    }

    private void OnDisable()
    {
        GameManager.OnCorrectCard -= ScoreUpdate;
    }

    private void Start()
    {
        OnScoreIncreased?.Invoke(_score);
        OnHighScoreIncreased?.Invoke(_highScore);
    }

    private void ScoreUpdate()
    {
        _score += 10;
        HighScore();
        OnScoreIncreased?.Invoke(_score);
    }

    private void HighScore()
    {
        if (_highScore > _score)
            return;
        
        _highScore = _score;
        PlayerPrefs.SetInt("HIGHSCORE",_highScore);
        OnHighScoreIncreased?.Invoke(_highScore);
    }
}
