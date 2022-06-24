using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCompletedTransition : BaseTransition
{
    public override (bool, BaseState) CheckTransitionRequirements()
    {
        if(stateMachine.CurrentState.IsCompleted)
        {
            return (true, stateToTransit);
        }

        return (false, null);
    }
}
