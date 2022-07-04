using DG.Tweening;
using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] private MeshRenderer rockRenderer;
    [Min(0)]
    [SerializeField] private float showDuration;
    [Min(0)]
    [SerializeField] private float fadeDuration;
    [SerializeField] private float minMoveDistanceY;
    [SerializeField] private float maxMoveDistanceY;
    [SerializeField] private float minDeviationAngle;
    [SerializeField] private float maxDeviationAngle;

    private Transform poolRoot;
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private float maxObstacleDistance;
    private Material[] rockMaterials = new Material[0];

    void Awake()
    {
        poolRoot = transform.parent;
        defaultRotation = transform.localRotation;
        defaultPosition = transform.localPosition;
        rockMaterials = rockRenderer.materials;

        EventHandler.RegisterEvent<float>(poolRoot, "ShowRocks", OnShowRocks);
        EventHandler.RegisterEvent(poolRoot, "HideRocks", OnHideRocks);
    }

    private void OnShowRocks(float aoeRadius)
    {
        maxObstacleDistance = aoeRadius;

        if (DefinePositionY())
        {
            var moveDistanceY = Random.Range(minMoveDistanceY, maxMoveDistanceY);
            var endValue = transform.position.y + moveDistanceY;

            var deviationAngle = Random.Range(minDeviationAngle, maxDeviationAngle);
            var eulerRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(eulerRotation.x + deviationAngle, eulerRotation.y + deviationAngle, eulerRotation.z + deviationAngle);

            transform.parent = null;
            transform.DOMoveY(endValue, showDuration);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, new Vector3(0, 2.5f, 0), Color.red);
        Debug.DrawRay(transform.position, new Vector3(0, -2.5f, 0), Color.blue);
    }

    private bool DefinePositionY()
    {
        if(CheckObstacleHit(Vector3.down))
        {
            return true;
        }
        else if(CheckObstacleHit(Vector3.up))
        {
            return true;
        }

        return false;
    }

    private bool CheckObstacleHit(Vector3 rayDirection)
    {
        var origin = new Vector3(transform.position.x, transform.position.y - rayDirection.y, transform.position.z);
        if(Physics.Raycast(origin, rayDirection, out RaycastHit hitInfo, maxObstacleDistance))
        {
            var newPositionY = hitInfo.point.y - transform.localScale.y;
            transform.position = new Vector3(hitInfo.point.x, newPositionY, hitInfo.point.z);
            return true;
        }

        return false;
    }

    private void OnHideRocks()
    {
        if (rockMaterials.Length > 0 && gameObject.activeSelf)
        {
            StartCoroutine(FadeRock());
        }
        else
        {
            DeactivateRock();
        }
    }

    private IEnumerator FadeRock()
    {
        foreach (var material in rockMaterials)
        {
            material.DOFade(0, fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);
        DeactivateRock();
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<float>(poolRoot, "ShowRocks", OnShowRocks);
        EventHandler.UnregisterEvent(poolRoot, "HideRocks", OnHideRocks);
    }

    public void DeactivateRock()
    {
        gameObject.SetActive(false);
        transform.SetParent(poolRoot);

        transform.localRotation = defaultRotation;
        transform.localPosition = defaultPosition;

        foreach (var material in rockMaterials)
        {
            material.DOKill();
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
        }
    }
}
