using UnityEngine;

[CreateAssetMenu(fileName = "CombatSettings", menuName = "Scriptable Objects/CombatSettings")]
public class CombatSettings : ScriptableObject
{
    [Header("Leveling")]
    [SerializeField, Min(1)] private int maxLevel = 40;
    [SerializeField, Min(1f)] private float statGrowthPerLevel = 1.04f;

    [Header("Status Effects")]
    [SerializeField, Range(0f, 1f)] private float maxBlindChance = 0.2f;
    [SerializeField, Range(0f, 1f)] private float maxHealReduction = 0.75f;

    public int MaxLevel => maxLevel;
    public float StatGrowthPerLevel => statGrowthPerLevel;

    public float MaxBlindChance => maxBlindChance;
    public float MaxHealReduction => maxHealReduction;

    public int ClampLevel(int level)
    {
        return Mathf.Clamp(level, 1, maxLevel);
    }

    public float GetStatsMultiplier(int level)
    {
        return Mathf.Pow(statGrowthPerLevel, ClampLevel(level) - 1);
    }
}
