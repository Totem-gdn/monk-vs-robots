using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AoeProjectileAttackState : BaseState
{
    [SerializeField] private GameObject aoeRoot;

    private List<RobotProjectileCannon> aoeCannons = new List<RobotProjectileCannon>();

    protected override void Awake()
    {
        base.Awake();
        aoeCannons = aoeRoot.GetComponentsInChildren<RobotProjectileCannon>(true).ToList();
    }

    private void OnEnable()
    {
        animator.SetTrigger(Constants.IDLE_ANIMATION_TRIGGER);
        StartCoroutine(PerformAoeAttack());
    }

    private IEnumerator PerformAoeAttack()
    {
        //Play shoot animation
        yield return new WaitForSeconds(0.5f);

        foreach (var cannon in aoeCannons)
        {
            cannon.Shoot();
        }

        //Play return to normal state animation
        yield return new WaitForSeconds(0.5f);
        IsCompleted = true;
    }
}
