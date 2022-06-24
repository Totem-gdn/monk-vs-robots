using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioSource musicAudioSource;

    #region UI_AudioClips

    public AudioClip buttonClickSound;

    #endregion

    #region Music

    public AudioClip menuClip;
    public AudioClip battleClip;
    public AudioClip defeatClip;
    public AudioClip victoryClip;

    #endregion

    private VolumeType volumeType;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetVolume(float value)
    {
        switch(volumeType)
        {
            case VolumeType.UI:
                uiAudioSource.volume = value;
                break;
            case VolumeType.Music:
                musicAudioSource.volume = value;
                break;
        }

        PlayerPrefs.SetFloat(volumeType.ToString(), value);
    }

    public void PlayButtonSound()
    {
        uiAudioSource.clip = buttonClickSound;
        uiAudioSource.Play();
    }

    public void SetMusic(MusicType musicType)
    {
        switch (musicType)
        {
            case MusicType.Menu:
                musicAudioSource.clip = menuClip;
                break;
            case MusicType.Battle:
                musicAudioSource.clip = battleClip;
                break;
            case MusicType.Defeat:
                musicAudioSource.clip = defeatClip;
                break;
            case MusicType.Victory:
                musicAudioSource.clip = victoryClip;
                break;
        }

        if(!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
    }

    //For UI purposes
    public void SetVolumeType(int volumeTypeIndex)
    {
        volumeType = (VolumeType)volumeTypeIndex;
    }
}
