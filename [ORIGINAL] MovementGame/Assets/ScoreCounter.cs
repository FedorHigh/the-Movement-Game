using System;
using TMPro;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class ScoreCounter : MonoBehaviour, ISaveable
{
    public TextMeshProUGUI text;
    public int score = 0;
    void Start() { 
        text = GetComponent<TextMeshProUGUI>();
    }
    public void SetScore(int score_) { 
        score = score_;
        text.text = score.ToString();
    }
    public void AddScore(int add) { 
        SetScore(score+add);
    }
    public void LoadData(GameData data)
    {
        Debug.Log("Loaded: " + data.score);
        SetScore(data.score);
    }

    public void SaveData(ref GameData data)
    {
        data.score = score;
    }
}
