using PlayFab;
using PlayFab.ClientModels;
using System;
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
        if (RoundTime > 0 && !levelManager.canPauseTime)
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
        SaveLevelDataToPlayfab(roundTime, targetScore1, targetScore2, targetScore3);
        levelManager.SetLevelButtonText(levelType);
    }
    private void GetLevelData()
    {
        GetLevelDataFromPlayfab();
    }

    private void SaveLevelDataToPlayfab(float roundTime, int targetScore1, int targetScore2, int targetScore3)
    {
        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            {"RoundTime", roundTime.ToString()},
            {"TargetScore1", targetScore1.ToString()},
            {"TargetScore2", targetScore2.ToString()},
            {"TargetScore3", targetScore3.ToString()}
        }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSendSuccessToPlayfab, OnError);
    }

    private void OnDataSendSuccessToPlayfab(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        Debug.Log("Successfully saved level data to PlayFab!");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error saving data to PlayFab: " + error.GenerateErrorReport());
    }

    private void GetLevelDataFromPlayfab()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(PlayFab.ClientModels.GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("RoundTime"))
        {
            RoundTime = float.Parse(result.Data["RoundTime"].Value);
            roundTime = float.Parse(result.Data["RoundTime"].Value);
            scoreTarget1 = int.Parse(result.Data["TargetScore1"].Value);
            scoreTarget2 = int.Parse(result.Data["TargetScore2"].Value);
            scoreTarget3 = int.Parse(result.Data["TargetScore3"].Value);

            Debug.Log("Successfully retrieved level data from PlayFab!");
        }
        else
        {
            Debug.Log("No data found. Using default values.");
            RoundTime = 60f;
            roundTime = 60f;
            scoreTarget1 = 10;
            scoreTarget2 = 50;
            scoreTarget3 = 100;
        }
    }


}
