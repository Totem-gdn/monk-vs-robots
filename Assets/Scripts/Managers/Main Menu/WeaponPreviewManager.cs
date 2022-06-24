using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class WeaponPreviewManager : MonoBehaviour
{
    [SerializeField] private WeaponPreview currentWeaponPreview;

    public void ApplyWeaponPreview(TotemSpear totemWeapon)
    {
        currentWeaponPreview.ApplyShaftColor(totemWeapon.shaftColor);
        currentWeaponPreview.ApplyTipMaterial(totemWeapon.tipMaterial);
        currentWeaponPreview.ApplySpearElement(totemWeapon.element);
    }
}
