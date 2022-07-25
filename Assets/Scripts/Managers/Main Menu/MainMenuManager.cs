using enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject avatarChooserPanel;
    public GameObject mainMenuPanel;
    public GameObject volumeSettingsPanel;

    public Slider uiVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;

    public AssetsChooser assetsChooser;

    [SerializeField] private ElementEnumSceneIdDictionary arenasDictionary;
    
    public TMP_Text VolumeSliderValueText { get; set; }
    public static MainMenuManager Instance { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        if (TotemManager.Instance.userAuthenticated)
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            AuthenticationManager.Instance.LogInPanel.SetActive(true);
        }

        InitializeVolumeSlider(uiVolumeSlider, VolumeType.UI);
        InitializeVolumeSlider(musicVolumeSlider, VolumeType.Music);
        InitializeVolumeSlider(effectsVolumeSlider, VolumeType.Effects);

        uiVolumeSlider.value = InitVolumeSlider(VolumeType.UI.ToString());
        effectsVolumeSlider.value = PlayerPrefs.GetFloat(VolumeType.Effects.ToString());
        musicVolumeSlider.value = InitVolumeSlider(VolumeType.Music.ToString());

        AudioManager.Instance.SetMusic(MusicType.Menu);
    }

    public void OnChooseAvatarClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        mainMenuPanel.SetActive(false);
        avatarChooserPanel.SetActive(true);
    }

    public void OnLogOutClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        assetsChooser.ClearUserData();
        mainMenuPanel.SetActive(false);
        AuthenticationManager.Instance.LogOut();
    }

    public void OnExitClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        Application.Quit();
    }

    public void OnPlayClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        var arenaIndex = arenasDictionary[TotemManager.Instance.currentSpear.element];
        AudioManager.Instance.SetMusic(DefineArenaMusicType());
        UnityEngine.SceneManagement.SceneManager.LoadScene(arenaIndex);
    }

    public void OnAvatarChooserCancelClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        avatarChooserPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ShowHideVolumeSettings(bool isActive)
    {
        AudioManager.Instance?.PlayButtonSound();
        volumeSettingsPanel.SetActive(isActive);
        mainMenuPanel.SetActive(!isActive);
    }

    public void SetVolumeSliderText(float value)
    {
        VolumeSliderValueText.text = Math.Round(value * Constants.VOLUME_RECALCULATION_COEF, 1).ToString();
    }

    public void OnSetDefaultVolumeClick()
    {
        AudioManager.Instance?.PlayButtonSound();
        uiVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
        musicVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
    }

    private void InitializeVolumeSlider(Slider volumeSlider, VolumeType volumeType)
    {
        volumeSlider.onValueChanged.AddListener(delegate { AudioManager.Instance.volumeType = volumeType; });
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
    }

    private float InitVolumeSlider(string volumeTypeKey)
    {
        return PlayerPrefs.HasKey(volumeTypeKey) ?
            PlayerPrefs.GetFloat(volumeTypeKey) : Constants.VOLUME_DEFAULT_VALUE; 
    }

    private MusicType DefineArenaMusicType()
    {
        var currenctElement = TotemManager.Instance.currentSpear.element;

        switch(currenctElement)
        {
            case ElementEnum.Air:
                return MusicType.AirArena;
            case ElementEnum.Earth:
                return MusicType.EarthArena;
            case ElementEnum.Fire:
                return MusicType.FireArena;
            case ElementEnum.Water:
                return MusicType.WaterArena;
            default:
                return MusicType.Menu;
        }
    }
}
