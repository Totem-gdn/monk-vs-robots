using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDebuff : ScriptableObject
{

    [SerializeField] protected DebuffDurationType debuffDurationType;
    [Min(0)]
    [SerializeField] protected float debuffDuration;
    [Min(0)]
    [SerializeField] protected float resistanceDuration;
    [SerializeField] protected Sprite debuffIcon;

    [NonSerialized] public DebuffsManager debuffTarget;

    public float ResistanceDuration
    {
        get
        {
            return resistanceDuration;
        }
    }

    public DebuffDurationType DebuffDurationType
    {
        get
        {
            return debuffDurationType;
        }
    }

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }
    }

    public Sprite DebuffIcon
    {
        get
        {
            return debuffIcon;
        }
    }

    public abstract bool Initialize(BaseDebuff baseDebuff);
    public abstract void Apply();
    public abstract void Remove();
}
