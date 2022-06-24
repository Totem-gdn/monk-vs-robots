using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousHitDetector : MonoBehaviour
{
    [SerializeField] private List<string> hitTags = new List<string>();
    [SerializeField] private DamageInfo damageInfo;
    [Min(0)]
    [SerializeField] private float hitTickIntervals;

    private List<GameObject> hittedTargets = new List<GameObject>();
    private List<DamageProcessor> damageProcessors = new List<DamageProcessor>();
    private Dictionary<HitBox,Coroutine> damageTickCoroutines = new Dictionary<HitBox, Coroutine>();

    public void ClearHittedTargets()
    {
        foreach(var damageTick in damageTickCoroutines)
        {
            StopDamageTicks(damageTick.Key);
        }
        hittedTargets.Clear();
        damageProcessors.Clear();
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitTags.Contains(hitCollider.tag) && !hittedTargets.Contains(hitCollider.gameObject))
        {
            hittedTargets.Add(hitCollider.gameObject);
            if (hitCollider.TryGetComponent(out HitBox hitBox))
            {
                if (!damageProcessors.Contains(hitBox.DamageProcessor))
                {
                    RegisterHitBox(hitBox);
                }
                else if (hitBox.IsMultipleAreaHit)
                {
                    RegisterHitBox(hitBox);
                }
            }
        }
    }

    private void OnTriggerExit(Collider hitCollider)
    {
        hittedTargets.Remove(hitCollider.gameObject);
        if (hitCollider.TryGetComponent(out HitBox hitBox))
        {
            StopDamageTicks(hitBox);
            damageTickCoroutines.Remove(hitBox);
        }
    }

    private void RegisterHitBox(HitBox hitBox)
    {
        damageProcessors.Add(hitBox.DamageProcessor);
        damageTickCoroutines.Add(hitBox, StartCoroutine(DamageTick(hitBox.DamageProcessor)));
    }

    private IEnumerator DamageTick(DamageProcessor damageProcessor)
    {
        while (true)
        {
            damageProcessor.ProcessDamage(damageInfo);
            yield return new WaitForSeconds(hitTickIntervals);
        }
    }

    private void StopDamageTicks(HitBox hitBox)
    {
        hittedTargets.Remove(hitBox.gameObject);
        damageProcessors.Remove(hitBox.DamageProcessor);
        StopCoroutine(damageTickCoroutines[hitBox]);
    }
}
