using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAoe : MonoBehaviour
{
    [SerializeField] private Transform rocksPoolRoot;
    [Min(0)]
    [SerializeField] private float rocksActiveTime;
    [Min(0)]
    [SerializeField] private float aoeDamagePercentage;
    [SerializeField] private HitDetector aoeHitDetector;
    [SerializeField] private SphereCollider aoeCollider;

    private List<RockController> pooledRocks = new List<RockController>();

    private void Awake()
    {
        if (rocksPoolRoot != null)
        {
            InitializeRocks();
        }
    }

    private void InitializeRocks()
    {
        foreach(Transform child in rocksPoolRoot.transform)
        {
            pooledRocks.Add(child.GetComponent<RockController>());
        }
    }

    private IEnumerator HideRocks()
    {
        yield return new WaitForSeconds(rocksActiveTime);
        EventHandler.ExecuteEvent(rocksPoolRoot, "HideRocks");
    }

    private void ShowRocks()
    {
        foreach (var rock in pooledRocks)
        {
            if (rock.gameObject.activeSelf)
            {
                rock.StopAllCoroutines();
                rock.DeactivateRock();
            }
            rock.gameObject.SetActive(true);
        }

        rocksPoolRoot.eulerAngles = Vector3.zero;
        EventHandler.ExecuteEvent(rocksPoolRoot, "ShowRocks", aoeCollider.radius);
    }

    public void InitializeAoeDamage(float spearDamage)
    {
        var aoeDamage = spearDamage * aoeDamagePercentage / 100;
        aoeHitDetector.damageInfo.baseDamage = aoeDamage;
        aoeHitDetector.damageInfo.damageAmount = aoeDamage;
    }

    public IEnumerator ActivateAoe()
    {
        ShowRocks();
        aoeCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);

        aoeHitDetector.ClearHittedTargets();
        aoeCollider.enabled = false;
        StartCoroutine(HideRocks());
    }
}
