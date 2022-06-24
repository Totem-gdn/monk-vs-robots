using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpController : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    public float MaxHp
    {
        get
        {
            return maxHp;
        }
    }
    public float CurrentHp
    {
        get
        {
            return currentHp;
        }
    }

    void Awake()
    {
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    public void Damage(float damageValue)
    {
        currentHp -= damageValue > currentHp ? currentHp : damageValue;
        hpSlider.value = CurrentHp;
        if(currentHp == 0)
        {
            EventHandler.ExecuteEvent(gameObject, "Death");
        }
    }

    public void Heal(float healValue)
    {
        var lostHp = maxHp - CurrentHp;
        currentHp += lostHp < healValue ? lostHp : healValue;
        hpSlider.value = CurrentHp;
    }
}
