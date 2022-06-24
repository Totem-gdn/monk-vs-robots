using Opsive.Shared.Input;
using Opsive.UltimateCharacterController.Camera;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultStartType(AbilityStartType.Manual)]
[DefaultStopType(AbilityStopType.Manual)]
public class GameEndedAbility : Ability
{
    [SerializeField] private CameraController characterCameraController;
    [SerializeField] private CameraControllerHandler characterCameraControllerHandler;
    private UnityInput characterInput;
    private CharacterIK characterIK;
    private UltimateCharacterLocomotionHandler ultimateCharacterLocomotionHandler;

    public override void Awake()
    {
        characterInput = m_CharacterLocomotion.GetComponent<UnityInput>();
        characterIK = m_CharacterLocomotion.GetComponent<CharacterIK>();
        ultimateCharacterLocomotionHandler = m_CharacterLocomotion.GetComponent<UltimateCharacterLocomotionHandler>();
        base.Awake();
    }

    protected override void AbilityStarted()
    {
        RestrictUnrestrictPlayerActions(false);
        base.AbilityStarted();
    }

    protected override void AbilityStopped(bool force)
    {
        RestrictUnrestrictPlayerActions(true);
        base.AbilityStopped(force);
    }

    private void RestrictUnrestrictPlayerActions(bool isUnrestricted)
    {
        characterInput.enabled = isUnrestricted;
        characterIK.enabled = isUnrestricted;
        ultimateCharacterLocomotionHandler.enabled = isUnrestricted;
        characterCameraControllerHandler.enabled = isUnrestricted;
        characterCameraController.TryZoom(isUnrestricted);
    }
}
