using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkAndShootState : RangedAttackState
{
    [SerializeField] private TargetType walkTarget;
    [SerializeField] private bool isCheckpointsPath;
    [Min(0)]
    [SerializeField] private float checkpointTriggerDistance;

    private Collider walkTargetCollider;

    private NavMeshAgent navMeshAgent;
    private bool isAgentExists = false;

    protected override void Awake()
    {
        base.Awake();

        switch(walkTarget)
        {
            case TargetType.Player:
                walkTargetCollider = CharacterControllerHelper.Instance.playerCollider;
                break;
            case TargetType.Shrine:
                walkTargetCollider = ShrineController.Instance.shrineCollider;
                break;
            case TargetType.CurrentTarget:
                walkTargetCollider = stateMachine.currentTargetCollider;
                break;
        }

        isAgentExists = TryGetComponent<NavMeshAgent>(out navMeshAgent);
        if (!isAgentExists)
        {
            Debug.LogError($"NavMeshAgent not set to the WalkState of {name} enemy");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (isAgentExists)
        {
            navMeshAgent.isStopped = false;
            animator.SetTrigger(Constants.WALK_ANIMATION_TRIGGER);
        }
        IsCompleted = true;
    }

    protected override void Update()
    {
        base.Update();
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            if (IsCompleted)
            {
                animator.SetTrigger(Constants.WALK_ANIMATION_TRIGGER);
            }
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
        base.OnDisable();
        if (isAgentExists && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }

    private void SetNewDestination()
    {
        var newDestination = walkTargetCollider.ClosestPoint(transform.position);
        navMeshAgent.SetDestination(newDestination);
    }
}
