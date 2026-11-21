using UnityEngine;

public enum CharacterRole
{
    Tank,
    Support,
    DPS
}

public enum AttackType
{
    Melee,
    Ranged
}

public enum StatusEffectType
{
    Stun,            
    DamageOverTime,
    HealOverTime,
    BoozeBlock,
    Blind,           
    ProjectileShield,
    DamageBuff,      
    BoozeGainBuff,
    HealReduction,   
    Charm,           
    Revived
}

public enum DotKind
{
    None,
    Fire,
    Bleed
}