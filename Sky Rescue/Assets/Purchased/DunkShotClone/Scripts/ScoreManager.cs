using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance { get; private set; }

    public Text ScoreText;
    public Text BestScoreText;

    [HideInInspector]
    public int ScoreValue;

    private int _bestScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ScoreValue = 0;
        _bestScore = PlayerPrefs.GetInt("BestScore");
        UpdateScoreUI();
        UpdateBestScoreUI();
    }

    public void AddScore(int value)
    {
        ScoreValue += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        ScoreText.text = ScoreValue.ToString();
    }

    public void UpdateBestScoreUI()
    {
        if (ScoreValue > _bestScore)
        {
            _bestScore = ScoreValue;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
        BestScoreText.text = "BEST SCORE: " + _bestScore;
    }
}
