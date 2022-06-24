using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private void OnEnable()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        animator.SetTrigger(Constants.DEATH_ANIMATION_TRIGGER);
        yield return new WaitForSeconds(GetCurentAnimatonLength());
        EventHandler.ExecuteEvent(stateMachine.spawnerRoot, "RobotDied", stateMachine);
        EventHandler.ExecuteEvent(WavesManager.Instance, "RobotDied");
        stateMachine.ResetStateMachine();
        this.enabled = false;
    }
}
