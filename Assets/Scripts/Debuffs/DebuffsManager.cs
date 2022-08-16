using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> avaliableDebuffsVisualizers;

    private Dictionary<BaseDebuff, GameObject> debuffsVisualizersInUse = new Dictionary<BaseDebuff, GameObject>();
    private List<BaseDebuff> activeDebuffs = new List<BaseDebuff>();
    private List<Type> debuffsOnCooldown = new List<Type>();

    private bool IsDebuffAvaliable(Type debuffType)
    {
        foreach(var debuff in activeDebuffs)
        {
            if(debuff.GetType() == debuffType)
            {
                return false;
            }
        }
        foreach(var debuff in debuffsOnCooldown)
        {
            if(debuff == debuffType)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator DebuffDurationTimer(BaseDebuff temporaryDebuff)
    {
        yield return new WaitForSeconds(temporaryDebuff.DebuffDuration);
        RemoveDebuff(temporaryDebuff);
    }

    private IEnumerator DebuffCooldown(Type expiredDebuffType, float resistanceDuration)
    {
        debuffsOnCooldown.Add(expiredDebuffType);
        yield return new WaitForSeconds(resistanceDuration);
        debuffsOnCooldown.Remove(expiredDebuffType);
    }

    private void RemoveDebuff(BaseDebuff debuffToRemove)
    {
        debuffToRemove.Remove();
        activeDebuffs.Remove(debuffToRemove);
        if (gameObject.activeSelf)
        {
            StartCoroutine(DebuffCooldown(debuffToRemove.GetType(), debuffToRemove.ResistanceDuration));
        }
        RemoveDebuffVisualization(debuffToRemove);
        Destroy(debuffToRemove);
    }

    private void ApplyDebuffVisualization(BaseDebuff debuffToApply)
    {
        if(debuffToApply.DebuffIcon != null && avaliableDebuffsVisualizers.Count > 0)
        {
            var debuffVisualizer = avaliableDebuffsVisualizers[0];
            avaliableDebuffsVisualizers.RemoveAt(0);
            debuffVisualizer.GetComponent<Image>().sprite = debuffToApply.DebuffIcon;
            debuffVisualizer.SetActive(true);
            debuffsVisualizersInUse.Add(debuffToApply, debuffVisualizer);
        }
    }

    private void RemoveDebuffVisualization(BaseDebuff debuffToRemove)
    {
        var debuffVisualizer = debuffsVisualizersInUse[debuffToRemove];
        debuffsVisualizersInUse.Remove(debuffToRemove);
        debuffVisualizer.GetComponent<Image>().sprite = null;
        debuffVisualizer.SetActive(false);
        avaliableDebuffsVisualizers.Add(debuffVisualizer);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        for(int i = activeDebuffs.Count-1; i > 0; i--)
        { 
            RemoveDebuff(activeDebuffs[i]);
        }
    }

    public void InitDebuff(BaseDebuff newDebuffData)
    {
        Type debuffType = newDebuffData.GetType();
        if(IsDebuffAvaliable(debuffType))
        {
            var debuff = ScriptableObject.CreateInstance(debuffType) as BaseDebuff;
            newDebuffData.debuffTarget = this;
            if (debuff.Initialize(newDebuffData))
            {
                activeDebuffs.Add(debuff);
                debuff.Apply();
                ApplyDebuffVisualization(debuff);
                if (debuff.DebuffDurationType == DebuffDurationType.Temporary)
                {
                    StartCoroutine(DebuffDurationTimer(debuff));
                }
            }
        }
    }
}
