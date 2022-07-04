using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunDebuff", menuName = "ScriptableObjects/Debuffs/Stun")]
public class Stun : BaseDebuff
{
    private Animator targetAnimator;
    private BaseStateMachine targetStateMachine;

    public override bool Initialize(BaseDebuff baseDebuff)
    {
        try
        {
            Stun stunData = baseDebuff as Stun;

            debuffTarget = stunData.debuffTarget;
            debuffDurationType = stunData.DebuffDurationType;
            debuffDuration = stunData.DebuffDuration;
            resistanceDuration = stunData.ResistanceDuration;
            debuffIcon = stunData.debuffIcon;

            debuffTarget.TryGetComponent<Animator>(out targetAnimator);
            debuffTarget.TryGetComponent<BaseStateMachine>(out targetStateMachine);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override void Apply()
    {
        targetStateMachine?.ResetStateMachine();
        targetAnimator?.SetTrigger(Constants.IDLE_ANIMATION_TRIGGER);

        if (targetAnimator != null)
        {
            targetAnimator.enabled = false;
        }
    }

    public override void Remove()
    {
        targetStateMachine?.ActivateStartState();
        if (targetAnimator != null)
        {
            targetAnimator.enabled = true;
        }
    }
}
