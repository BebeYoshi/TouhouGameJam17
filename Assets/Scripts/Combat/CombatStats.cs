using UnityEngine;
using System;

public class CombatStats : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private string characterId;

    private CharacterData data;
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
    public bool IsAttackReady => IsAlive && attackTimer <= 0f;
    public bool IsSkillReady => IsAlive && skillTimer <= 0f;
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
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || !IsAlive) 
        {
            return;
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
            Died?.Invoke(this);
            return;
        }

        AddBooze(data.BoozeOnHurt);
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

    public bool UseUlt(CombatStats target)
    {
        if (!CanHit(target))
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
