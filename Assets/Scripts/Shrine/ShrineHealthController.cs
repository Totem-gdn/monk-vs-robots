using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrineHealthController : MonoBehaviour
{
    [SerializeField] private Slider shrineHpSlider;
    [SerializeField] private ShrineController shrineController;
    [SerializeField] private float maxHp;

    private float currentHp;

    public static ShrineHealthController Instance { get; private set; }

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        InitializeHp();
    }

    public void Damage(float damageValue)
    {
        currentHp -= damageValue > currentHp ? currentHp : damageValue;
        shrineHpSlider.value = currentHp;

        if(currentHp == 0)
        {
            shrineController.OnShrineDestroyed();
        }
    }

    public void Repair(float repairValue)
    {
        var lostHp = maxHp - currentHp;
        currentHp += lostHp < repairValue ? lostHp : repairValue;
        shrineHpSlider.value = currentHp;
    }

    public void InitializeHp()
    {
        currentHp = maxHp;
        shrineHpSlider.maxValue = maxHp;
        shrineHpSlider.value = maxHp;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
