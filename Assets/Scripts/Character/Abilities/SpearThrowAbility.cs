using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultStartType(AbilityStartType.Manual)]
[DefaultStopType(AbilityStopType.Automatic)]
public class SpearThrowAbility : Ability
{
    public float chargeMultiplier;

    protected override void AbilityStarted()
    {
        base.AbilityStarted();

        EventHandler.ExecuteEvent(CharacterControllerHelper.Instance.Character, "OnThrowSpear", chargeMultiplier);
    }
}
