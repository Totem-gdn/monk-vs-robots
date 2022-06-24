using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComboHelper : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float comboResetTime;
    [SerializeField] private List<int> comboAbilitiesIndexes;

    private int currentComboIndex = 0;
    private int maxComboIndex;
    private Coroutine comboReset;

    void Awake()
    {
        maxComboIndex = comboAbilitiesIndexes.Count;
        EventHandler.RegisterEvent(gameObject, "MeleeAttackStarted", OnMeleeAttackStarted);
        EventHandler.RegisterEvent(gameObject, "MeleeAttackEnded", OnMeleeAttackEnded);
    }

    private void OnMeleeAttackStarted()
    {
        if (comboReset != null)
        {
            StopCoroutine(comboReset);
        }
    }

    private void OnMeleeAttackEnded()
    {
        currentComboIndex++;
        if (currentComboIndex >= maxComboIndex)
        {
            currentComboIndex = 0;
        }
        EventHandler.ExecuteEvent(gameObject, "ComboAbilityIndexChanged", comboAbilitiesIndexes[currentComboIndex]);
        comboReset = StartCoroutine(ComboReset());
    }

    private IEnumerator ComboReset()
    {
        yield return new WaitForSeconds(comboResetTime);
        currentComboIndex = 0;
        EventHandler.ExecuteEvent(gameObject, "ComboAbilityIndexChanged", comboAbilitiesIndexes[currentComboIndex]);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent(gameObject, "MeleeAttackStarted", OnMeleeAttackStarted);
        EventHandler.UnregisterEvent(gameObject, "MeleeAttackEnded", OnMeleeAttackEnded);
    }
}
