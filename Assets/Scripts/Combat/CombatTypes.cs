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

public enum Species
{
    Youkai,
    Doll,
    Human
}

public enum BattleSide
{
    Player,
    Enemy
}

public enum HitboxTargets
{
    Enemies, 
    Allies
}

public enum HitboxSpawn
{
    Owner,
    Target
}

public enum MultiplierSource
{
    Basic,
    Skill,
    Custom
}