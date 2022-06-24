using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.UltimateCharacterController;
using Opsive.UltimateCharacterController.Traits;

public class PlayerHpTransition : BaseTransition
{
    [SerializeField] private float requiredHp;
    [SerializeField] private HpComparerTransitionType hpComparerType;
    private Health currentHpController;

    public override (bool, BaseState) CheckTransitionRequirements()
    {
        switch (hpComparerType)
        {
            case HpComparerTransitionType.Above:
                if (currentHpController.HealthValue > requiredHp)
                {
                    stateMachine.currentTargetTransform = CharacterControllerHelper.Instance.Character.transform;
                    stateMachine.currentTargetCollider = CharacterControllerHelper.Instance.playerCollider;
                    return (true, stateToTransit);
                }
                break;
            case HpComparerTransitionType.Equals:
                if (currentHpController.HealthValue == requiredHp)
                {
                    stateMachine.currentTargetTransform = ShrineController.Instance.transform;
                    stateMachine.currentTargetCollider = ShrineController.Instance.shrineCollider;
                    return (true, stateToTransit);
                }
                break;
            case HpComparerTransitionType.Below:
                if (currentHpController.HealthValue < requiredHp)
                {
                    return (true, stateToTransit);
                }
                break;
        }
        return (false, null);
    }

    public override void InitializeTransition(BaseStateMachine stateMachine)
    {
        base.InitializeTransition(stateMachine);
        currentHpController = CharacterControllerHelper.Instance.characterHealth;
    }
}
