using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseTransition : MonoBehaviour
{
    protected BaseStateMachine stateMachine;
    [SerializeField] protected BaseState stateToTransit;

    public bool IsInitializedSuccessfully { get; protected set; }

    public abstract (bool, BaseState) CheckTransitionRequirements();

    public virtual void InitializeTransition(BaseStateMachine stateMachine)
    {
        //Type type = Type.GetType(stateType);
        //if (type == null)
        //{
        //    IsInitializedSuccessfully = false;
        //    return;
        //}

        //IsInitializedSuccessfully = stateMachine.TryGetComponent(type, out Component stateComponent);
        //if (!(stateComponent is BaseState))
        //{
        //    IsInitializedSuccessfully = false;
        //    return;
        //}

        //stateToTransit = stateComponent as BaseState;
        this.stateMachine = stateMachine;
        IsInitializedSuccessfully = true;
    }
}
