using Opsive.UltimateCharacterController.SurfaceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SurfaceEffect> surfaceEffects;

    public AudioSource uiAudioSource;
    public AudioSource musicAudioSource;

    #region UI_AudioClips

    public AudioClip buttonClickSound;

    #endregion

    #region Music

    public AudioClip menuClip;
    public AudioClip fireArenaClip;
    public AudioClip waterArenaClip;
    public AudioClip airArenaClip;
    public AudioClip earthArenaClip;

    #endregion

    public VolumeType volumeType;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
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
            case VolumeType.Effects:
                foreach(var effect in surfaceEffects)
                {
                    effect.MinAudioVolume = value;
                    effect.MaxAudioVolume = value;
                }
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
            case MusicType.FireArena:
                musicAudioSource.clip = fireArenaClip;
                break;
            case MusicType.WaterArena:
                musicAudioSource.clip = waterArenaClip;
                break;
            case MusicType.EarthArena:
                musicAudioSource.clip = earthArenaClip;
                break;
            case MusicType.AirArena:
                musicAudioSource.clip = airArenaClip;
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
}
