using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeState : BaseState
{
    private const float EXPLOSION_IMPACT_DURATION = 0.1f;

    [SerializeField] private Collider explosionCollider;
    [SerializeField] private HitDetector explosionhitDetector;
    [Min(0)]
    [SerializeField] private float explosionTimer;

    private void OnEnable()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTimer);
        explosionCollider.enabled = true;
        stateMachine.RobotSoundsManager.PlayAudioClip(SoundType.BombExplosion);
        yield return new WaitForSeconds(EXPLOSION_IMPACT_DURATION);
        explosionCollider.enabled = false;
        explosionhitDetector.ClearHittedTargets();
        stateMachine.hpController.Damage(stateMachine.hpController.CurrentHp);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        explosionCollider.enabled = false;
    }
}
