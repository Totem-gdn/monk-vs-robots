using Opsive.UltimateCharacterController.Camera;
using System;
using UnityEngine;
using EventHandler = Opsive.Shared.Events.EventHandler;
using OpsiveAudioManager = Opsive.Shared.Audio.AudioManager;
using Opsive.Shared.Audio;
using TotemEnums;

public class GameManager : MonoBehaviour
{
    public CharacterTypeGameObjectDictionary maleAvatars;
    public CharacterTypeGameObjectDictionary femaleAvatars;
    public CameraController cameraController;
    public RespawnManager respawnManager;

    [SerializeField] private Transform spawnPoint;

    private GameObject character;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        SetOpsiveAudioManagerVolume();

        DefinePlayerAvatar();
        respawnManager.characterObject = character;
        ReturnCharacterToSpawn();
        character.SetActive(true);
        cameraController.Character = character;
        cameraController.enabled = true;

        EventHandler.RegisterEvent("SetVolume", SetOpsiveAudioManagerVolume);
        EventHandler.RegisterEvent("GameRestarted", ReturnCharacterToSpawn);
    }

    private void DefinePlayerAvatar()
    {
        var choosedAvatar = TotemManager.Instance.currentAvatar;
        var characterType = (CharacterType)Enum.Parse(typeof(CharacterType),
            choosedAvatar.bodyFat.ToString() + choosedAvatar.bodyMuscles.ToString());

        switch (choosedAvatar.sex)
        {
            case SexEnum.Male:
                character = maleAvatars[characterType];
                break;
            case SexEnum.Female:
                character = femaleAvatars[characterType];
                break;
        }
    }

    private void ReturnCharacterToSpawn()
    {
        character.transform.position = spawnPoint.position;
    }

    private void SetOpsiveAudioManagerVolume()
    {
        var AudioManagerModule = OpsiveAudioManager.GetAudioManagerModule();
        var effectsVolumeKey = VolumeType.Effects.ToString();
        var effectsVolume = PlayerPrefs.HasKey(effectsVolumeKey) ?
            PlayerPrefs.GetFloat(effectsVolumeKey) : Constants.VOLUME_DEFAULT_VALUE;

        AudioManagerModule.DefaultAudioConfig.AudioModifier = new AudioModifier { VolumeOverride = new FloatOverride(FloatOverride.Override.Constant, effectsVolume) };
        OpsiveAudioManager.SetAudioManagerModule(AudioManagerModule);
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent("GameRestarted", ReturnCharacterToSpawn);
        EventHandler.UnregisterEvent("SetVolume", SetOpsiveAudioManagerVolume);
    }
}
