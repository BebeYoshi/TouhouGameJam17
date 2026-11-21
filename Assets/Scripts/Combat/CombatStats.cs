using System;
using System.Collections.Generic;
using UnityEngine;


public class CombatStats : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private string characterId;

    private CharacterData data;
    private readonly List<StatusEffect> effects = new List<StatusEffect>();
    private float currentHealth;
    private float currentBooze;
    private float boozeGainBonus;
    private float attackTimer;
    private float skillTimer;
    

    public CharacterData Data => data;
    public float CurrentHealth => currentHealth;
    public float CurrentBooze => currentBooze;
    public float BoozeGainMultiplier => Mathf.Max(0f, 1f + boozeGainBonus);

    public bool IsAlive => currentHealth > 0f;
    public bool IsUltReady => data != null && data.HasUlt && currentBooze >= data.MaxBooze;
    public bool IsStunned => HasEffect(StatusEffectType.Stun);
    public bool CanAct => IsAlive && !IsStunned;
    public bool IsAttackReady => CanAct && attackTimer <= 0f;
    public bool IsSkillReady => CanAct && skillTimer <= 0f;
    public float SkillCooldownRemaining => skillTimer;
     

    public event Action<float, float> BoozeChanged;
    public event Action UltReady;
    public event Action<float, float> HealthChanged;
    public event Action<CombatStats> Died;

    private void Awake()
    {
        if (database == null)
        {
            Debug.LogError($"{name}: database not assigned", this);
            enabled = false;
            return;
        }

        data = database.FindById(characterId);

        if (data == null)
        {
            Debug.LogError($"{name}: character '{characterId}' not found.", this);
            enabled = false;
            return;
        }

        currentHealth = data.MaxHealth;
        currentBooze = 0f;
        boozeGainBonus = 0f;
        attackTimer = 0f;
        skillTimer = 0f;
    } 

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        AddBooze(data.BoozePerSecond * deltaTime);
        attackTimer = Mathf.Max(0f, attackTimer - deltaTime);
        skillTimer = Mathf.Max(0f, skillTimer - deltaTime);
        TickEffects(deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || !IsAlive) 
        {
            return;
        }

        if (ApplyDamage(amount))
        {
            return;
        }

        AddBooze(data.BoozeOnHurt);
    }

    private bool ApplyDamage(float amount)
    {
        if (amount <= 0f || !IsAlive)
        {
            return false;
        }

        float previousHealth = currentHealth;

        int roundedDamage = Mathf.RoundToInt(amount);
        currentHealth = Mathf.Max(0f, currentHealth - roundedDamage);

        if (currentHealth != previousHealth)
        {
            HealthChanged?.Invoke(currentHealth, data.MaxHealth);
        }

        if (!IsAlive)
        {
            effects.Clear();
            Died?.Invoke(this);
            return true;
        }

        return false;
    }

    public bool BasicAttack(CombatStats target)
    {
        if (!IsAttackReady || !CanHit(target))
        {
            return false;
        }

        Attack(target, data.BasicMultiplier);
        attackTimer = 1f / data.AttackSpeed;
        return true;
    }

    public bool UseSkill(CombatStats target)
    {
        if (!IsSkillReady || !CanHit(target))
        {
            return false;
        }

        Attack(target, data.SkillMultiplier);
        skillTimer = data.SkillCooldown;
        return true;
    }

    public bool IsInRange(CombatStats target)
    {
        return target != null 
            && Vector2.Distance(transform.position, target.transform.position) <= data.AttackRange;
    }

    private void Attack(CombatStats target, float multiplier)
    {
        if (!CanHit(target))
        {
            return;
        }

        DealDamage(target, multiplier);
        AddBooze(data.BoozeOnHit);

        if (!target.IsAlive)
        {
            AddBooze(data.BoozeOnKill);
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || !IsAlive)
        {
            return;
        }

        float previousHealth = currentHealth;

        int roundedHeal = Mathf.RoundToInt(amount);
        currentHealth = Mathf.Min(data.MaxHealth, currentHealth + roundedHeal);

        if (currentHealth != previousHealth)
        {
            HealthChanged?.Invoke(currentHealth, data.MaxHealth); 
        }
    }

    public void ApplyEffect(StatusEffect effect)
    {
        if (effect == null || !IsAlive)
        {
            return;
        }

        effects.Add(effect);
    }

    public bool HasEffect(StatusEffectType type)
    {
        foreach (StatusEffect effect in effects)
        {
            if (effect.Type == type)
            {
                return true;
            }
        }

        return false;
    }

    private void TickEffects(float deltaTime)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            StatusEffect effect = effects[i];
            int ticks = effect.Advance(deltaTime);

            for (int t = 0; t < ticks; t++)
            {
                ApplyTick(effect);

                if (!IsAlive)
                {
                    return;
                }
            }

            if (effect.IsExpired)
            {
                effects.RemoveAt(i);
            }
        }
    }

    private void ApplyTick(StatusEffect effect)
    {
        switch (effect.Type)
        {
            case StatusEffectType.DamageOverTime:
                float damage = DamageCalculator.CalculateDamage(effect.SourceAttack, data.Defense, effect.Multiplier);

                if (ApplyDamage(damage) && effect.Source != null)
                {
                    effect.Source.AddBooze(effect.Source.Data.BoozeOnKill);
                }
                break;

            case StatusEffectType.HealOverTime:
                Heal(effect.SourceAttack * effect.Multiplier);
                break;
        }
    }

    public bool UseUlt(CombatStats target)
    {
        if (!CanAct || !CanHit(target))
        {
            return false;
        }

        if (!TryUseUlt())
        {
            return false;
        }

        DealDamage(target, data.UltMultiplier);
        return true;
    }

    private bool CanHit(CombatStats target)
    {
        return IsAlive && target != null && target.IsAlive;
    }

    private void DealDamage(CombatStats target, float multiplier)
    {
        float damage = DamageCalculator.CalculateDamage(data.Attack, target.Data.Defense, multiplier);
        target.TakeDamage(damage);
    }

    public void AddBooze(float amount)
    {
        if (!IsAlive) 
        {
            return;
        }

        if (amount > 0f && HasEffect(StatusEffectType.BoozeBlock))
        {
            return;
        }

        if (amount > 0f)
        {
            amount *= BoozeGainMultiplier;
        }

        float previousBooze = currentBooze;
        bool wasUltReady = IsUltReady;

        currentBooze = Mathf.Clamp(currentBooze + amount, 0f, data.MaxBooze);

        if (currentBooze != previousBooze)
        {
            BoozeChanged?.Invoke(currentBooze, data.MaxBooze);
        }

        if (!wasUltReady && IsUltReady)
        {
            UltReady?.Invoke();
        }
    }

    public void AddBoozeGainBonus(float bonus)
    {
        boozeGainBonus += bonus;
    }

    public void RemoveBoozeGainBonus(float bonus)
    {
        boozeGainBonus -= bonus;
    }

    private bool TryUseUlt()
    {
        if (!IsUltReady)
        {
            return false;
        }

        currentBooze = 0f;
        BoozeChanged?.Invoke(currentBooze, data.MaxBooze);
        return true;
    }
    
}
