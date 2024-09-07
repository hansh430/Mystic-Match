using DG.Tweening;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winScoreText;
    [SerializeField] private TMP_Text winMessageText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject roundOverPanel;
    [SerializeField] private GameObject winStars0, winStars1, winStars2, winStars3;
    [SerializeField] private Material[] skyboxMaterials;
    private void Start()
    {
        SetSkybox();
    }
  
    private void SetSkybox()
    {
        if (skyboxMaterials.Length > 0)
        {
            int index = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[index];
        }
    }

    public void UpdateScoreText(float Score, string scoreType)
    {
        if(scoreType=="DisplayScore")
            scoreText.text = Score.ToString("0");
        else if(scoreType == "WinScore")
            winScoreText.text = Score.ToString("0");
    }
    public void UpdateTimerText(float roundTime)
    {
        timerText.text = roundTime.ToString("0") + " s";
    }
    public void RoundOver()
    {
        MakePanelEnable(roundOverPanel);
        AudioManager.Instance.PlayRoundOver();
    }
    public void MakePanelEnable(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.DOScale(1f, 0.5f);
    }
    public void MakePanelDisable(GameObject panel)
    {
        panel.transform.DOScale(0f, 0.5f).onComplete = () => { panel.SetActive(false); };
    }
    public void WinMessage(string message,int noOfStars)
    {
        winMessageText.text= message;
        switch (noOfStars)
        {
            case 0:
                winStars0.SetActive(true);
                break;
            case 1:
                winStars1.SetActive(true);
                break;
            case 2:
                winStars2.SetActive(true);
                break;
            case 3:
                winStars3.SetActive(true);
                break;
        }
    }
    public void OnClickLogOutButton()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadSceneAsync(0);
    }
}
