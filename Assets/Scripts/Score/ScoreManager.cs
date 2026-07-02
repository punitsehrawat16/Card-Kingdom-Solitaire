using System;
using UnityEngine;
using UnityEngine.XR;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    // events for ui update
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHighScoreChanged;
    
    // score values
    private int _score = 0;
    private int _highScore = 0;

    private void Awake()
    {
        Instance = this;
        LoadScoreData();
    }

    private void OnEnable()
    {
        TableManager.OnCorrectCard += ScoreUpdate;
    }
    private void OnDisable()
    {
        TableManager.OnCorrectCard -= ScoreUpdate;
    }
    
    private void Start()
    {
        ResetScore();
        OnScoreChanged?.Invoke(_score);
        OnHighScoreChanged?.Invoke(_highScore);
    }

    public int Score => _score;
    public int HighScore => _highScore;
    
    public void ResetScore()
    {
        _score = 0;
        OnScoreChanged?.Invoke(_score);
    }
    private void ScoreUpdate()
    {
        _score += 10;
        HighScoreUpdate();
        OnScoreChanged?.Invoke(_score);
    }

    private void HighScoreUpdate()
    {
        if (_highScore > _score)
            return;
        
        _highScore = _score;
        SaveScoreData();
        OnHighScoreChanged?.Invoke(_highScore);
    }
    private void SaveScoreData()
    {
        PlayerPrefs.SetInt("HIGHSCORE",_highScore);
    }

    private void LoadScoreData()
    {
        _highScore = PlayerPrefs.GetInt("HIGHSCORE", _highScore);
    }
    public int GetHighScore()
    {
        return _highScore;
    }
}
