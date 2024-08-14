using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        levelCount = PlayerPrefs.GetInt("Level", 1);
        levelText.text = "Level " + levelCount;
        SetScorePanel();
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SetLevelButtonText(string level)
    {
        levelCount++;
        levelButtonText.text = level;
        PlayerPrefs.SetInt("Level", levelCount);
        PlayerPrefs.Save();
    }
    private void SetScorePanel()
    {
        oneStarScore.text = PlayerPrefs.GetInt("TargetScore1", 10).ToString();
        twoStarScore.text = PlayerPrefs.GetInt("TargetScore2", 50).ToString();
        threeStarScore.text = PlayerPrefs.GetInt("TargetScore3", 100).ToString();
    }
}
