using Opsive.Shared.Audio;
using Opsive.Shared.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Opsive.Shared.Events.EventHandler;

[Serializable]
public struct AudioClipInfo
{
    public AudioClip clip;
    [Range(0,1)]
    public float defaultVolume;
}

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool loop;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundTypeEnumAudioClipInfoDictionary soundEffects;

    private float defaultVolume;

    private void Awake()
    {
        defaultVolume = audioSource.volume;

        SetSoundsVolume();
        if(playOnAwake)
        {
            PlayAudioClip(SoundType.Default, loop);
        }

        EventHandler.RegisterEvent("SetVolume", SetSoundsVolume);
    }

    private void SetSoundsVolume()
    {
        var effectsVolumeKey = VolumeType.Effects.ToString();
        var effectsVolume = PlayerPrefs.HasKey(effectsVolumeKey) ?
            PlayerPrefs.GetFloat(effectsVolumeKey) : Constants.VOLUME_DEFAULT_VALUE;

        audioSource.volume = defaultVolume * effectsVolume;
    }

    public void PlayAudioClip(SoundType soundToPlay, bool isLooping = false)
    {
        if (soundEffects.ContainsKey(soundToPlay))
        {
            var soundEffect = soundEffects[soundToPlay];
            audioSource.loop = isLooping;

            if (isLooping)
            {
                audioSource.clip = soundEffect.clip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(soundEffect.clip, soundEffect.defaultVolume);
            }
        }
    }

    public void StopAudioSource()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
