using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EventHandler = Opsive.Shared.Events.EventHandler;

[CreateAssetMenu(fileName = "SlowDebuff", menuName = "ScriptableObjects/Debuffs/Slow")]
public class Slow : BaseDebuff
{
    [SerializeField] private float slowMultiplier;

    private Animator targetAnimator;
    private NavMeshAgent targetNavMeshAgent;

    private float normalAnimatorSpeed;
    private float normalMoveSpeed;
    private float normalAngularSpeed;

    public override bool Initialize(BaseDebuff baseDebuff)
    {
        try
        {
            Slow slowData = baseDebuff as Slow;

            debuffTarget = slowData.debuffTarget;
            debuffDurationType = slowData.DebuffDurationType;
            debuffDuration = slowData.DebuffDuration;
            resistanceDuration = slowData.ResistanceDuration;
            debuffIcon = slowData.debuffIcon;

            slowMultiplier = slowData.slowMultiplier;
            if(debuffTarget.TryGetComponent<Animator>(out targetAnimator))
            {
                normalAnimatorSpeed = targetAnimator.speed;
            }
            if(debuffTarget.TryGetComponent<NavMeshAgent>(out targetNavMeshAgent))
            {
                normalMoveSpeed = targetNavMeshAgent.speed;
                normalAngularSpeed = targetNavMeshAgent.angularSpeed;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override void Apply() 
    {
        if (targetAnimator != null)
        {
            targetAnimator.speed *= slowMultiplier;
        }
        if(targetNavMeshAgent!=null)
        {
            targetNavMeshAgent.speed *= slowMultiplier;
            targetNavMeshAgent.angularSpeed *= slowMultiplier;
        }
        EventHandler.ExecuteEvent(debuffTarget.gameObject, "SpeedChange", slowMultiplier, false);
    }

    public override void Remove()
    {
        if (targetAnimator != null)
        {
            targetAnimator.speed = normalAnimatorSpeed;
        }
        if (targetNavMeshAgent != null)
        {
            targetNavMeshAgent.speed = normalMoveSpeed;
            targetNavMeshAgent.angularSpeed = normalAngularSpeed;
        }

        EventHandler.ExecuteEvent(debuffTarget.gameObject, "SpeedChange", 1, true);
    }
}
