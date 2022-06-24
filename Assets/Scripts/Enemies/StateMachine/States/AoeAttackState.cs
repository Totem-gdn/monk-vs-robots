using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackState : BaseState
{
    [SerializeField] private Transform aoeAttackTransform;
    [SerializeField] private float finalScale;
    [SerializeField] private float duration;
    [SerializeField] private HitDetector aoeAttackHitDetector;

    private float startScale;

    protected override void Awake()
    {
        base.Awake();
        startScale = aoeAttackTransform.localScale.x;
    }

    private void OnEnable()
    {
        animator.SetTrigger(Constants.IDLE_ANIMATION_TRIGGER);
        aoeAttackTransform.gameObject.SetActive(true);
        StartCoroutine(PerformAoeGrow());
    }

    private IEnumerator PerformAoeGrow()
    {
        float timeElapsed = 0;

        while (aoeAttackTransform.localScale.x < finalScale)
        {
            var newScale = Mathf.Lerp(startScale, finalScale, timeElapsed / duration);
            aoeAttackTransform.localScale = new Vector3(newScale, aoeAttackTransform.localScale.y, newScale);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        IsCompleted = true;
        ReturnToStartState();
    }

    private void ReturnToStartState()
    {
        aoeAttackTransform.localScale = new Vector3(startScale, aoeAttackTransform.localScale.y, startScale);
        aoeAttackTransform.gameObject.SetActive(false);
        aoeAttackHitDetector.ClearHittedTargets();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ReturnToStartState();
    }
}
