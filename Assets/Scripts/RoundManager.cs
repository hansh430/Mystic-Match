using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
   // private UIManager uiMan;

    private bool endingRound = false;

    public int CurrentScore;
    public float DisplayScore;
    public float ScoreSpeed;

    public int ScoreTarget1, ScoreTarget2, ScoreTarget3;
}
