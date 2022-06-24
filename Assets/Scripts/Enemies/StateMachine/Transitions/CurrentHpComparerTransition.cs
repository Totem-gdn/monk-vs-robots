using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentHpComparerTransition : BaseTransition
{
    [SerializeField] private float requiredHp;
    [SerializeField] private HpComparerTransitionType hpComparerType;
    private EnemyHpController currentHpController;

    public override (bool, BaseState) CheckTransitionRequirements()
    {
        switch (hpComparerType)
        {
            case HpComparerTransitionType.Above:
                if(currentHpController.CurrentHp > requiredHp)
                {
                    return (true, stateToTransit);
                }
                break;
            case HpComparerTransitionType.Equals:
                if (currentHpController.CurrentHp == requiredHp)
                {
                    return (true, stateToTransit);
                }
                break;
            case HpComparerTransitionType.Below:
                if (currentHpController.CurrentHp < requiredHp)
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

        if(IsInitializedSuccessfully)
        {
            currentHpController = stateMachine.hpController;
        }
    }
}
