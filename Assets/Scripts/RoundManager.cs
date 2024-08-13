using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public int CurrentScore;

    public float RoundTime;

    private float roundTime;

    private UIManager uiManager;

    private Board board;

    private LevelManager levelManager;

    private bool endingRound = false;

    [SerializeField] private float displayScore;

    [SerializeField] private float scoreSpeed;

    [SerializeField] private int scoreTarget1, scoreTarget2, scoreTarget3;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
        levelManager = FindObjectOfType<LevelManager>();
        GetLevelData();
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
            SaveLevelData(roundTime+30, scoreTarget1 + 20, scoreTarget2 + 60, scoreTarget3 + 100, "Next");
        }
       else if (CurrentScore >= scoreTarget2)
        {
            uiManager.WinMessage("Congratulations! You earned 2 stars!", 2);
            SaveLevelData(roundTime, scoreTarget1, scoreTarget2, scoreTarget3, "Retry");
        }
        else if (CurrentScore >= scoreTarget1)
        {
            uiManager.WinMessage("Congratulations! You earned 1 stars!", 1);
            SaveLevelData(roundTime, scoreTarget1, scoreTarget2, scoreTarget3, "Retry");
        }
        else
        {
            uiManager.WinMessage("Oh no! No stars for you! Try again?",0);
            SaveLevelData(roundTime, scoreTarget1, scoreTarget2, scoreTarget3, "Retry");
        }
    }
    private void SaveLevelData(float roundTime,int targetScore1, int targetScore2, int targetScore3, string levelType)
    {
        PlayerPrefs.SetFloat("RoundTime", roundTime);
        PlayerPrefs.SetInt("TargetScore1", targetScore1);
        PlayerPrefs.SetInt("TargetScore2", targetScore2);
        PlayerPrefs.SetInt("TargetScore3", targetScore3);
        levelManager.SetLevelButtonText(levelType);
        PlayerPrefs.Save();
    }
    private void GetLevelData()
    {
        RoundTime= PlayerPrefs.GetFloat("RoundTime", 60);
        roundTime= PlayerPrefs.GetFloat("RoundTime", 60);
        scoreTarget1 = PlayerPrefs.GetInt("TargetScore1", 10);
        scoreTarget2 = PlayerPrefs.GetInt("TargetScore2", 50);
        scoreTarget3 = PlayerPrefs.GetInt("TargetScore3", 100);
       
    }
}
