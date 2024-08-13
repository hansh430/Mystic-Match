using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelButtonText;
    [SerializeField] private TMP_Text levelText;
    private int levelCount = 1;

    private void Awake()
    {
        levelText.text = "Level " + PlayerPrefs.GetInt("Level", 1);
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

}
