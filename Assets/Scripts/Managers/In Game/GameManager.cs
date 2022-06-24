using enums;
using Opsive.UltimateCharacterController.Camera;
using Opsive.UltimateCharacterController.Character;
using System;
using System.Collections;
using UnityEngine;
using Opsive.Shared.Events;
using EventHandler = Opsive.Shared.Events.EventHandler;

public class GameManager : MonoBehaviour
{
    public CharacterTypeGameObjectDictionary maleAvatars;
    public CharacterTypeGameObjectDictionary femaleAvatars;
    public CameraController cameraController;
    public RespawnManager respawnManager;

    [SerializeField] private Transform spawnPoint;

    private GameObject character;
    private UltimateCharacterLocomotion ultimateCharacterLocomotion;
    private bool isTestStopProcessing = false;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }

        EventHandler.RegisterEvent("GameRestarted", ReturnCharacterToSpawn);

        DefinePlayerAvatar();
        ultimateCharacterLocomotion = character.GetComponent<UltimateCharacterLocomotion>();
        respawnManager.characterObject = character;
        ReturnCharacterToSpawn();
        character.SetActive(true);
        cameraController.Character = character;
        cameraController.enabled = true;

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

    //Test purposes
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnExit();
        }
    }

    //Test purposes
    private void OnExit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void ReturnCharacterToSpawn()
    {
        character.transform.position = spawnPoint.position;
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent("GameRestarted", ReturnCharacterToSpawn);
    }
}
