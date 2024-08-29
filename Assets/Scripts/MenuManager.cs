using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] private AudioSource commonAudioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
    public void PlayClickSound(AudioClip audioClip)
    {
        commonAudioSource.clip = audioClip;
        commonAudioSource.Play();
    }
    public void Close()
    {
        Application.Quit();
    }
}
