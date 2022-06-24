using enums;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Camera;
using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class SpearWeapon : MonoBehaviour
{
    public float criticalDamageChance = 0;

    [SerializeField] private TipMaterialEnumGameObjectDictionary tipTypes;
    [SerializeField] private Material spearShaftMaterial;
    [SerializeField] private SpearPickup spearPicker;
    [SerializeField] private Collider spearPickupCollider;
    [SerializeField] private SpearRanged spearRangedController;
    [SerializeField] private CameraController characterCameraController;
    [Min(0)]
    [SerializeField] private float pickupDelay;
    [SerializeField] private HitDetector weaponHitDetector;
    [SerializeField] private Collider spearTipCollider;
    [SerializeField] private ElementEnumGameObjectDictionary elementTypes;

    private TipMaterialEnum tipMaterial;
    private ElementEnum element;

    private GameObject character;
    private int nonCriticalAttacks = 0;
    private float damageMultiplier = 1;

    private Transform parentRoot;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool IsInHand { get; private set; } = true;

    void Awake()
    {
        character = CharacterControllerHelper.Instance.Character;
        InitializeSpear(TotemManager.Instance.currentSpear);

        EventHandler.RegisterEvent<bool>(character, "OnHitActivate", EnableHitCollider);
        EventHandler.RegisterEvent<float>(character,"OnThrowSpear", OnSpearThrow);
        EventHandler.RegisterEvent(gameObject, "OnSpearLanded", OnSpearLanded);
        EventHandler.RegisterEvent(gameObject, "OnSpearPickedUp", OnSpearPickedUp);
        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);
    }

    public void InitializeSpear(TotemSpear spear)
    {
        SetSpearTip(spear.tipMaterial);
        weaponHitDetector.damageInfo.damageAmount = spear.damage;
        weaponHitDetector.damageInfo.baseDamage = spear.damage;
        spearRangedController.throwingForce = spear.range;

        //To Do: assign attack buf\debuf depending on element
        SetSpearElement(spear.element);
        spearShaftMaterial.color = spear.shaftColor;

        parentRoot = transform.parent;
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void SetSpearTip(TipMaterialEnum tipMaterialToSet)
    {
        tipTypes[tipMaterial].SetActive(false);
        tipMaterial = tipMaterialToSet;

        tipTypes[tipMaterial].SetActive(true);
    }

    private void SetSpearElement(ElementEnum elementToSet)
    {
        elementTypes[element].SetActive(false);
        element = elementToSet;

        elementTypes[elementToSet].SetActive(true);
    }

    private void EnableHitCollider(bool isEnabled)
    {
        spearTipCollider.enabled = isEnabled;

        if(isEnabled)
        {
            if(criticalDamageChance > 0 && IsCriticalHit())
            {
                weaponHitDetector.damageInfo.damageAmount =
                    weaponHitDetector.damageInfo.baseDamage * Constants.CRITICAL_DAMAGE_MULTIPLIER;
            }
            else
            {
                weaponHitDetector.damageInfo.damageAmount = weaponHitDetector.damageInfo.baseDamage;
            }
            weaponHitDetector.damageInfo.damageAmount *= damageMultiplier;
        }
        else
        {
            weaponHitDetector.ClearHittedTargets();
        }
    }

    private bool IsCriticalHit()
    {
        if (nonCriticalAttacks >= Constants.NON_CRITICAL_ATTACKS_LIMIT)
        {
            nonCriticalAttacks = 0;
            return true;
        }

        if (Random.Range(0, 100) <= criticalDamageChance)
        {
            nonCriticalAttacks = 0;
            return true;
        }

        nonCriticalAttacks++;
        return false;
    }

    private void OnSpearThrow(float chargeMultiplier)
    {
        damageMultiplier = chargeMultiplier;
        spearRangedController.forceMultiplier = chargeMultiplier;

        gameObject.layer = Constants.RANGED_SPEAR_LAYER_INDEX;
        characterCameraController.CanZoom = false;
        IsInHand = false;

        transform.SetParent(null);
        EnableHitCollider(true);
        spearRangedController.enabled = true;
    }

    private void OnSpearLanded()
    {
        EnableHitCollider(false);
        spearRangedController.enabled = false;
        damageMultiplier = 1;
        StartCoroutine(DelayedPickupActivation());
    }

    private void OnSpearPickedUp()
    {
        transform.SetParent(parentRoot);
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        gameObject.layer = Constants.MELEE_SPEAR_LAYER_INDEX;
        IsInHand = true;
        spearPicker.enabled = false;
        spearPickupCollider.enabled = false;
        characterCameraController.CanZoom = true;
    }

    private void OnGameRestarted()
    {
        EnableHitCollider(false);
        spearRangedController.enabled = false;
    }

    private IEnumerator DelayedPickupActivation()
    {
        yield return new WaitForSeconds(pickupDelay);
        spearPicker.enabled = true;
        spearPickupCollider.enabled = true;
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<bool>(character, "OnHitActivate", EnableHitCollider);
        EventHandler.UnregisterEvent<float>(character, "OnThrowSpear", OnSpearThrow);
        EventHandler.UnregisterEvent(gameObject, "OnSpearLanded", OnSpearLanded);
        EventHandler.UnregisterEvent(gameObject, "OnSpearPickedUp", OnSpearPickedUp);
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
    }
}
