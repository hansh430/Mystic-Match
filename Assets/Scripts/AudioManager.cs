using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]private AudioSource tileSoundAudioSource, explodeSoundAudioSource, stoneSoundAudioSource, roundOverSoundAudioSource,bgMusicAudioSource,commmonAudioSource;
    private void Awake()
    {
        Instance = this;
    }
    public void PlayAudio(AudioSourceType audioSourceType)
    {
        AudioSource currentAudioSource=tileSoundAudioSource;
        switch (audioSourceType)
        {
            case AudioSourceType.TileSoundAudioSource:
                currentAudioSource= tileSoundAudioSource;
                break;
            case AudioSourceType.ExplodeSoundAudioSource:
                currentAudioSource= explodeSoundAudioSource;
                break;
            case AudioSourceType.StoneSoundAudioSource:
                currentAudioSource= stoneSoundAudioSource;
                break;
        }

        currentAudioSource.Stop();

        currentAudioSource.pitch = Random.Range(.8f, 1.2f);

        currentAudioSource.Play();
    }
    public void PlayRoundOver()
    {
        roundOverSoundAudioSource.Play();
    }
    public void PlayClickSound(AudioClip audioClip)
    {
        commmonAudioSource.clip = audioClip;
        commmonAudioSource.Play();
    }
    public void Mute()
    {
        tileSoundAudioSource.mute = true;
        explodeSoundAudioSource.mute = true;
        stoneSoundAudioSource.mute = true;
        roundOverSoundAudioSource.mute = true;
        bgMusicAudioSource.mute = true;
    }
    public void Unmute()
    {
        tileSoundAudioSource.mute = false;
        explodeSoundAudioSource.mute = false;
        stoneSoundAudioSource.mute = false;
        roundOverSoundAudioSource.mute = false;
        bgMusicAudioSource.mute = false;
    }
}
public enum AudioSourceType
{
    TileSoundAudioSource,
    ExplodeSoundAudioSource,
    StoneSoundAudioSource
}
