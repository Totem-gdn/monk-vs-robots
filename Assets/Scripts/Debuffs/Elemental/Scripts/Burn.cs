using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="BurnDebuff", menuName ="ScriptableObjects/Debuffs/Burn")]
public class Burn : BaseDebuff
{
    [Min(0)]
    [SerializeField] private float damagePerTick;
    [Min(0)]
    [SerializeField] private float tickIntervals;

    private EnemyHpController hpController;
    private Coroutine burnCoroutine;

    public override bool Initialize(BaseDebuff baseDebuff)
    {
        try
        {
            Burn burnData = baseDebuff as Burn;

            debuffTarget = burnData.debuffTarget;
            debuffDurationType = burnData.DebuffDurationType;
            debuffDuration = burnData.DebuffDuration;
            resistanceDuration = burnData.ResistanceDuration;
            debuffIcon = burnData.debuffIcon;

            damagePerTick = burnData.damagePerTick;
            tickIntervals = burnData.tickIntervals;
            hpController = debuffTarget.GetComponent<EnemyHpController>();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override void Apply()
    {
        burnCoroutine = debuffTarget.StartCoroutine(BurnTicks());
    }

    public override void Remove()
    {
        debuffTarget.StopCoroutine(burnCoroutine);
    }

    private IEnumerator BurnTicks()
    {
        while(true)
        {
            hpController.Damage(damagePerTick);
            yield return new WaitForSeconds(tickIntervals);
        }
    }
}
