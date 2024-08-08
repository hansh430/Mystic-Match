using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]private AudioSource tileSoundAudioSource, explodeSoundAudioSource, stoneSoundAudioSource, roundOverSoundAudioSource;
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
}
public enum AudioSourceType
{
    TileSoundAudioSource,
    ExplodeSoundAudioSource,
    StoneSoundAudioSource
}
