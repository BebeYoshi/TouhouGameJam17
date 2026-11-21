using UnityEngine;

public class StatusEffect
{
    private const float TickInterval = 1f;

    public StatusEffectType Type {get; }
    public DotKind Kind { get; }
    public CombatStats Source { get; }
    public float SourceAttack { get; }
    public float Multiplier { get; }
    public float Remaining { get; private set; }

    private float tickTimer;

    public bool IsExpired => Remaining <= 0f;

    private StatusEffect(StatusEffectType type, float duration, CombatStats source, float multiplier, DotKind kind)
    {
        Type = type;
        Kind = kind;
        Source = source;
        SourceAttack = source !=null ? source.Attack : 0f;
        Multiplier = multiplier;
        Remaining = duration;
        tickTimer = TickInterval;
    }

    public static StatusEffect Stun(float duration)
    {
        return new StatusEffect(StatusEffectType.Stun, duration, null, 0f, DotKind.None);
    }

    public static StatusEffect BoozeBlock(float duration)
    {
        return new StatusEffect(StatusEffectType.BoozeBlock, duration, null, 0f, DotKind.None);
    }

    public static StatusEffect DamageOverTime(CombatStats source, float multiplier, float duration, DotKind kind)
    {
        return new StatusEffect(StatusEffectType.DamageOverTime, duration, source, multiplier, kind);
    }

    public static StatusEffect HealOverTime(CombatStats source, float multiplier, float duration)
    {
        return new StatusEffect(StatusEffectType.HealOverTime, duration, source, multiplier, DotKind.None);
    }

    public int Advance(float deltaTime)
    {
        Remaining -= deltaTime;
        tickTimer -= deltaTime;

        int ticks = 0;

        while (tickTimer <= 0f)
        {
            ticks++;
            tickTimer += TickInterval;
        }

        return ticks;
    }
}
