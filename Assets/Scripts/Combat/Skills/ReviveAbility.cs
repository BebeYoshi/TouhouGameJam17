using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReviveAbility", menuName = "Scriptable Objects/Abilities/Revive Ability")]
public class ReviveAbility : CombatAbility
{
    [SerializeField, Range(0f, 1f)] private float healthFraction = 1f;
    [SerializeField, Min(0.1f)] private float duration = 10f;

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        List<CombatStats> targets = TargetSelector.Select(TargetMode.DeadAlly, owner, target, 1);

        if (targets.Count == 0)
        {
            return false;
        }

        return targets[0].Revive(healthFraction, duration);
    }
}
