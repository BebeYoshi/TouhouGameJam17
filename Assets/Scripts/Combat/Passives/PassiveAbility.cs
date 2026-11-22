using UnityEngine;

public abstract class PassiveAbility : ScriptableObject
{
    public virtual float GetAttackBonus(CombatStats owner)
    {
        return 0f;
    }

    public virtual float GetDefenseBonus(CombatStats owner)
    {
        return 0f;
    }

    public virtual float GetDamageBonus(CombatStats owner, CombatStats target)
    {
        return 0f;
    }

    public virtual float GetBoozeGainBonus(CombatStats owner)
    {
        return 0f;
    }

    public virtual float GetEvasionChance(CombatStats owner)
    {
        return 0f;
    }

    public virtual void OnInitialized(CombatStats owner){}
    public virtual void OnDied(CombatStats owner){}
}
