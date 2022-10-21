using DG.Tweening;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultStartType(AbilityStartType.ButtonDown)]
[DefaultStopType(AbilityStopType.Automatic)]
[DefaultInputName("Recall Spear")]
public class RecallSpearAbility : Ability
{
    private bool _isOnCooldown = false;
    private SpearWeapon _spearWeapon;
    private Coroutine _cooldownCoroutine;
    private Tween _cooldownTween;

    [SerializeField] private Image CooldownProgressImage;
    [SerializeField] private float SkillCooldown;

    public override void Awake()
    {
        _spearWeapon = CharacterControllerHelper.Instance.charactersSpearWeapon;
        base.Awake();
    }

    public override bool CanStartAbility()
    {
        if(!base.CanStartAbility())
        {
            return false;
        }

        return !_isOnCooldown && _spearWeapon.IsSpearLanded;
    }

    public override void OnDestroy()
    {
        if (_isOnCooldown)
        {
            m_CharacterLocomotion.StopCoroutine(_cooldownCoroutine);
            _cooldownTween.Kill();

            _isOnCooldown = false;
            if (CooldownProgressImage != null)
            {
                CooldownProgressImage.fillAmount = 0;
            }
        }

        base.OnDestroy();
    }

    protected override void AbilityStarted()
    {
        EventHandler.ExecuteEvent(_spearWeapon.gameObject, "OnSpearPickedUp");
        _isOnCooldown = true;
        _cooldownCoroutine = m_CharacterLocomotion.StartCoroutine(StartPickupCooldown());

        base.AbilityStarted();
    }

    private IEnumerator StartPickupCooldown()
    {
        CooldownProgressImage.fillAmount = 1;
        _cooldownTween = CooldownProgressImage.DOFillAmount(0, SkillCooldown);

        yield return new WaitForSeconds(SkillCooldown);

        _isOnCooldown = false;
    }
}
