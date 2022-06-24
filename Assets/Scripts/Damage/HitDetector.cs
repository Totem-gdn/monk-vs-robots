using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public float damageAmount;
    public string damageDealerName;
    public float baseDamage;
    //Add refs to attack buffs\debuffs
}

public class HitDetector : MonoBehaviour
{
    public List<string> hitTags = new List<string>();
    public DamageInfo damageInfo;

    private List<GameObject> hittedTargets = new List<GameObject>();
    private List<DamageProcessor> damageProcessors = new List<DamageProcessor>();

    public void ClearHittedTargets()
    {
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
                    hitBox.DamageProcessor.ProcessDamage(damageInfo);
                    damageProcessors.Add(hitBox.DamageProcessor);
                }
                else if(hitBox.IsMultipleAreaHit)
                {
                    hitBox.DamageProcessor.ProcessDamage(damageInfo);
                }
            }
        }
    }
}
