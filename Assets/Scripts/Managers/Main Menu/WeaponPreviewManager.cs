using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class WeaponPreviewManager : MonoBehaviour
{
    [SerializeField] private WeaponPreview currentWeaponPreview;

    public void ApplyWeaponPreview(TotemSpear totemWeapon)
    {
        currentWeaponPreview.ApplyShaftColor(GetColorFromHex(totemWeapon.shaftColor));
        currentWeaponPreview.ApplyTipMaterial(totemWeapon.tipMaterial);
        currentWeaponPreview.ApplySpearElement(totemWeapon.element);
    }

    private Color GetColorFromHex(string colorHex)
    {
        var resultColor = Color.black;
        if(!ColorUtility.TryParseHtmlString(colorHex,out resultColor))
        {
            resultColor = Color.black;
        }

        return resultColor;
    }
}
