using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAoeAttackState : BaseState
{
    [SerializeField] private List<Collider> aoeColliders;
    [SerializeField] private List<ContinuousHitDetector> aoeHitDetectors;
    [Min(0)]
    [SerializeField] private float aoeAttackDuration;

    private void OnEnable()
    {
        IsCompleted = true;
        animator.SetTrigger(Constants.IDLE_ANIMATION_TRIGGER);
        StartCoroutine(PerformAoeAttack());
    }

    private IEnumerator PerformAoeAttack()
    {
        EnableDisableAoeColliders(true);
        yield return new WaitForSeconds(aoeAttackDuration);
        EnableDisableAoeColliders(false);
        ClearAoeHitDetectors();
    }

    private void EnableDisableAoeColliders(bool isEnabled)
    {
        foreach(var aoeCollider in aoeColliders)
        {
            aoeCollider.enabled = isEnabled;
        }
    }

    private void ClearAoeHitDetectors()
    {
        foreach (var aoeHitDetector in aoeHitDetectors)
        {
            aoeHitDetector.ClearHittedTargets();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ClearAoeHitDetectors();
    }
}
