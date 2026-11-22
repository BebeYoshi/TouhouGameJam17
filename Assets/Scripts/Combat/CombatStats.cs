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
    [SerializeField] private BattleSide side = BattleSide.Player;

    private CharacterData data;
    private readonly List<StatusEffect> effects = new List<StatusEffect>();
    private float currentHealth;
    private float currentBooze;
    private float attackTimer;
    private float skillTimer;
    private float levelMultiplier = 1f;
    private float boozeLossOnHurtPercent;
    private float boozeLossOnHurt;
    private float maxBooze;
    private bool ultLocked;
    private bool hasBeenRevived;
    private float deathTime;
    

    public CharacterData Data => data;
    public float CurrentHealth => currentHealth;
    public float CurrentBooze => currentBooze;
    public float MaxBooze => maxBooze;
    public float BoozeFraction => data != null && maxBooze > 0f ? currentBooze / maxBooze : 0f;
    public float BoozeGainMultiplier => Mathf.Max(0f, 1f + GetPassiveBoozeGainBonus() + GetStackedSum(StatusEffectType.BoozeGainBuff));
    public int Level => level;
    public BattleSide Side => side;
    public bool IsInitialized => data != null;
    public float Attack => data.Attack * levelMultiplier * (1f + GetAttackBonus());
    public float Defense => data.Defense * levelMultiplier * (1f + GetDefenseBonus());
    public float MaxHealth => Mathf.Round(data.MaxHealth * levelMultiplier);
    public float MissChance => GetStackedStrength(StatusEffectType.Blind, settings.MaxBlindChance);
    public float HealReduction => GetStackedStrength(StatusEffectType.HealReduction, settings.MaxHealReduction);

    public bool IsAlive => currentHealth > 0f;
    public bool CanBeRevived => data != null && !IsAlive && !hasBeenRevived;
    public float DeathTime => deathTime;
    public bool HasUltAvailable => data != null && data.HasUlt && !ultLocked;
    public bool IsUltReady => data != null && data.HasUlt && !ultLocked && currentBooze >= maxBooze;
    public bool IsStunned => HasEffect(StatusEffectType.Stun);
    public bool CanAct => IsAlive && !IsStunned;
    public bool IsAttackReady => CanAct && attackTimer <= 0f;
    public bool IsSkillReady => CanAct && data.Skill != null && skillTimer <= 0f;
    public float SkillCooldownRemaining => skillTimer;
    private PassiveAbility Passive => data != null ? data.Passive : null;
     

    public event Action<float, float> BoozeChanged;
    public event Action UltReady;
    public event Action<float, float> HealthChanged;
    public event Action<CombatStats> Died;
    public event Action Unleashed;
    public event Action<CombatStats> Revived;

    private void Awake()
    {
        if (string.IsNullOrEmpty(characterId))
        {
            enabled = false;
            return;
        }

        Initialize(characterId, level, side);
    }

    private void OnEnable()
    {
        BattleRoster.Register(this);
    }

    private void OnDisable()
    {
        BattleRoster.Unregister(this);
    }

    public bool Initialize(string id, int startLevel, BattleSide startSide)
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
        side = startSide;
        level =settings.ClampLevel(startLevel);
        levelMultiplier = settings.GetStatsMultiplier(level);

        currentHealth = MaxHealth;
        maxBooze = data.MaxBooze;
        currentBooze = 0f;
        attackTimer = 0f;
        skillTimer = 0f;
        ultLocked = false;
        hasBeenRevived = false;
        deathTime = 0f;
        boozeLossOnHurt = 0f;
        boozeLossOnHurtPercent = 0f;
        effects.Clear();

        if (Passive != null)
        {
            Passive.OnInitialized(this);
        }

        enabled = true;
        return true;
    }

    private float GetAttackBonus()
    {
        return Passive != null ? Passive.GetAttackBonus(this) : 0f;
    }

    private float GetDefenseBonus()
    {
        return Passive != null ? Passive.GetDefenseBonus(this) : 0f;
    }

    private float GetDamageBonus(CombatStats target)
    {
        float passiveBonus = Passive != null ? Passive.GetDamageBonus(this, target) : 0f;
        return passiveBonus + GetStackedSum(StatusEffectType.DamageBuff);
    }

    private float GetPassiveBoozeGainBonus()
    {
        return Passive != null ? Passive.GetBoozeGainBonus(this) : 0f;
    }

    private float GetStackedSum(StatusEffectType type)
    {
        float sum = 0f;

        foreach (StatusEffect effect in effects)
        {
            if (effect.Type == type)
            {
                sum += effect.Multiplier;
            }
        }

        return sum;
    }
    
    private float GetStackedStrength(StatusEffectType type, float cap)
    {
        float remaining = 1f;

        foreach (StatusEffect effect in effects)
        {
            if (effect.Type == type)
            {
                remaining *= 1f - Mathf.Clamp01(effect.Multiplier);
            }
        }

        return Mathf.Min(cap, 1f - remaining);
    }

    private bool TryMiss()
    {
        float chance = MissChance;
        return chance > 0f && UnityEngine.Random.value < chance;
    }

    private bool TryEvade()
    {
        if (IsStunned)
        {
            return false;
        }

        float chance = Passive != null ? Passive.GetEvasionChance(this) : 0f;
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

        int roundedDamage = Mathf.Max(1, Mathf.RoundToInt(amount));
        currentHealth = Mathf.Max(0f, currentHealth - roundedDamage);

        if (currentHealth != previousHealth)
        {
            HealthChanged?.Invoke(currentHealth, MaxHealth);
        }

        if (!IsAlive)
        {
            deathTime = Time.time;
            effects.Clear();

            if (Passive != null)
            {
                Passive.OnDied(this);
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

        if (data.BasicAttackAbility != null)
        {
            if (!data.BasicAttackAbility.Execute(this, target, AttackKind.Basic))
            {
                return false;
            }
        }
        else
        {
            PerformAttack(target, data.BasicMultiplier, AttackKind.Basic);
        }

        attackTimer = 1f / data.AttackSpeed;
        return true;
    }

    public bool UseSkill(CombatStats target)
    {
        if (!IsSkillReady || data.Skill == null)
        {
            return false;
        }

        if (!data.Skill.Execute(this, target, AttackKind.Skill))
        {
            return false;
        }

        skillTimer = data.SkillCooldown;
        return true;
    }

    public bool IsInRange(CombatStats target)
    {
        return target != null 
            && Vector2.Distance(transform.position, target.transform.position) <= data.AttackRange;
    }

    public bool PerformAttack(CombatStats target, float multiplier, AttackKind kind)
    {
        if (target == null || !target.IsAlive)
        {
            return false;
        }

        bool isUlt = kind == AttackKind.Ult;

        if (!DealDamage(target, multiplier, !isUlt))
        {
            return false;
        }

        if (isUlt)
        {
            return true;
        }

        AddBooze(data.BoozeOnHit);

        if (!target.IsAlive)
        {
            RewardKill();
        }

        return true;
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || !IsAlive)
        {
            return;
        }
        
        amount *= 1f - HealReduction;
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

                if (effect.Type == StatusEffectType.Revived)
                {
                    ApplyDamage(currentHealth);
                    return;
                }
            }
        }
    }

    public bool Revive(float healthFraction, float duration)
    {
        if (!CanBeRevived)
        {
            return false;
        }

        hasBeenRevived = true;
        currentHealth = Mathf.Max(1f, Mathf.Round(MaxHealth * Mathf.Clamp01(healthFraction)));
        effects.Add(StatusEffect.Revived(duration));

        HealthChanged?.Invoke(currentHealth, MaxHealth);
        Revived?.Invoke(this);
        return true;
    }

    private void ApplyTick(StatusEffect effect)
    {
        switch (effect.Type)
        {
            case StatusEffectType.DamageOverTime:
                float damage = DamageCalculator.CalculateDamage(effect.SourceAttack, Defense, effect.Multiplier);

                if (ApplyDamage(damage) && effect.Source != null)
                {
                    effect.Source.RewardKill();
                }
                break;

            case StatusEffectType.HealOverTime:
                Heal(effect.SourceAttack * effect.Multiplier);
                break;
        }
    }

    public bool UseUlt(CombatStats target)
    {
        if (!CanAct || !IsUltReady || data.UltAbility == null)
        {
            return false;
        }

        if (!data.UltAbility.Execute(this, target, AttackKind.Ult))
        {
            return false;
        }

        TryUseUlt();
        return true;
    }

    private bool CanHit(CombatStats target)
    {
        return IsAlive && target != null && target.IsAlive;
    }

    private bool DealDamage(CombatStats target, float multiplier, bool canBeEvaded)
    {
        if (canBeEvaded && (TryMiss() || target.TryEvade()))
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

        SetBooze(currentBooze + amount);
    }

    private void SetBooze(float value)
    {
        float previousBooze = currentBooze;
        bool wasUltReady = IsUltReady;

        currentBooze = Mathf.Clamp(value, 0f, maxBooze);

        if (currentBooze != previousBooze)
        {
            BoozeChanged?.Invoke(currentBooze, maxBooze);
        }

        if (!wasUltReady && IsUltReady)
        {
            UltReady?.Invoke();
        }
    }

    private void RewardKill()
    {
        AddBooze(data.BoozeOnKill);
    }

    private bool TryUseUlt()
    {
        if (!IsUltReady)
        {
            return false;
        }

        SetBooze(0f);
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
