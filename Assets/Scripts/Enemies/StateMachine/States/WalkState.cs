using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : BaseState
{
    [SerializeField] private TargetType targetType;
    [SerializeField] private bool isCheckpointsPath;
    [Min(0)]
    [SerializeField] private float checkpointTriggerDistance;

    private Collider targetCollider;
    private NavMeshAgent navMeshAgent;
    private bool isAgentExists = false;
    private int localCheckpointIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        targetCollider = DefineTargetCollider();
        isAgentExists = TryGetComponent<NavMeshAgent>(out navMeshAgent);
        if (!isAgentExists)
        {
            Debug.LogError($"NavMeshAgent not set to the WalkState of {name} enemy");
        }
    }

    protected void Update()
    {
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            if (isCheckpointsPath && stateMachine.CurrentCheckpointIndex < stateMachine.CheckpointsPath.Count)
            {
                var distance = Vector3.Distance(transform.position, stateMachine.CurrentCheckpoint.position);

                if (distance <= checkpointTriggerDistance)
                {
                    stateMachine.CheckpointReached();
                    navMeshAgent.SetDestination(stateMachine.CurrentCheckpoint.position);
                }
            }
            else
            {
                SetNewDestination();
            }
        }
    }

    protected override void OnDisable()
    {
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
        base.OnDisable();
    }

    private void OnEnable()
    {
        if (isAgentExists)
        {
            if(isCheckpointsPath && localCheckpointIndex != stateMachine.CurrentCheckpointIndex)
            {
                navMeshAgent.SetDestination(stateMachine.CurrentCheckpoint.position);
            }
            navMeshAgent.isStopped = false;
            animator.SetTrigger(Constants.WALK_ANIMATION_TRIGGER);
        }
        IsCompleted = true;
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
}
