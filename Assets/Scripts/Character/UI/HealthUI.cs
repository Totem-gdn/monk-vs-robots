
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Traits.Damage;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private GameObject character;
    private Slider hpSlider;
    private AttributeManager attributeManager;
    private Attribute healthAttribute;

    void Awake()
    {
        hpSlider = GetComponent<Slider>();
        character = CharacterControllerHelper.Instance.Character;
        attributeManager = character.GetComponent<AttributeManager>();
        healthAttribute = attributeManager.GetAttribute(Constants.HEALTH_ATTRIBUTE_NAME);

        InitializeHp();

        EventHandler.RegisterEvent(character, "OnRespawn", OnRespawn);
        EventHandler.RegisterEvent<DamageData>(character,"OnHealthDamageWithData", OnDamage);
        EventHandler.RegisterEvent<float>(character, "OnHealthHeal", OnHeal);
    }

    private void OnDamage(DamageData damage)
    {
        hpSlider.value -= damage.Amount;
    }

    private void OnHeal(float healAmount)
    {
        hpSlider.value += healAmount;
    }

    private void OnRespawn()
    {
        hpSlider.value = healthAttribute.MaxValue; 
    }

    private void InitializeHp()
    {
        hpSlider.minValue = healthAttribute.MinValue;
        hpSlider.maxValue = healthAttribute.MaxValue;
        hpSlider.value = healthAttribute.MaxValue;
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent(character,"OnRespawn", OnRespawn);
        EventHandler.UnregisterEvent<DamageData>(character, "OnHealthDamageWithData", OnDamage);
        EventHandler.UnregisterEvent<float>(character, "OnHealthHeal", OnHeal);
    }
}
