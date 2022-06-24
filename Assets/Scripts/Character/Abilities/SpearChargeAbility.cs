using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultStartType(AbilityStartType.ButtonDown)]
[DefaultStopType(AbilityStopType.ButtonUp)]
[DefaultInputName("Fire1")]
public class SpearChargeAbility : Ability
{
    private const float CHARGE_MULTIPLIER_START_VALUE = 1f;

    [Min(1.1f)]
    [SerializeField] private float maxChargeMultiplierValue = 1.1f;
    [Min(0.1f)]
    [SerializeField] private float chargingTime = 2f;
    [SerializeField] private Image chargeProgressImage;
    public override bool IsConcurrent { get { return true; } }

    private Coroutine spearChargeCoroutine;
    protected SpearAimAbility spearAimAbility;
    protected SpearThrowAbility spearThrowAbility;
    private float chargeMultiplier = CHARGE_MULTIPLIER_START_VALUE;

    public override void Awake()
    {
        base.Awake();
        spearAimAbility = m_CharacterLocomotion.GetAbility<SpearAimAbility>();
        spearThrowAbility = m_CharacterLocomotion.GetAbility<SpearThrowAbility>();
    }

    public override bool CanStartAbility()
    {
        if (!base.CanStartAbility())
        {
            return false;
        }

        return m_CharacterLocomotion.IsAbilityTypeActive<SpearAimAbility>();
    }

    protected override void AbilityStarted()
    {
        base.AbilityStarted();
        spearChargeCoroutine = m_CharacterLocomotion.StartCoroutine(SpearCharge());
    }

    protected override void AbilityStopped(bool force)
    {
        base.AbilityStopped(force);
        if (m_CharacterLocomotion.IsAbilityTypeActive<SpearAimAbility>())
        {
            m_CharacterLocomotion.TryStopAbility(spearAimAbility);
            spearThrowAbility.chargeMultiplier = chargeMultiplier;
            m_CharacterLocomotion.TryStartAbility(spearThrowAbility);
        }
        m_CharacterLocomotion.StopCoroutine(spearChargeCoroutine);
        chargeMultiplier = CHARGE_MULTIPLIER_START_VALUE;
        UpdateChargeProgress();
    }

    private IEnumerator SpearCharge()
    {
        float timeElapsed = 0;

        while (chargeMultiplier < maxChargeMultiplierValue)
        {
            chargeMultiplier = Mathf.Lerp(CHARGE_MULTIPLIER_START_VALUE, maxChargeMultiplierValue, timeElapsed / chargingTime);
            timeElapsed += Time.deltaTime;
            UpdateChargeProgress();
            yield return null;
        }
    }

    private void UpdateChargeProgress()
    {
        if(chargeProgressImage != null && chargeProgressImage.type == Image.Type.Filled)
        {
            var currentProggress = chargeMultiplier - CHARGE_MULTIPLIER_START_VALUE;
            chargeProgressImage.fillAmount = currentProggress / (maxChargeMultiplierValue - CHARGE_MULTIPLIER_START_VALUE);
        }
    }
}
