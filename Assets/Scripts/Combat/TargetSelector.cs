using UnityEngine;
using System.Collections.Generic;

public static class TargetSelector
{
    public static List<CombatStats> Select(TargetMode mode, CombatStats owner, CombatStats currentTarget, int count)
    {
        List<CombatStats> targets = new List<CombatStats>();

        switch (mode)
        {
            case TargetMode.CurrentOrNearest:
                bool currentIsValid = currentTarget != null && currentTarget.IsAlive && currentTarget.Side != owner.Side;
                AddIfFound(targets, currentIsValid ? currentTarget : BattleRoster.GetNearestEnemy(owner));
                break;

            case TargetMode.NearestEnemies:
                targets.AddRange(BattleRoster.GetNearestEnemies(owner, count));
                break;

            case TargetMode.FarthestEnemy:
                AddIfFound(targets, BattleRoster.GetFarthestEnemy(owner));
                break;

            case TargetMode.LowestHealthAlly:
                AddIfFound(targets, BattleRoster.GetLowestHealthAlly(owner));
                break;

            case TargetMode.AllyClosestToUlt:
                AddIfFound(targets, BattleRoster.GetAllyClosestToUlt(owner));
                break;

            case TargetMode.DeadAlly:
                AddIfFound(targets, BattleRoster.GetMostRecentlyDeadAlly(owner));
                break;
        }

        return targets;
    }

    private static void AddIfFound(List<CombatStats> targets, CombatStats unit)
    {
        if (unit != null)
        {
            targets.Add(unit);
        }
    }
}


