using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Transform pickupPoolRoot;
    [SerializeField] private int timeToDisappear = 5;
    [SerializeField] private List<string> avaliableTriggerTags;

    private int disappearCountdown = 0;

    protected virtual void PickupAction()
    {
        HideItem();
    }

    private void Awake()
    {
        EventHandler.RegisterEvent("GameRestarted", HideItem);
    }

    protected virtual void HideItem()
    {
        gameObject.SetActive(false);
        if (pickupPoolRoot != null)
        {
            transform.position = pickupPoolRoot.position;
        }
    }

    private IEnumerator ItemDisappearCountdown()
    {
        yield return new WaitForSeconds(1);
        if (disappearCountdown > 0)
        {
            disappearCountdown--;
            StartCoroutine(ItemDisappearCountdown());
        }
        else
        {
            HideItem();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (avaliableTriggerTags.Contains(collider.tag))
        {
            StopAllCoroutines();
            PickupAction();
        }
    }

    private void OnEnable()
    {
        disappearCountdown = timeToDisappear;
        StartCoroutine(ItemDisappearCountdown());
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent("GameRestarted", HideItem);
    }
}
