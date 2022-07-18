using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VolumeType
{
    UI,
    Music,
    Effects
}

public enum MusicType
{
    Menu,
    FireArena,
    WaterArena,
    EarthArena,
    AirArena
}

public enum SoundType
{
    Default,
    StaffSwing,
    TakeDamage,
    CannonShoot,
    RingAoe,
    BombExplosion,
    BossAoe,
    TankDrive
}

public enum CharacterType
{
    FatWimp,
    FatMuscular,
    ThinWimp,
    ThinMuscular
}

public enum DistanceTransitionType
{
    InRange,
    OutOfRange
}

public enum HpComparerTransitionType
{
    Above,
    Below,
    Equals
}

public enum GameEndedType
{
    Win,
    Lose
}

public enum AxisType
{ 
    X,
    Y,
    Z
}

public enum TargetType
{
    Shrine,
    Player,
    CurrentTarget
}

public enum DebuffDurationType
{
    Permanent,
    Temporary
}
