using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceTransition : BaseTransition
{
    [SerializeField] private float triggerDistance;
    [SerializeField] private DistanceTransitionType distanceType;

    protected Transform selfTransform;
    protected Transform targetTransform;
    protected Collider selfCollider;
    protected Collider targetCollider;

    public override (bool,BaseState) CheckTransitionRequirements()
    {
        var closestTargetPoint = targetCollider.ClosestPoint(selfTransform.position);
        var closestSelfPoint = selfCollider.ClosestPoint(closestTargetPoint);
        var distanceToTarget = Vector3.Distance(closestTargetPoint, closestSelfPoint);

        switch (distanceType)
        {
            case DistanceTransitionType.InRange:
                if (distanceToTarget <= triggerDistance)
                {
                    stateMachine.currentTargetTransform = targetTransform;
                    stateMachine.currentTargetCollider = targetCollider;
                    return (true, stateToTransit);
                }
                break;
            case DistanceTransitionType.OutOfRange:
                if (distanceToTarget >= triggerDistance)
                {
                    stateMachine.currentTargetTransform = targetTransform;
                    stateMachine.currentTargetCollider = targetCollider;
                    return (true, stateToTransit);
                }
                break;
        }

        return (false, null);
    }

    public override void InitializeTransition(BaseStateMachine stateMachine)
    {
        base.InitializeTransition(stateMachine);

        if (IsInitializedSuccessfully)
        {
            InitializeTargets();
        }
    }

    protected virtual void InitializeTargets()
    {
        selfCollider = stateMachine.mainCollider;
        targetCollider = CharacterControllerHelper.Instance.playerCollider;
        selfTransform = stateMachine.transform;
        targetTransform = CharacterControllerHelper.Instance.Character.transform;
    }
}
