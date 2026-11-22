using System.Collections.Generic;
using UnityEngine;

public static class BattleRoster
{
    private static readonly List<CombatStats> units = new List<CombatStats>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        units.Clear();
    }

    public static void Register(CombatStats unit)
    {
        if (unit != null && !units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public static void Unregister(CombatStats unit)
    {
        units.Remove(unit);
    }

    public static List<CombatStats> GetEnemiesOf(CombatStats unit)
    {
        List<CombatStats> enemies = new List<CombatStats>();

        foreach (CombatStats other in units)
        {
            if (other.IsAlive && other.Side != unit.Side)
            {
                enemies.Add(other);
            }
        }

        return enemies;
    }

    public static List<CombatStats> GetAlliesOf(CombatStats unit, bool includeSelf)
    {
        List<CombatStats> allies = new List<CombatStats>();

        foreach (CombatStats other in units)
        {
            if (other.IsAlive && other.Side == unit.Side && (includeSelf || other != unit))
            {
                allies.Add(other);
            }
        }

        return allies;
    }
}
