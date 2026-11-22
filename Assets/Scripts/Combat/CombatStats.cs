using System;
using System.Collections.Generic;
using UnityEngine;


public class CombatStats : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private string characterId;
    [SerializeField] private CombatSettings settings;
    [SerializeField, Min(1)] private int level = 1;

    private CharacterData data;
    private readonly List<StatusEffect> effects = new List<StatusEffect>();
    private float currentHealth;
    private float currentBooze;
    private float boozeGainBonus;
    private float attackTimer;
    private float skillTimer;
    private float levelMultiplier = 1f;
    private float boozeLossOnHurtPercent;
    private float boozeLossOnHurt;
    private float maxBooze;
    private bool ultLocked;
    

    public CharacterData Data => data;
    public float CurrentHealth => currentHealth;
    public float CurrentBooze => currentBooze;
    public float MaxBooze => maxBooze;
    public float BoozeFraction => data != null && maxBooze > 0f ? currentBooze / maxBooze : 0f;
    public float BoozeGainMultiplier => Mathf.Max(0f, 1f + boozeGainBonus + GetPassiveBoozeGainBonus());
    public int Level => level;
    public bool IsInitialized => data != null;
    public float Attack => data.Attack * levelMultiplier * (1f + GetAttackBonus());
    public float Defense => data.Defense * levelMultiplier * (1f + GetDefenseBonus());
    public float MaxHealth => Mathf.Round(data.MaxHealth * levelMultiplier);

    public bool IsAlive => currentHealth > 0f;
    public bool IsUltReady => data != null && data.HasUlt && !ultLocked && currentBooze >= maxBooze;
    public bool IsStunned => HasEffect(StatusEffectType.Stun);
    public bool CanAct => IsAlive && !IsStunned;
    public bool IsAttackReady => CanAct && attackTimer <= 0f;
    public bool IsSkillReady => CanAct && skillTimer <= 0f;
    public float SkillCooldownRemaining => skillTimer;
     

    public event Action<float, float> BoozeChanged;
    public event Action UltReady;
    public event Action<float, float> HealthChanged;
    public event Action<CombatStats> Died;
    public event Action Unleashed;

    private void Awake()
    {
        if (string.IsNullOrEmpty(characterId))
        {
            enabled = false;
            return;
        }

        Initialize(characterId, level);
    } 

    public bool Initialize(string id, int startLevel)
    {
        if (database == null || settings == null)
        {
            Debug.LogError($"{name}: databse or settings not assigned", this);
            enabled = false;
            return false;
        }

        CharacterData found = database.FindById(id);

        if (found == null)
        {
            Debug.LogError($"{name}: character '{id}' not found.", this);
            enabled = false;
            return false;
        }

        data = found;
        characterId = id;
        level = settings.ClampLevel(startLevel);
        levelMultiplier = settings.GetStatsMultiplier(level);

        currentHealth = MaxHealth;
        maxBooze = data.MaxBooze;
        currentBooze = 0f;
        boozeGainBonus = 0f;
        attackTimer = 0f;
        skillTimer = 0f;
        ultLocked = false;
        boozeLossOnHurt = 0f;
        boozeLossOnHurtPercent = 0f;
        effects.Clear();

        if (data.Passive != null)
        {
            data.Passive.OnInitialized(this);
        }

        enabled = true;
        return true;
    }

    private float GetAttackBonus()
    {
        return data.Passive != null ? data.Passive.GetAttackBonus(this) : 0f;
    }

    private float GetDefenseBonus()
    {
        return data.Passive != null ? data.Passive.GetDefenseBonus(this) : 0f;
    }

    private float GetDamageBonus(CombatStats target)
    {
        return data.Passive != null ? data.Passive.GetDamageBonus(this, target) : 0f;
    }

    private float GetPassiveBoozeGainBonus()
    {
        return data != null && data.Passive != null ? data.Passive.GetBoozeGainBonus(this) : 0f;
    }

    private bool TryEvade()
    {
        if (IsStunned)
        {
            return false;
        }

        float chance = data.Passive != null ? data.Passive.GetEvasionChance(this) : 0f;
        return chance > 0f && UnityEngine.Random.value < chance;
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

        float boozeLoss = boozeLossOnHurt + currentBooze * boozeLossOnHurtPercent;

        if (boozeLoss > 0f)
        {
            AddBooze(-boozeLoss);
        }
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
            HealthChanged?.Invoke(currentHealth, MaxHealth);
        }

        if (!IsAlive)
        {
            effects.Clear();

            if (data.Passive != null)
            {
                data.Passive.OnDied(this);
            }

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

        PerformAttack(target, data.BasicMultiplier);
        attackTimer = 1f / data.AttackSpeed;
        return true;
    }

    public bool UseSkill(CombatStats target)
    {
        if (!IsSkillReady || !CanHit(target))
        {
            return false;
        }

        PerformAttack(target, data.SkillMultiplier);
        skillTimer = data.SkillCooldown;
        return true;
    }

    public bool IsInRange(CombatStats target)
    {
        return target != null 
            && Vector2.Distance(transform.position, target.transform.position) <= data.AttackRange;
    }

    private void PerformAttack(CombatStats target, float multiplier)
    {
        if (!CanHit(target))
        {
            return;
        }

        if (!DealDamage(target, multiplier, true))
        {
            return;
        }

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
        currentHealth = Mathf.Min(MaxHealth, currentHealth + roundedHeal);

        if (currentHealth != previousHealth)
        {
            HealthChanged?.Invoke(currentHealth, MaxHealth); 
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
                float damage = DamageCalculator.CalculateDamage(effect.SourceAttack, Defense, effect.Multiplier);

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

        DealDamage(target, data.UltMultiplier, false);
        return true;
    }

    private bool CanHit(CombatStats target)
    {
        return IsAlive && target != null && target.IsAlive;
    }

    private bool DealDamage(CombatStats target, float multiplier, bool canBeEvaded)
    {
        if (canBeEvaded && target.TryEvade())
        {
            return false;
        }

        float damage = DamageCalculator.CalculateDamage(Attack, target.Defense, multiplier * (1f + GetDamageBonus(target)));
        target.TakeDamage(damage);
        return true;
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

        currentBooze = Mathf.Clamp(currentBooze + amount, 0f, maxBooze);

        if (currentBooze != previousBooze)
        {
            BoozeChanged?.Invoke(currentBooze, maxBooze);
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
        BoozeChanged?.Invoke(currentBooze, maxBooze);
        return true;
    }

    public void Unleash(float newMaxBooze, float lossOnHurt, float lossOnHurtPercent)
    {
        if (!IsAlive)
        {
            return;
        }

        maxBooze = Mathf.Max(1f, newMaxBooze);
        ultLocked = true;
        boozeLossOnHurt = lossOnHurt;
        boozeLossOnHurtPercent = lossOnHurtPercent;

        currentBooze = 0f;
        BoozeChanged?.Invoke(currentBooze, maxBooze);
        Unleashed?.Invoke();
    }
    
}
