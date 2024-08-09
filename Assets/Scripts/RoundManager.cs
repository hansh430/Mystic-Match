using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int CurrentScore;

    private UIManager uiManager;

    private bool endingRound = false;

    [SerializeField] private float roundTime = 60f;

    [SerializeField] private float DisplayScore;

    [SerializeField] private float ScoreSpeed;

    [SerializeField] private int scoreTarget1, scoreTarget2, scoreTarget3;
}
