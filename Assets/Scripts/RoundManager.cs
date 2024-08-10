using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public int CurrentScore;

    public float RoundTime = 60f;

    private UIManager uiManager;

    private Board board;

    private bool endingRound = false;

    [SerializeField] private float displayScore;

    [SerializeField] private float scoreSpeed;

    [SerializeField] private int scoreTarget1, scoreTarget2, scoreTarget3;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }
    private void Update()
    {
        Timer();
    }
    private void Timer()
    {
        if (RoundTime > 0)
        {
            RoundTime -= Time.deltaTime;

            if (RoundTime <= 0)
            {
                RoundTime = 0;

                endingRound = true;
            }
        }
        if (endingRound && board.CurrentState == BoardState.move)
        {
            WinCheck();
            endingRound = false;
        }
        displayScore = Mathf.Lerp(displayScore, CurrentScore, scoreSpeed * Time.deltaTime);
        uiManager.UpdateScoreText(displayScore,"DisplayScore");
        uiManager.UpdateTimerText(RoundTime);
    }

    private void WinCheck()
    {
        uiManager.RoundOver();
        uiManager.UpdateScoreText(CurrentScore, "WinScore");
        SetWinType();
    }
    private void SetWinType()
    {
        if (CurrentScore >= scoreTarget3)
        {
            uiManager.WinMessage("Congratulations! You earned 3 stars!",3);
        }
       else if (CurrentScore >= scoreTarget2)
        {
            uiManager.WinMessage("Congratulations! You earned 2 stars!", 2);
        }
       else if (CurrentScore >= scoreTarget1)
        {
            uiManager.WinMessage("Congratulations! You earned 1 stars!", 1);
        }
        else
        {
            uiManager.WinMessage("Oh no! No stars for you! Try again?",0);
        }
    }
}
