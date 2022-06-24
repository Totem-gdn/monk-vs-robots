using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Opsive.Shared.Events.EventHandler;

public class RobotProjectile : MonoBehaviour
{
    [NonSerialized] public float force;
    [NonSerialized] public GameObject projectileCannon;

    [Tooltip("Collision with such tags will destroy the projectile and return it to it's pool")]
    [SerializeField] private List<string> destructionTags;
    [SerializeField] private Rigidbody projectileRigidbody;
    [SerializeField] private Collider projectileCollider;
    [SerializeField] private HitDetector projectileHitDetector;
    [SerializeField] private bool isDestroyedOverTime = false;
    [SerializeField] private float timeUntilDestruction = 0;

    private void OnEnable()
    {
        transform.SetParent(null);
        EnableDisablePhysics(true);
        projectileRigidbody.AddForce(projectileCannon.transform.forward * force, ForceMode.Impulse);
        if(isDestroyedOverTime)
        {
            StartCoroutine(DestructionTimer());
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(destructionTags.Contains(collider.tag))
        {
            if(isDestroyedOverTime)
            {
                StopAllCoroutines();
            }
            ReturnProjectileToPool();
        }
    }

    private void EnableDisablePhysics(bool isEnabled)
    {
        projectileRigidbody.isKinematic = !isEnabled;
        projectileCollider.isTrigger = isEnabled;
    }

    private IEnumerator DestructionTimer()
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        ReturnProjectileToPool();
    }

    private void ReturnProjectileToPool()
    {
        EnableDisablePhysics(false);
        projectileHitDetector.ClearHittedTargets();
        //Play destroy animation
        EventHandler.ExecuteEvent(projectileCannon, "ProjectileDestroyed", this);
    }
}
