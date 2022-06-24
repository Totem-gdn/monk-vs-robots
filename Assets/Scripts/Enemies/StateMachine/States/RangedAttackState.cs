using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : BaseState
{
    [SerializeField] private RobotAim mainProjectileAimCannon;
    [SerializeField] private List<RobotAim> projectileAimParts;
    [SerializeField] private RobotProjectileCannon rangedCannon;
    [SerializeField] private float reloadTime;

    List<Coroutine> aimCoroutines = new List<Coroutine>();

    protected virtual void OnEnable()
    {
        IsCompleted = true;
        foreach(var aimPart in projectileAimParts)
        {
            aimCoroutines.Add(StartCoroutine(aimPart.Aim(stateMachine.currentTargetTransform)));
        }
    }

    protected virtual void Update()
    {
        if(mainProjectileAimCannon.IsAimed && rangedCannon.IsReloaded)
        {
            StartCoroutine(RangedAttack());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var aimCoroutine in aimCoroutines)
        {
            StopCoroutine(aimCoroutine);
        }
        aimCoroutines.Clear();
    }

    private IEnumerator RangedAttack()
    {
        IsCompleted = false;
        animator.SetTrigger(Constants.SHOOT_ANIMATION_TRIGGER);
        rangedCannon.Shoot(reloadTime);
        yield return new WaitForSeconds(GetCurentAnimatonLength());

        IsCompleted = true;
    }
}
