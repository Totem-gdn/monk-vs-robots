using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPickup : PickupItem
{
    [SerializeField] private SpearWeapon spearWeapon;

    protected override void PickupAction()
    {
        if(!spearWeapon.IsInHand)
        {
            EventHandler.ExecuteEvent(spearWeapon.gameObject, "OnSpearPickedUp");
        }
    }

    protected override void HideItem()
    {
        StopAllCoroutines();
        if (!spearWeapon.IsInHand)
        {
            EventHandler.ExecuteEvent(spearWeapon.gameObject, "OnSpearPickedUp");
        }
    }
}
