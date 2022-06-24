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

    private AuthenticationManager authenticationManager;
    
    public TMP_Text VolumeSliderValueText { get; set; }

    void Awake()
    {
        authenticationManager = GetComponent<AuthenticationManager>();
        if (TotemManager.Instance.currentUser == null)
        {
            authenticationManager.logInPanel.SetActive(true);
        }
        else
        {
            mainMenuPanel.SetActive(true);
        }

        uiVolumeSlider.value = InitVolumeSlider(VolumeType.UI.ToString());
        //effectsVolumeSlider.value = PlayerPrefs.GetFloat(VolumeType.Effects.ToString());
        musicVolumeSlider.value = InitVolumeSlider(VolumeType.Music.ToString());

        AudioManager.Instance.SetMusic(MusicType.Menu);
    }

    public void OnChooseAvatarClick()
    {
        mainMenuPanel.SetActive(false);
        avatarChooserPanel.SetActive(true);
    }

    public void OnLogOutClick()
    {
        assetsChooser.ClearUserData();
        mainMenuPanel.SetActive(false);
        authenticationManager.LogOut();
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnPlayClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnAvatarChooserCancelClick()
    {
        avatarChooserPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ShowHideVolumeSettings(bool isActive)
    {
        volumeSettingsPanel.SetActive(isActive);
        mainMenuPanel.SetActive(!isActive);
    }

    public void SetVolumeSliderText(float value)
    {
        VolumeSliderValueText.text = Math.Round(value * Constants.VOLUME_RECALCULATION_COEF, 1).ToString();
    }

    public void OnSetDefaultVolumeClick()
    {
        uiVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
        musicVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
    }

    private float InitVolumeSlider(string volumeTypeKey)
    {
        return PlayerPrefs.HasKey(volumeTypeKey) ?
            PlayerPrefs.GetFloat(volumeTypeKey) : Constants.VOLUME_DEFAULT_VALUE; 
    }
}
