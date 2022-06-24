using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineDistanceTransition : PlayerDistanceTransition
{
    protected override void InitializeTargets()
    {
        selfTransform = stateMachine.transform;
        selfCollider = stateMachine.mainCollider;
        targetTransform = ShrineController.Instance.transform;
        targetCollider = ShrineController.Instance.shrineCollider;
    }
}
