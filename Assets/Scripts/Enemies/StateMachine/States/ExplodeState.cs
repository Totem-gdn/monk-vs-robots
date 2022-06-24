using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeState : BaseState
{
    [SerializeField] private Collider explosionCollider;
    [SerializeField] private HitDetector explosionhitDetector;

    private void OnEnable()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        //Play explode animation
        //Constant two seconds wait time during tests
        yield return new WaitForSeconds(2);
        explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        explosionCollider.enabled = false;
        explosionhitDetector.ClearHittedTargets();
        stateMachine.hpController.Damage(stateMachine.hpController.CurrentHp);
    }
}
