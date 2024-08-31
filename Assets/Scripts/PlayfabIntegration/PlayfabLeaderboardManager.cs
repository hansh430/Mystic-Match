using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabLeaderboardManager : MonoBehaviour
{
    public static PlayfabLeaderboardManager Instance;
    [SerializeField] private GameObject leaderBoradPrefab;
    [SerializeField] private Transform leaderBoardParent;
    private List<GameObject> leaderBoardItems=new List<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName="PlatformScore",
                    Value=score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error);
    }

    private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }
    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    private void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        for (int i = leaderBoardItems.Count - 1; i >= 0; i--)
        {
            GameObject item = leaderBoardItems[i];
            if (item != null)
            {
                Destroy(item);
                leaderBoardItems.RemoveAt(i);
            }
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(leaderBoradPrefab, leaderBoardParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            leaderBoardItems.Add(newGo);
        }
    }
}
