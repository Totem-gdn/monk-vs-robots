using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseState : MonoBehaviour
{
    protected Animator animator;
    protected BaseStateMachine stateMachine;

    [SerializeReference]
    public List<BaseTransition> stateTransitions = new List<BaseTransition>();

    public bool IsCompleted { get; protected set; } = false;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<BaseStateMachine>();
        animator = GetComponent<Animator>();
    }

    protected virtual void StopTimerTransitions()
    {
        foreach(var transition in stateTransitions)
        {
            if(transition is TimerTransition)
            {
                (transition as TimerTransition).StopTimer();
            }
        }
    }

    protected virtual void OnDisable()
    {
        IsCompleted = false;
        StopTimerTransitions();
        StopAllCoroutines();
    }

    protected float GetCurentAnimatonLength()
    {
        var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var animatorClips = animator.GetCurrentAnimatorClipInfo(0);
        return currentStateInfo.length;
    }
}
