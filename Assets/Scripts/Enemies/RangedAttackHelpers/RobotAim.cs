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

    public float centerOffsetY = 0;

    public bool IsAimed { get; private set; }

    public IEnumerator Aim(Transform targetTransform)
    {
        while (true)
        {
            var modiffiedTargetPosition = new Vector3(targetTransform.position.x,
                targetTransform.position.y + centerOffsetY,
                targetTransform.position.z);

            var targetDirection = (modiffiedTargetPosition - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            var currentRotation = isLocalRotation ? transform.localRotation : transform.rotation;

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

            var newRotationQuaternion = Quaternion.Slerp(currentRotation, targetRotation, 0.1f);
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
            }

            if(isRaycasting)
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

            yield return null;
        }
    }

    private bool RestrictedAxisInBounds(float nextAngle)
    {
        nextAngle = nextAngle > 180 ? nextAngle - 360 : nextAngle;

        if(nextAngle >= maxAngle || nextAngle <= minAngle)
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
}
