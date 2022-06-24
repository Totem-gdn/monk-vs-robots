using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private bool isMultipleAreaHit;
    [SerializeField] private DamageProcessor damageProcessor;

    public bool IsMultipleAreaHit
    {
        get
        {
            return isMultipleAreaHit;
        }
    }

    public DamageProcessor DamageProcessor
    {
        get
        {
            return damageProcessor;
        }
    }
}
