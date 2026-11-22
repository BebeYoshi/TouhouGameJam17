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

    public static CombatStats GetNearestEnemy(CombatStats unit)
    {
        List<CombatStats> enemies = GetNearestEnemies(unit, 1);
        return enemies.Count > 0 ? enemies[0] : null;
    }

    public static List<CombatStats> GetNearestEnemies(CombatStats unit, int count)
    {
        if (count <= 0)
        {
            return new List<CombatStats>();
        }

        List<CombatStats> enemies = GetEnemiesOf(unit);
        enemies.Sort((a, b) => DistanceTo(unit, a).CompareTo(DistanceTo(unit, b)));

        if (enemies.Count > count)
        {
            enemies.RemoveRange(count, enemies.Count - count);
        }

        return enemies;
    }

    public static CombatStats GetFarthestEnemy(CombatStats unit)
    {
        CombatStats farthest = null;
        float farthestDistance = -1f;

        foreach (CombatStats enemy in GetEnemiesOf(unit))
        {
            float distance = DistanceTo(unit, enemy);

            if (distance > farthestDistance)
            {
                farthest = enemy;
                farthestDistance = distance;
            }
        }

        return farthest;
    }

    public static CombatStats GetLowestHealthAlly(CombatStats unit)
    {
        CombatStats lowest = null;
        float lowestFraction = float.MaxValue;

        foreach (CombatStats ally in GetAlliesOf(unit, true))
        {
            float maxHealth = ally.MaxHealth;
            float fraction = maxHealth > 0f ? ally.CurrentHealth / maxHealth : 1f;

            if (fraction < lowestFraction)
            {
                lowest = ally;
                lowestFraction = fraction;
            }
        }

        return lowest;
    }

    public static CombatStats GetAllyClosestToUlt(CombatStats unit)
    {
        CombatStats closest = null;
        float highestFraction = -1f;

        foreach (CombatStats ally in GetAlliesOf(unit, false))
        {
            if (!ally.HasUltAvailable || ally.IsUltReady)
            {
                continue;
            }

            if (ally.BoozeFraction > highestFraction)
            {
                closest = ally;
                highestFraction = ally.BoozeFraction;
            }
        }

        return closest;
    }

    public static CombatStats GetMostRecentlyDeadAlly(CombatStats unit)
    {
        CombatStats mostRecent = null;

        foreach (CombatStats other in units)
        {
            if (other.Side != unit.Side || !other.CanBeRevived)
            {
                continue;
            }

            if (mostRecent == null || other.DeathTime > mostRecent.DeathTime)
            {
                mostRecent = other;
            }
        }

        return mostRecent;
    }

    private static float DistanceTo(CombatStats from, CombatStats to)
    {
        return Vector2.Distance(from.transform.position, to.transform.position);
    }
}
