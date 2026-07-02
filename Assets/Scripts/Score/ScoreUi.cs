using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType
{
    Score,
    HighScore
}public class ScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreType _type;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (_text != null)
        {
            if(_type == ScoreType.Score)
                _text.text = ScoreManager.Instance.Score.ToString();
            else 
                _text.text = ScoreManager.Instance.HighScore.ToString();
        }
    }
}
