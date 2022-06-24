using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : BaseState
{
    [SerializeField] private Collider meleeAttackCollider;
    [SerializeField] private HitDetector weaponHitDetector;
    [Min(0.1f)]
    [SerializeField] private float delayBetweenAttacks;
    [Min(0)]
    [Tooltip("Angle between robot and his target to perform the attack. (Without Y axis)")]
    [SerializeField] private float avaliableAttackAngle;
    [SerializeField] private bool isRotateAnimationApplied;

    private bool isAttacking = false;
    private bool isOnCooldown = false;
    private bool isRotating = false;

    private void OnEnable()
    {
        IsCompleted = true;
        if (meleeAttackCollider == null)
        {
            Debug.LogError($"MeleeAttackCollider wasn't set on {name} enemy");
            isAttacking = true;
        }
    }

    private IEnumerator MeleeAttack()
    {
        //Wait for the attack preparation
        yield return new WaitForSeconds(GetCurentAnimatonLength());

        meleeAttackCollider.enabled = true;
        yield return new WaitForSeconds(GetCurentAnimatonLength());

        meleeAttackCollider.enabled = false;
        weaponHitDetector.ClearHittedTargets();
        IsCompleted = true;

        yield return new WaitForSeconds(GetCurentAnimatonLength());

        isAttacking = false;
        StartCoroutine(AttackCooldown());
        yield return new WaitForSeconds(delayBetweenAttacks);
    }

    private void Update()
    {
        if (!isAttacking)
        {
            var (isTargetInRange, targetRotation) = CalculateTargetRotation();

            if (isTargetInRange && !isOnCooldown)
            {
                isAttacking = true;
                animator.SetTrigger(Constants.MELEE_ATTACK_ANIMATION_TRIGGER);
            }
            else if (!isTargetInRange)
            {
                if (isRotateAnimationApplied && !isRotating)
                {
                    animator.SetTrigger(Constants.ROTATE_ANIMATION_TRIGGER);
                    isRotating = transform;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
            }
        }
    }

    private (bool, Quaternion) CalculateTargetRotation()
    {
        var modiffiedTargetPosition = new Vector3(
            stateMachine.currentTargetTransform.position.x,
            transform.position.y,
            stateMachine.currentTargetTransform.position.z);

        var targetDirection = (modiffiedTargetPosition - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        bool isTargetInRange = Quaternion.Angle(transform.rotation, targetRotation) < avaliableAttackAngle ? true : false;

        return (isTargetInRange, targetRotation);
    }

    private IEnumerator AttackCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(delayBetweenAttacks);
        isOnCooldown = false;
    }

    private void OnMeleeAttackPreparation()
    {
        IsCompleted = false;
        isRotating = false;
        StartCoroutine(MeleeAttack());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isAttacking = false;
        isOnCooldown = false;
        isRotating = false;
    }
}
