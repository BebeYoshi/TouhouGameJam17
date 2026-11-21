using UnityEngine;

public class CombatStats : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private string characterId;

    private CharacterData data;
    private float currentHealth;
    private float currentBooze;
    private float boozeGainBonus;

    public CharacterData Data => data;
    public float CurrentHealth => currentHealth;
    public float CurrentBooze => currentBooze;
    public float BoozeGainMultiplier => Mathf.Max(0f, 1f + boozeGainBonus);

    public bool IsAlive => currentHealth > 0f;
    public bool IsUltReady => data != null && currentBooze >= data.MaxBooze;
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
    } 

    private void Update()
    {
        AddBooze(data.BoozePerSecond * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || !IsAlive) 
        {
            return;
        }

        int roundedDamage = Mathf.RoundToInt(amount);
        currentHealth = Mathf.Max(0f, currentHealth - roundedDamage);

        AddBooze(data.BoozeOnHurt);
    }

    public void Attack(CombatStats target, float multiplier)
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

        currentBooze = Mathf.Clamp(currentBooze + amount, 0f, data.MaxBooze);
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
        return true;
    }
    
}
