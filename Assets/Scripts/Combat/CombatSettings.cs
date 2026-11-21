using UnityEngine;

[CreateAssetMenu(fileName = "CombatSettings", menuName = "Scriptable Objects/CombatSettings")]
public class CombatSettings : ScriptableObject
{
    [Header("Leveling")]
    [SerializeField, Min(1)] private int maxLevel = 40;
    [SerializeField, Min(1f)] private float statGrowthPerLevel = 1.04f;

    public int MaxLevel => maxLevel;
    public float StatGrowthPerLevel => statGrowthPerLevel;

    public int ClampLevel(int level)
    {
        return Mathf.Clamp(level, 1, maxLevel);
    }

    public float GetStatsMultiplier(int level)
    {
        return Mathf.Pow(statGrowthPerLevel, ClampLevel(level) - 1);
    }
}
