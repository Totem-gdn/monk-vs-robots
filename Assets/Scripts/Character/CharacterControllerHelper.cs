using enums;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Traits;
using System;
using UnityEngine;
using EventHandler = Opsive.Shared.Events.EventHandler;

public class CharacterControllerHelper : MonoBehaviour
{
    public SpearWeapon charactersSpearWeapon;
    public DamageProcessor characterDamageProcessor;
    public AttributeManager attributeManager;
    public Health characterHealth;
    public UltimateCharacterLocomotion characterLocomotion;
    public HairStyleEnumGameObjectDictionary hairStyles;
    public Collider playerCollider;

    public Material skinMaterial;
    public Material eyesMaterial;
    public Material hairMaterial;

    private HairStyleEnum hairStyle;
    private CharacterType characterType;
    private GameEndedAbility gameEndedAbility;
    private bool isGameEnded = false;
    private GameEndedType gameEndedType;

    public GameObject Character { get; private set; }
    public static CharacterControllerHelper Instance { get; private set; }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Character = gameObject;
        InitializeAvatar();

        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }

    public void CheckIsGameEnded()
    {
        if (isGameEnded)
        {
            OnGameEnded(gameEndedType);
        }
    }

    private void InitializeAvatar()
    {
        var choosedAvatar = TotemManager.Instance.currentAvatar;

        skinMaterial.color = choosedAvatar.skinColor;
        hairMaterial.color = choosedAvatar.hairColor;
        eyesMaterial.color = choosedAvatar.eyeColor;
        hairStyle = choosedAvatar.hairStyle;
        SwitchHairStyle(hairStyle);

        characterType = (CharacterType) Enum.Parse(typeof(CharacterType),
            choosedAvatar.bodyFat.ToString() + choosedAvatar.bodyMuscles.ToString());
        DefineCharacterAbilities(characterType);
    }

    private void DefineCharacterAbilities(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.FatMuscular:
                charactersSpearWeapon.criticalDamageChance = Constants.CRIT_DAMAGE_CHANCE;
                break;
            case CharacterType.FatWimp:
                var healthAttribute = attributeManager.GetAttribute(Constants.HEALTH_ATTRIBUTE_NAME);
                healthAttribute.MaxValue *= Constants.MAX_HEALTH_MULTIPLIER;
                healthAttribute.Value = healthAttribute.MaxValue;
                break;
            case CharacterType.ThinMuscular:
                characterLocomotion.RootMotionSpeedMultiplier = Constants.CHARACTER_SPEED_VALUE;
                break;
            case CharacterType.ThinWimp:
                characterDamageProcessor.dodgeChance = Constants.DODGE_CHANCE;
                break;
        }
    }

    private void SwitchHairStyle(HairStyleEnum hairStyleToSet)
    {
        hairStyles[hairStyle].SetActive(false);
        hairStyles[hairStyleToSet].SetActive(true);
        hairStyle = hairStyleToSet;
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        //To Do: use gameEndedType to define Win or Lose ability
        isGameEnded = true;
        this.gameEndedType = gameEndedType;
        GetGameEndedAbility();
        foreach(var ability in characterLocomotion.ActiveAbilities)
        {
            if (ability != null)
            {
                ability.StopAbility();
            }
        }
        characterLocomotion.TryStartAbility(gameEndedAbility);
    }

    private void OnGameRestarted()
    {
        isGameEnded = false;
        GetGameEndedAbility();
        characterLocomotion.TryStopAbility(gameEndedAbility, true);
        characterHealth.Heal(attributeManager.GetAttribute(Constants.HEALTH_ATTRIBUTE_NAME).MaxValue);
        //To Do: stop the action of losing\winning animation
    }

    private void GetGameEndedAbility()
    {
        if (gameEndedAbility == null)
        {
            gameEndedAbility = characterLocomotion.GetAbility<GameEndedAbility>();
        }
    }

    private void OnDisable()
    {
        Instance = null;
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
        EventHandler.UnregisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }
}
