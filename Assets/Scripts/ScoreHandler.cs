using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public sealed class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler Instance { get; private set; }
    private int score;
    public int Score
    {
        get=>score;
        set
        {
            if(score == value) return;
            score = value;
            scoreText.SetText($"Score: {score}");
        }
    }
    [SerializeField] private TMP_Text scoreText;
    private void Awake() => Instance = this;

}
