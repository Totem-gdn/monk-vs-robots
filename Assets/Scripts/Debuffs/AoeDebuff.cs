using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDebuff : MonoBehaviour
{
    [SerializeField] private GameObject parentRoot;
    [SerializeField] private Collider aoeCollider;
    [SerializeField] private HitDetector aoeHitDetector;
    [SerializeField] private ParticleSystem aoeParticles;
    [Min(0.1f)]
    [SerializeField] private float aoeActiveDuration;

    void Awake()
    {
        EventHandler.RegisterEvent(parentRoot, "OnSpearLanded", OnSpearLanded);
    }

    private void OnSpearLanded()
    {
        if(aoeHitDetector.damageInfo.attackDebuff != null)
        {
            StartCoroutine(ActivateDebuffAoe());
        }
    }

    private IEnumerator ActivateDebuffAoe()
    {
        aoeParticles.Play();
        aoeCollider.enabled = true;
        yield return new WaitForSeconds(aoeActiveDuration);
        aoeHitDetector.ClearHittedTargets();
        aoeCollider.enabled = false;
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent(gameObject, "OnSpearLanded", OnSpearLanded);
    }
}
