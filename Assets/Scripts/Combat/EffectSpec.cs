using System;
using UnityEngine;

[Serializable]
public class EffectSpec
{
    [SerializeField] private bool apply = false;
    [SerializeField] private StatusEffectType type = StatusEffectType.DamageOverTime;
    [SerializeField, Min(0f)] private float multiplier = 0.2f;
    [SerializeField, Min(0f)] private float duration = 3f;
    [SerializeField] private DotKind dotKind = DotKind.None;

    public StatusEffect Create(CombatStats source)
    {
        if (!apply)
        {
            return null;
        }

        switch (type)
        {
            case StatusEffectType.Stun:
                return StatusEffect.Stun(duration);
            
            case StatusEffectType.BoozeBlock:
                return StatusEffect.BoozeBlock(duration);

            case StatusEffectType.DamageOverTime:
                return StatusEffect.DamageOverTime(source, multiplier, duration, dotKind);
            
            case StatusEffectType.HealOverTime:
                return StatusEffect.HealOverTime(source, multiplier, duration);

            case StatusEffectType.Blind:
                return StatusEffect.Blind(multiplier, duration);

            case StatusEffectType.DamageBuff:
                return StatusEffect.DamageBuff(multiplier, duration);

            case StatusEffectType.BoozeGainBuff:
                return StatusEffect.BoozeGainBuff(multiplier, duration);
            
            case StatusEffectType.HealReduction:
                return StatusEffect.HealReduction(multiplier, duration);
            
            default:
                Debug.LogWarning($"Effect type '{type}' is not supported yet.");
                return null;
        }
    }
}
