using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : BaseState
{
    [SerializeField] private TargetType targetType;

    private Collider targetCollider;
    private NavMeshAgent navMeshAgent;
    private bool isAgentExists = false;

    protected override void Awake()
    {
        base.Awake();
        targetCollider = DefineTargetCollider();
        isAgentExists = TryGetComponent<NavMeshAgent>(out navMeshAgent);
        if(!isAgentExists)
        {
            Debug.LogError($"NavMeshAgent not set to the WalkState of {name} enemy");
        }
    }

    protected void Update()
    {
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            SetNewDestination();
        }
    }

    protected override void OnDisable()
    {
        if(isAgentExists && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
        base.OnDisable();
    }

    private void SetNewDestination()
    {
        var newDestination = targetCollider.ClosestPoint(transform.position);
        navMeshAgent.SetDestination(newDestination);
    }

    private Collider DefineTargetCollider()
    {
        switch (targetType)
        {
            case TargetType.Player:
                return CharacterControllerHelper.Instance.playerCollider;
            case TargetType.Shrine:
                return ShrineController.Instance.shrineCollider;
            case TargetType.CurrentTarget:
                return stateMachine.currentTargetCollider;
            default:
                return stateMachine.currentTargetCollider;
        }
    }

    private void OnEnable()
    {
        if (isAgentExists)
        {
            navMeshAgent.isStopped = false;
            animator.SetTrigger(Constants.WALK_ANIMATION_TRIGGER);
        }
        IsCompleted = true;
    }
}
