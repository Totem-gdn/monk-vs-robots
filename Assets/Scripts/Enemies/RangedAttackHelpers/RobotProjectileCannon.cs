using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotProjectileCannon : MonoBehaviour
{
    [SerializeField] private float cannonForce;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform projectilesPool;

    private List<RobotProjectile> pooledProjectiles = new List<RobotProjectile>();

    public bool IsReloaded { get; private set; } = true;

    private void Awake()
    {
        InitializedPooledProjectiles();
        EventHandler.RegisterEvent<RobotProjectile>(gameObject, "ProjectileDestroyed", OnProjectileDestroyed);
    }

    private void InitializedPooledProjectiles()
    {
        pooledProjectiles = projectilesPool.GetComponentsInChildren<RobotProjectile>(true).ToList();
        foreach(var projectile in pooledProjectiles)
        {
            projectile.force = cannonForce;
            projectile.projectileCannon = gameObject;
        }
    }

    private void OnProjectileDestroyed(RobotProjectile destroyedProjectile)
    {
        destroyedProjectile.gameObject.SetActive(false);
        destroyedProjectile.transform.SetParent(projectilesPool);
        destroyedProjectile.transform.localPosition = Vector3.zero;
        pooledProjectiles.Add(destroyedProjectile);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<RobotProjectile>(gameObject, "ProjectileDestroyed", OnProjectileDestroyed);
    }

    public void Shoot(float reloadTime = 0)
    {
        if(pooledProjectiles.Count > 0)
        {
            pooledProjectiles[0].transform.SetParent(shootPoint);
            pooledProjectiles[0].transform.localPosition = Vector3.zero;
            pooledProjectiles[0].gameObject.SetActive(true);
            pooledProjectiles.RemoveAt(0);
            StartCoroutine(ReloadCannon(reloadTime));
        }
    }

    private IEnumerator ReloadCannon(float reloadTime)
    {
        IsReloaded = false;
        yield return new WaitForSeconds(reloadTime);
        IsReloaded = true;
    }
}
