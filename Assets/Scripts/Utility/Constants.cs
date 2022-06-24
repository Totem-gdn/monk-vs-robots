using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int PASSWORD_MIN_LENGTH = 4;

    public const int PICKUP_LAYER_INDEX = 20;
    public const int RANGED_SPEAR_LAYER_INDEX = 22;
    public const int MELEE_SPEAR_LAYER_INDEX = 23;

    public const float VOLUME_RECALCULATION_COEF = 100.0f;
    public const float VOLUME_DEFAULT_VALUE = 1;

    public const float RESPAWN_SCREEN_FADE_TIME = 1.5f;
    public const int HIT_COLLIDER_LAYER_INDEX = 24;

    public const string HEALTH_ATTRIBUTE_NAME = "Health";
    public const float MAX_HEALTH_MULTIPLIER = 1.5f;
    public const float CRIT_DAMAGE_CHANCE = 15;
    public const int NON_CRITICAL_ATTACKS_LIMIT = 4;
    public const float CRITICAL_DAMAGE_MULTIPLIER = 1.5f;
    public const float DODGE_CHANCE = 15;
    public const int NOT_DODGED_ATTACKS_LIMIT = 4;
    public const float CHARACTER_SPEED_VALUE = 1.5f;

    public const string WALK_ANIMATION_TRIGGER = "Walk";
    public const string IDLE_ANIMATION_TRIGGER = "Idle";
    public const string MELEE_ATTACK_ANIMATION_TRIGGER = "MeleeAttack";
    public const string DEATH_ANIMATION_TRIGGER = "Death";
    public const string VICTORY_ANIMATION_TRIGGER = "Victory";
    public const string SHOOT_ANIMATION_TRIGGER = "Shoot";
    public const string ROTATE_ANIMATION_TRIGGER = "Rotate";

    public const string FATWIMP_AVATAR_INFO = "Max HP +50%";
    public const string FATMUSCULAR_AVATAR_INFO = "15% chance to deal 150% critical damage";
    public const string THINWIMP_AVATAR_INFO = "15% chance to dodge attacks";
    public const string THINMUSCULAR_AVATAR_INFO = "Speed +50%";
}
