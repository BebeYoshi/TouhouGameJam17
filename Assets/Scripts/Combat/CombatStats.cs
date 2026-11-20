using UnityEngine;

public class CombatStats : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private string characterId;

    private CharacterData data;
    private float currentHealth;
    private float currentBooze;

    public CharacterData Data => data;
    public float CurrentHealth => currentHealth;
    public float CurrentBooze => currentBooze;

    public bool IsAlive => currentHealth > 0f;
    public bool IsUltReady => currentBooze >= data.MaxBooze;

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
    } 

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || !IsAlive) 
        {
            return;
        }

        int roundedDamage = Mathf.RoundToInt(amount);
        currentHealth = Mathf.Max(0f, currentHealth - roundedDamage);
    }

    public void AddBooze(float amount)
    {
        if (!IsAlive) 
        {
            return;
        }

        currentBooze = Mathf.Clamp(currentBooze + amount, 0f, data.MaxBooze);
    }

    public bool TryUseUlt()
    {
        if (!IsUltReady)
        {
            return false;
        }

        currentBooze = 0f;
        return true;
    }
    
}
