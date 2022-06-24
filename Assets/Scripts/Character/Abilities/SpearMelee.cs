using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultStartType(AbilityStartType.ButtonDown)]
[DefaultStopType(AbilityStopType.Automatic)]
[DefaultInputName("Fire1")]
public class SpearMelee : Ability
{
    private bool isAttacking = false;
    private SpearWeapon spearWeapon;

    public override void Awake()
    {
        base.Awake();
        spearWeapon = CharacterControllerHelper.Instance.charactersSpearWeapon;
        EventHandler.RegisterEvent(m_GameObject, "MeleeAttackStarted", OnMeleeAttackStarted);
        EventHandler.RegisterEvent(m_GameObject, "MeleeAttackEnded", OnMeleeAttackEnded);
        EventHandler.RegisterEvent<int>(m_GameObject, "ComboAbilityIndexChanged", OnComboAbilityIndexChanged);
    }

    public override bool CanStartAbility()
    {
        if(!base.CanStartAbility())
        {
            return false;
        }

        return !isAttacking
            && m_CharacterLocomotion.Grounded
            && !m_CharacterLocomotion.IsAbilityTypeActive<SpearAimAbility>()
            && spearWeapon.IsInHand;
    }

    public override bool CanStopAbility()
    {
        return isAttacking ? false : true;
    }

    public override void OnDestroy()
    {
        EventHandler.UnregisterEvent(m_GameObject, "MeleeAttackStarted", OnMeleeAttackStarted);
        EventHandler.UnregisterEvent(m_GameObject, "MeleeAttackEnded", OnMeleeAttackEnded);
        EventHandler.UnregisterEvent<int>(m_GameObject, "ComboAbilityIndexChanged", OnComboAbilityIndexChanged);
        base.OnDestroy();
    }

    protected override void AbilityStarted()
    {
        base.AbilityStarted();
        isAttacking = true;
    }

    protected override void AbilityStopped(bool force)
    {
        base.AbilityStopped(force);
        ChangeAttackState(false);
    }

    private void OnMeleeAttackStarted()
    {
        ChangeAttackState(true);
    }

    private void OnMeleeAttackEnded()
    {
        ChangeAttackState(false);
    }

    private void OnComboAbilityIndexChanged(int newIndexParamter)
    {
        AbilityIndexParameter = newIndexParamter;
    }

    private void ChangeAttackState(bool isActive)
    {
        isAttacking = isActive;
        EventHandler.ExecuteEvent<bool>(CharacterControllerHelper.Instance.Character, "OnHitActivate", isAttacking);
    }
}
