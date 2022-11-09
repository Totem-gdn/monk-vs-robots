using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAim : MonoBehaviour
{
    [SerializeField] private List<AxisType> staticAxises;
    [SerializeField] private AxisType restrictedAxis;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    [SerializeField] private bool isLocalRotation;
    [SerializeField] private bool isRaycasting;
    [SerializeField] private LayerMask hitTargetsMask;
    [SerializeField] private float shootDistance;

    private float defaultAimRotationSpeed = 0.1f;
    private float currentAimRotationSpeed;

    private bool isAiming = false;
    private Transform targetAimTransform;
    private Quaternion currentRotationQuaternion;

    public float centerOffsetY = 0;

    public bool IsAimed { get; private set; }

    private void Awake()
    {
        currentAimRotationSpeed = defaultAimRotationSpeed;
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            var modiffiedTargetPosition = new Vector3(targetAimTransform.position.x,
                targetAimTransform.position.y + centerOffsetY,
                targetAimTransform.position.z);

            var targetDirection = (modiffiedTargetPosition - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);

            foreach (var vertice in staticAxises)
            {
                switch (vertice)
                {
                    case AxisType.X:
                        targetRotation.x = 0;
                        break;
                    case AxisType.Y:
                        targetRotation.y = 0;
                        break;
                    case AxisType.Z:
                        targetRotation.z = 0;
                        break;
                }
            }

            var newRotationQuaternion = Quaternion.Slerp(currentRotationQuaternion, targetRotation, currentAimRotationSpeed);
            if (RestrictedAxisInBounds(GetAxisEulerAngel(restrictedAxis, newRotationQuaternion)))
            {
                if (isLocalRotation)
                {
                    transform.localRotation = newRotationQuaternion;
                }
                else
                {
                    transform.rotation = newRotationQuaternion;
                }

                currentRotationQuaternion = newRotationQuaternion;
            }

            if (isRaycasting)
            {
                if (Physics.Raycast(transform.position, transform.forward, shootDistance, hitTargetsMask))
                {
                    IsAimed = true;
                }
                else
                {
                    IsAimed = false;
                }
            }
        }
    }

    private bool RestrictedAxisInBounds(float nextAngle)
    {
        nextAngle = nextAngle > 180 ? nextAngle - 360 : nextAngle;

        if (nextAngle >= maxAngle || nextAngle <= minAngle)
        {
            return false;
        }

        return true;
    }

    private float GetAxisEulerAngel(AxisType axisType, Quaternion rotationQuaternion)
    {
        switch (axisType)
        {
            case AxisType.X:
                return rotationQuaternion.eulerAngles.x;
            case AxisType.Y:
                return rotationQuaternion.eulerAngles.y;
            case AxisType.Z:
                return rotationQuaternion.eulerAngles.z;
            default:
                return 0;
        }
    }

    private void OnSpeedChange(float speedMultiplier, bool isNormalSpeed)
    {
        if(isNormalSpeed)
        {
            currentAimRotationSpeed = defaultAimRotationSpeed;
            return;
        }
        currentAimRotationSpeed *= speedMultiplier;
    }

    private void OnEnable()
    {
        EventHandler.RegisterEvent<float, bool>(gameObject, "SpeedChange", OnSpeedChange);
    }

    private void OnDisable()
    {
        currentAimRotationSpeed = defaultAimRotationSpeed;
        isAiming = false;
        EventHandler.UnregisterEvent<float, bool>(gameObject, "SpeedChange", OnSpeedChange);
    }

    public void Aim(Transform targetTransform)
    {
        isAiming = true;
        targetAimTransform = targetTransform;
        currentRotationQuaternion = isLocalRotation ? transform.localRotation : transform.rotation;
    }

    public void TargetOutOfRange()
    {
        isAiming = false;
        targetAimTransform = null;
    }
}
