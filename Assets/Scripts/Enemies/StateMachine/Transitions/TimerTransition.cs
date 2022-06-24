using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimerTransition : BaseTransition
{
    [SerializeField] private float timerDuration;

    private bool isTimerRunning = false;
    private bool isTimerEnded = false;

    public override (bool, BaseState) CheckTransitionRequirements()
    {
        if(!isTimerRunning)
        {
            isTimerRunning = true;
            stateMachine.StartCoroutine(RunTimer());
        }
        else if(isTimerEnded)
        {
            isTimerEnded = false;
            isTimerRunning = false;
            return (true, stateToTransit);
        }

        return (false, null);
    }

    public void StopTimer()
    {
        isTimerEnded = false;
        isTimerRunning = false;
    }

    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(timerDuration);
        isTimerEnded = true;
    }
}
