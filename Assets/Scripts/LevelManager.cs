using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelButtonText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text oneStarScore;
    [SerializeField] private TMP_Text twoStarScore;
    [SerializeField] private TMP_Text threeStarScore;
    private int levelCount;

    private void Awake()
    {
        GetUserData();
    }
    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(), OnLevelDataReceived, OnError);
    }
    private void OnLevelDataReceived(PlayFab.ClientModels.GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("Level"))
        {
            levelCount = int.Parse(result.Data["Level"].Value);
        }
        else
        {
            levelCount = 1;
        }

        levelText.text = "Level " + levelCount;
        SetScorePanel(result);
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving data from PlayFab: " + error.GenerateErrorReport());
    }
    public void SetLevelButtonText(string level)
    {
        levelCount++;

        levelButtonText.text = level;

        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "Level", levelCount.ToString() }
        }
        };

        PlayFabClientAPI.UpdateUserData(request, OnLevelDataSaved, OnError);
    }
    private void SetScorePanel(PlayFab.ClientModels.GetUserDataResult result)
    {
        oneStarScore.text = result.Data != null && result.Data.ContainsKey("TargetScore1")
                            ? result.Data["TargetScore1"].Value
                            : "10";

        twoStarScore.text = result.Data != null && result.Data.ContainsKey("TargetScore2")
                            ? result.Data["TargetScore2"].Value
                            : "50";

        threeStarScore.text = result.Data != null && result.Data.ContainsKey("TargetScore3")
                            ? result.Data["TargetScore3"].Value
                            : "100";
    }

    private void OnLevelDataSaved(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        Debug.Log("Level data saved to PlayFab successfully.");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
